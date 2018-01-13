using CardFramework.Decks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CardFramework {
    public class CribPlayer : Player {
        private CribHand _hand;
        private bool _isComputer;
        //private bool _isDealer;
        private List<Card> _crib;

        public bool IsDealer { get; set; }
        public CribPlayer() : this(false, false) {
        }

        public CribPlayer(bool isComputer, bool isDealer) {
            _hand = new CribHand(false);
            _isComputer = isComputer;
            IsDealer = isDealer;
        }

        public void CountHand(Card turnCard) {
            _hand.GetScore(true, turnCard);
        }

        public void CountCrib(Card turnCard) {
            if (!_isDealer) {
                throw new ApplicationException("Only the dealer is allowed to count the crib");
            }

            //todo: handle crib scoring
        }

        public void TakeCards(List<Card> cardsToAdd) {
            _hand.AddCards(cardsToAdd);
        }

        public List<Card> PassCardsToCrib(int index1, int index2, List<Card> crib) {
            var cardsToPass = new List<Card> { _hand.Cards[index1], _hand.Cards[index2] };

            _hand.Cards.RemoveAt(index1);
            _hand.Cards.RemoveAt(index2);

            return cardsToPass;
        }

        public void ChooseCardsForCrib(List<Card> crib) {
            if (IsComputer) {
                var cards = new Card[4];
                var combos = new List<CardCombination>();
                var cardsToScore = new List<Card>(_hand.Cards);
                var max = 0;
                CountTotalByCombo(4, 6, 0, cards, cardsToScore, combos);

                for (var i = 1; i < combos.Count; i++) {
                    if (combos[i].Score > combos[max].Score) {
                        max = i;
                    }
                }

                var tempCards = _hand.Cards.Where(c => combos[max].Cards.IndexOf(c) == -1);

                crib.AddRange(tempCards);
            }
        }

        public override string DisplayHand() {
            return _hand.DisplayHand();
        }

        public static int CompCardToPlay(List<Card> playable, List<Card> played, int totalPlayed) {
            int play = -1;
            int indexFifteen;
            int indexThirtyOne;
            //int pairSet;
            int indexStraight;
            int indexSet;

            if (played.Count < 2) {
                //If only one card has been played only a pair or a fifteen is possible as a run needs 3 cards and total can't be near 31 yet.
                if (played.Count > 0) {
                    indexFifteen = CanMakeFifteen(totalPlayed, playable);
                    indexSet = CanMakeSet(played, playable, totalPlayed);
                } else {
                    //If zero cards are played then both will be negative 1.
                    indexFifteen = -1;
                    indexSet = -1;
                }

                //Choose fifteen over pair because they score the same
                //and the pair gives the opponent the ability to score 6.
                if (indexFifteen > -1) {
                    play = indexFifteen;
                } else if (indexSet > -1) {
                    play = indexSet;
                } else {
                    int max = 0;
                    for (int i = 0; i < playable.Count; i++) {
                        if (playable[i].FaceNum > playable[max].FaceNum) {
                            max = i;
                        }
                    }

                    play = max;
                }
            } else {
                indexFifteen = CanMakeFifteen(totalPlayed, playable);
                indexThirtyOne = CanMakeThirtyOne(totalPlayed, playable);
                indexSet = CanMakeSet(played, playable, totalPlayed);
                indexStraight = CanMakeStraight(playable, played, totalPlayed);

                if (indexStraight > -1) {
                    play = indexStraight;
                } else if (indexSet > -1) {
                    play = indexSet;
                } else if (indexFifteen > -1) {
                    play = indexFifteen;
                } else if (indexThirtyOne > -1) {
                    play = indexThirtyOne;
                } else {
                    int max = 0;
                    for (int i = 0; i < playable.Count; i++) {
                        if (playable[i].FaceNum > playable[max].FaceNum && playable[i].FaceNum + totalPlayed <= 31) {
                            max = i;
                        }
                    }

                    if (max == 0 && playable[max].FaceNum + totalPlayed <= 31) {
                        play = max;
                    }
                }
            }

            return play;
        }

        public void ClearLastHand() {
            _hand = new CribHand(false);
        }

        protected void CountTotalByCombo(int selected, int total, int start, Card[] cards, List<Card> tempHand, List<CardCombination> combos) {
            selected--;
            for (int i = start; i < tempHand.Count; i++) {
                cards[selected] = tempHand[i];
                if (selected == 0) {
                    var comboToScore = new CribHand(cards.ToList());
                    var score = comboToScore.GetScore(false, null);
                    combos.Add(new CardCombination(comboToScore.Cards, score));
                } else {
                    start++;
                    CountTotalByCombo(selected, total, start, cards, tempHand, combos);
                }
            }
        }

        private static int CanMakeStraight(List<Card> playable, List<Card> played, int totalPlayed) {
            List<Card> cards = new List<Card>();

            for (int i = 0; i < playable.Count; i++) {
                foreach (Card card in played) {
                    cards.Add(card);
                }
                cards.Add(playable[i]);
                //CribHand.Sort(cards);
                if (IsStraight(cards.ToArray()) && playable[i].FaceNum + totalPlayed <= 31) {
                    return i;
                }

                cards.Clear();
            }

            return -1;
        }

        private static bool IsStraight(Card[] cards) {
            for (int i = 0; i < cards.GetUpperBound(0); i++) {
                if (cards[i].FaceNum + 1 != cards[i + 1].FaceNum) {
                    return false;
                }
            }

            return true;
        }

        private static int CanMakeFifteen(int totalPlayed, List<Card> playable) {

            for (int i = 0; i < playable.Count; i++) {
                if (playable[i].FaceNum + totalPlayed == 15) {
                    return i;
                }
            }

            return -1;
        }

        private static int CanMakeThirtyOne(int totalPlayed, List<Card> playable) {

            for (int i = 0; i < playable.Count; i++) {
                if (playable[i].FaceNum + totalPlayed == 31) {
                    return i;
                }
            }

            return -1;
        }

        private static int CanMakeSet(List<Card> played, List<Card> playable, int totalPlayed) {
            for (int i = 0; i < playable.Count; i++) {
                if (playable[i].FaceNum == played[played.Count - 1].FaceNum && playable[i].FaceNum + totalPlayed <= 31) {
                    return i;
                }
            }


            return -1;
        }
    }
}
