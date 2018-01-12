using CardFramework.Decks;
using CardFramework.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CardFramework {
    public enum PokerHandValues {
        None,
        HighCard,
        Pair,
        TwoPair,
        ThreeOfAKind,
        Straight,
        Flush,
        FullHouse,
        FourOfAKind,
        StraightFlush
    }

    public enum FullHouseTypes {
        None,
        ThreeLow,
        ThreeHigh
    }

    public class PokerPlayer : Player {
        #region Properties

        public List<Card> Hand { get; protected set; }

        public PokerHandValues HandValue { get; protected set; }

        public bool IsDealer { get; protected set; }

        public bool IsSmallBlind { get; protected set; }

        public bool IsBigBlind { get; protected set; }

        public decimal BankRoll { get; protected set; }

        public int HighCard { get; protected set; }

        public int HighSetValue { get; protected set; }

        public int LowSetValue { get; protected set; }

        public FullHouseTypes FullHouseType { get; protected set; }

        #endregion

        public PokerPlayer() {
            Hand = new List<Card>();
            BankRoll = 0M;
            HighCard = 0;
            HighSetValue = 0;
            LowSetValue = 0;
            FullHouseType = FullHouseTypes.None;
        }

        public override void ScoreHand() {
            if (Hand.Count == 5) {
                GetScore();
            } else {
                GetScoreByCombo();
            }
        }

        public void ScoreHandLegacy() {
            if (Hand.Count == 5) {
                GetScoreLegacy();
            } else {
                GetScoreByCombo();
            }
        }

        public override string DisplayHand() {
            throw new NotImplementedException();
        }

        public void ReceiveCards(IEnumerable<Card> cards) {
            Hand.AddRange(cards);
        }

        protected void GetScore() {
            var cards = new List<Card>(Hand);
            cards.Sort((c1, c2) => c1.FaceNum.CompareTo(c2.FaceNum));

            var containsAce = cards[0].Face == CardFaces.Ace.Key;

            CheckHandForStraight(cards, containsAce);

            CheckHandForFlush(cards, containsAce);


            if (HandValue == PokerHandValues.None) {
                CheckHandForSet(cards, containsAce);
            }
        }

        protected void GetScoreLegacy() {
            var cards = new List<Card>(Hand);
            cards.Sort((c1, c2) => c1.FaceNum.CompareTo(c2.FaceNum));

            var containsAce = cards[0].Face == CardFaces.Ace.Key;

            CheckHandForStraight(cards, containsAce);

            CheckHandForFlush(cards, containsAce);


            if (HandValue == PokerHandValues.None) {
                CheckHandForSetLegacy(cards, containsAce);
            }

            if (HandValue == PokerHandValues.None) {
                HandValue = PokerHandValues.HighCard;
                HighCard = cards.Any(c => c.Face == CardFaces.Ace.Key)
                    ? CardFaces.AceHigh.Value
                    : cards.Max(c => c.FaceNum);
            }
        }

        protected void GetScoreByCombo() {
            var tempCards = new List<Card>(Hand);
            tempCards.Sort((c1, c2) => c1.FaceNum.CompareTo(c2.FaceNum));
        }

        protected void CheckHandForStraight(List<Card> cards, bool containsAce) {
            //Cards passed in should always be sorted before
            var maxCardValue = cards[cards.Count - 1].FaceNum;
            var minCardValue = cards[0].FaceNum;
            var containsKing = cards.Any(tc => tc.Face == CardFaces.King.Key);
            var isStraight = true;

            if (maxCardValue - minCardValue == 4 || containsKing && containsAce) {

                //If hand contains both Ace and King, start at 1 to skip the ace which is the first card in the list
                var startValue = containsAce && containsKing ? 1 : 0;

                for (var i = startValue; i < cards.Count - 1; i++) {
                    if (cards[i].FaceNum + 1 != cards[i + 1].FaceNum) {
                        isStraight = false;
                        break;
                    }
                }

                if (isStraight) {
                    HandValue = PokerHandValues.Straight;
                }
            }
        }

        protected void CheckHandForFlush(List<Card> cards, bool containsAce) {
            if (cards.All(tc => tc.Suit == cards[0].Suit)) {
                if (HandValue == PokerHandValues.Straight) {
                    HandValue = PokerHandValues.StraightFlush;
                    if (containsAce) {
                        HighCard = cards[cards.Count - 1].Face == CardFaces.King.Key
                            ? CardFaces.AceHigh.Value
                            : cards[cards.Count - 1].FaceNum;
                    } else {
                        HighCard = cards[cards.Count - 1].FaceNum;
                    }
                } else {
                    HandValue = PokerHandValues.Flush;
                    HighCard = containsAce
                        ? CardFaces.AceHigh.Value
                        : cards[cards.Count - 1].FaceNum;
                }
            }
        }

        protected void CheckHandForSet(List<Card> cards, bool containsAce) {
            var cardsIndex = new Dictionary<int, int>();

            foreach (var card in cards) {
                if (cardsIndex.ContainsKey(card.FaceNum)) {
                    cardsIndex[card.FaceNum]++;
                } else {
                    cardsIndex.Add(card.FaceNum, 1);
                }
            }

            if (cardsIndex.Count == 5) {
                HandValue = PokerHandValues.HighCard;
                HighCard = containsAce
                    ? CardFaces.AceHigh.Value
                    : cards[cards.Count - 1].FaceNum;
            } else if (cardsIndex.Count == 4) {
                HandValue = PokerHandValues.Pair;
                var pairValue = cardsIndex.Single(ci => ci.Value == 2).Key;
                HighSetValue = pairValue == CardFaces.Ace.Value
                    ? CardFaces.AceHigh.Value
                    : pairValue;
                HighCard = containsAce && HighSetValue != CardFaces.AceHigh.Value
                    ? CardFaces.AceHigh.Value
                    : cards.Where(c => c.FaceNum != HighSetValue).Max(c => c.FaceNum);
            } else if (cardsIndex.Count == 3) {
                if (cardsIndex.Any(ci => ci.Value == 3)) {
                    HandValue = PokerHandValues.ThreeOfAKind;
                    var threeKindValue = cardsIndex.Single(ci => ci.Value == 3).Key;
                    HighSetValue = threeKindValue == CardFaces.Ace.Value
                        ? CardFaces.AceHigh.Value
                        : threeKindValue;
                    HighCard = containsAce && threeKindValue != CardFaces.Ace.Value
                        ? CardFaces.AceHigh.Value
                        : cards.Where(c => c.FaceNum != threeKindValue).Max(c => c.FaceNum);
                } else {
                    var pairValues = cardsIndex.Where(ci => ci.Value == 2).OrderBy(ci => ci.Key);
                    var kickerValue = cardsIndex.Single(ci => ci.Value == 1).Key;

                    HandValue = PokerHandValues.TwoPair;
                    HighCard = kickerValue == CardFaces.Ace.Value
                        ? CardFaces.AceHigh.Value
                        : kickerValue;
                    if (pairValues.First().Key == CardFaces.Ace.Value) {
                        HighSetValue = CardFaces.AceHigh.Value;
                        LowSetValue = pairValues.Single(pv => pv.Key != CardFaces.Ace.Value).Key;
                    } else {
                        LowSetValue = pairValues.First().Key;
                        HighSetValue = pairValues.Single(pv => pv.Key != LowSetValue).Key;
                    }
                }
            } else if (cardsIndex.Count == 2) {
                if (cardsIndex.Any(ci => ci.Value == 4)) {
                    // 4 of a kind
                    var fourKindValue = cardsIndex.Single(ci => ci.Value == 4).Key;
                    var kickerValue = cardsIndex.Single(ci => ci.Value == 1).Key;

                    HandValue = PokerHandValues.FourOfAKind;
                    HighSetValue = fourKindValue == CardFaces.Ace.Value
                        ? CardFaces.AceHigh.Value
                        : fourKindValue;
                    HighCard = kickerValue == CardFaces.Ace.Value
                        ? CardFaces.AceHigh.Value
                        : kickerValue;
                } else {
                    // Full  house
                    var setValues = cardsIndex.OrderBy(ci => ci.Key);

                    HandValue = PokerHandValues.FullHouse;
                    LowSetValue = setValues.First().Key;
                    HighSetValue = setValues.Single(sv => sv.Key != LowSetValue).Key;
                    FullHouseType = setValues.First().Value == 3
                        ? FullHouseTypes.ThreeLow
                        : FullHouseTypes.ThreeHigh;
                }
            }
        }

        protected int GetCardValue(Card card) {
            return card.Face == CardFaces.Ace.Key ? CardFaces.AceHigh.Value : card.FaceNum;
        }

        protected int GetCardValue(int cardValue) {
            return cardValue == CardFaces.Ace.Value ? CardFaces.AceHigh.Value : cardValue;
        }

        protected void CheckHandForSetLegacy(List<Card> cards, bool containsAce) {
            Card card = null;
            for (int i = 0; i < cards.Count - 1; i++) {
                if (cards[i].FaceNum == cards[i + 1].FaceNum) {
                    if (HandValue == PokerHandValues.ThreeOfAKind) {
                        if (cards[i + 1].FaceNum == card.FaceNum) {
                            HandValue = PokerHandValues.FourOfAKind;
                            HighSetValue = GetCardValue(cards[i]);
                            LowSetValue = 0;
                            if ((i + 1) == cards.Count - 1) {
                                HighCard = GetCardValue(cards[0]);
                            } else {
                                HighCard = GetCardValue(cards[cards.Count - 1]);
                            }
                        } else {
                            HandValue = PokerHandValues.FullHouse;
                            HighSetValue = GetCardValue(cards[i]);
                            LowSetValue = GetCardValue(cards[i - 1]);
                            FullHouseType = FullHouseTypes.ThreeLow;
                        }
                    } else if (HandValue == PokerHandValues.TwoPair) {
                        HandValue = PokerHandValues.FullHouse;
                        HighSetValue = GetCardValue(cards[i]);
                        LowSetValue = GetCardValue(cards[i - 2]);
                        FullHouseType = FullHouseTypes.ThreeHigh;
                    } else if (HandValue == PokerHandValues.Pair) {
                        if (cards[i + 1].FaceNum == card.FaceNum) {
                            HandValue = PokerHandValues.ThreeOfAKind;
                            HighSetValue = GetCardValue(cards[i]);
                            LowSetValue = 0;
                            if ((i + 1) == cards.Count - 1) {
                                HighCard = GetCardValue(containsAce && !card.IsAce() ? cards[0] : cards[1]);
                            } else {
                                HighCard =
                                    GetCardValue(containsAce && !card.IsAce() ? cards[0] : cards.Last());
                            }
                        } else {
                            HandValue = PokerHandValues.TwoPair;
                            HighSetValue = GetCardValue(cards[i]);
                            LowSetValue = GetCardValue(card);
                            if ((i + 1) == cards.Count - 1) {
                                if (cards.Min(c => c.FaceNum) == card.FaceNum) {
                                    HighCard = GetCardValue(cards.Middle());
                                } else {
                                    HighCard = GetCardValue(cards.First());
                                }
                            } else {
                                HighCard = GetCardValue(cards.Last());
                            }
                        }
                    } else if (HandValue == PokerHandValues.None) {
                        HandValue = PokerHandValues.Pair;
                        card = cards[i];
                        HighSetValue = GetCardValue(cards[i]);
                        if (i + 1 == cards.Count - 1) {
                            HighCard = GetCardValue(containsAce ? cards[0] : cards[i - 1]);
                        } else {
                            HighCard = GetCardValue(containsAce && !card.IsAce() ? cards[0] : cards.Last());
                        }
                    }
                }
            }
        }
    }
}
