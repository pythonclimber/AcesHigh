using CardFramework.Decks;
using System.Collections.Generic;
using System.Linq;

namespace CardFramework {
    public class CribHand : Hand {
        public bool IsCrib { get; protected set; }

        public CribHand(bool isCrib) : this(new List<Card>(), isCrib) {

        }

        public CribHand(List<Card> cards)
            : this(cards, false) {
        }

        public CribHand(List<Card> cards, bool isCrib) : base(cards, cards.Count) {
            IsCrib = isCrib;
        }

        public override string DisplayHand() {
            var result = string.Empty;

            foreach (var card in Cards) {
                result += card.ToString();
            }

            return result;
        }

        /// <summary>
        /// Scores the players hand using a boolean to determine whether the player should be looking for Nobs.
        /// </summary>
        /// <param name="countNobs"></param>
        /// <param name="turnCard"></param>
        /// <returns>Returns player score</returns>
        public int GetScore(bool countNobs, Card turnCard) {
            var score = 0;
            var num3s = 0;
            var num4s = 0;
            var num5s = 0;
            var tempCards = new List<Card>(this.Cards);

            if (countNobs) {
                score += CountNobs(tempCards, turnCard);
            }

            if (turnCard != null) {
                tempCards.Add(turnCard);
            }

            score += CountFlush(tempCards);

            tempCards.Sort((c1, c2) => c1.FaceNum - c2.FaceNum);

            Card[] cards;
            for (int i = 2; i <= tempCards.Count; i++) {
                cards = new Card[i];
                this.CountFifteens(ref score, i, 0, cards, tempCards);
            }

            cards = new Card[2];
            CountSets(ref score, 2, 0, cards, tempCards);

            for (int i = 3; i <= tempCards.Count; i++) {
                cards = new Card[i];
                this.CountRuns(ref num3s, ref num4s, ref num5s, i, 0, cards, tempCards);
            }

            score += (num3s * 3) + (num4s * 4) + (num5s * 5);

            return score;
        }

        /// <summary>
        /// Used by the computer player to decide and remove which cards are being passed to the crib and pass them to CribPlayer object
        /// </summary>
        /// <param name="cardsToKeep"></param>
        /// <returns>Returns cards to be passed to the crib by a human player</returns>
        public List<Card> GetCardsToPass(List<Card> cardsToKeep) {
            var outCards = new List<Card>();

            foreach (var card in this.Cards) {
                if (cardsToKeep.IndexOf(card) == -1) {
                    outCards.Add(card);
                }
            }

            foreach (var card in outCards) {
                this.Cards.RemoveAt(this.Cards.IndexOf(card));
            }

            return outCards;
        }

        public void AddCards(IEnumerable<Card> cardsToAdd) {
            Cards.AddRange(cardsToAdd);
        }

        #region Protected Scoring and Sorting Methods

        /// <summary>
        /// Called from ScoreHand to count all possible combinations adding to 15
        /// </summary>
        /// <param name="score"></param>
        /// <param name="selected"></param>
        /// <param name="start"></param>
        /// <param name="cards"></param>
        /// <param name="tempCards"></param>
        protected virtual void CountFifteens(ref int score, int selected, int start, Card[] cards, List<Card> tempCards) {
            selected--;
            for (int i = start; i < tempCards.Count; i++) {
                cards[selected] = tempCards[i];
                if (selected == 0) {
                    var total = cards.Sum(c => c.FaceValue());

                    if (total == 15) {
                        score += 2;
                    }
                } else {
                    start++;
                    CountFifteens(ref score, selected, start, cards, tempCards);
                }
            }
        }

        /// <summary>
        /// Called from ScoreHand to count all  pairs existing in a hand.
        /// </summary>
        /// <param name="score"></param>
        /// <param name="selected"></param>
        /// <param name="start"></param>
        /// <param name="cards"></param>
        /// <param name="tempCards"></param>
        protected virtual void CountSets(ref int score, int selected, int start, Card[] cards, List<Card> tempCards) {
            selected--;
            for (int i = start; i < tempCards.Count; i++) {
                cards[selected] = tempCards[i];
                if (selected == 0) {
                    for (int j = cards.GetUpperBound(0) - 1; j >= cards.GetLowerBound(0); j--) {
                        for (int k = 1; k + j <= cards.GetUpperBound(0); k++) {
                            if (cards[j].FaceNum == cards[k].FaceNum) {
                                score += 2;
                            }
                        }
                    }
                } else {
                    start++;
                    CountSets(ref score, selected, start, cards, tempCards);
                }
            }
        }

        /// <summary>
        /// Called from ScoreHand to count all runs of 3 or more cards
        /// </summary>
        /// <param name="num3s"></param>
        /// <param name="num4s"></param>
        /// <param name="num5s"></param>
        /// <param name="score"></param>
        /// <param name="selected"></param>
        /// <param name="start"></param>
        /// <param name="cards"></param>
        /// <param name="tempCards"></param>
        protected virtual void CountRuns(ref int num3s, ref int num4s, ref int num5s, int selected, int start, Card[] cards, List<Card> tempCards) {
            selected--;
            for (int i = start; i < tempCards.Count; i++) {
                cards[selected] = tempCards[i];
                if (selected == 0) {
                    bool isRun = true;
                    for (int j = cards.GetUpperBound(0); j > cards.GetLowerBound(0); j--) {
                        if (cards[j - 1].FaceNum != (cards[j].FaceNum + 1)) {
                            isRun = false;
                            break;
                        }
                    }

                    if (isRun && cards.Length == 3) {
                        num3s++;
                    } else if (isRun && cards.Length == 4) {
                        num3s = 0;
                        num4s++;
                    } else if (isRun && cards.Length == 5) {
                        num4s = 0;
                        num5s++;
                    }
                } else {
                    start++;
                    CountRuns(ref num3s, ref num4s, ref num5s, selected, start, cards, tempCards);
                }
            }
        }

        /// <summary>
        /// Called from ScoreHand to determine if a player has Nobs
        /// </summary>
        /// <param name="score"></param>
        protected virtual int CountNobs(List<Card> tempCards, Card turnCard) {
            return tempCards.Any(c => c.Face == CardFaces.Jack.Key && c.Suit == turnCard.Suit)
                ? 1
                : 0;
        }

        /// <summary>
        /// Called from ScoreHand to determine if a player has a flush
        /// This must be overridden for the Crib as only a flush of all 5 cards counts for the crib
        /// </summary>
        protected virtual int CountFlush(List<Card> tempCards) {
            var cardsInFlush = 1;

            for (int i = 1; i < tempCards.Count; i++) {
                if (tempCards[i - 1].Suit != tempCards[i].Suit) {
                    break;
                } else {
                    cardsInFlush++;
                }
            }

            if (cardsInFlush == 5) {
                return 5;
            }
            if (cardsInFlush == 4 && !IsCrib) {
                return 4;
            }
            return 0;
        }

        #endregion
    }
}
