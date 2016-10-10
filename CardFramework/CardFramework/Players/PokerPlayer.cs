using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CardFramework.Decks;
using CardFramework.Helpers;

namespace CardFramework
{
    public enum PokerHandValues
    {
        None,
        HighCard,
        Pair,
        TwoPair,
        ThreeKind,
        Straight,
        Flush,
        FullHouse,
        FourKind,
        StraightFlush,
        RoyalFlush
    }

    public enum FullHouseTypes
    {
        None,
        ThreeLow,
        ThreeHigh
    }

    public class PokerPlayer : Player
    {
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

        public PokerPlayer()
        {
            Hand = new List<Card>();
            BankRoll = 0M;
            HighCard = 0;
            HighSetValue = 0;
            LowSetValue = 0;
            FullHouseType = FullHouseTypes.None;
        }

        public override void ScoreHand()
        {
            if (Hand.Count == 5)
            {
                GetScore();
            }
            else
            {
                GetScoreByCombo();
            }
        }

        public override string DisplayHand()
        {
            throw new NotImplementedException();
        }

        public void ReceiveCards(IEnumerable<Card> cards)
        {
            Hand.AddRange(cards);
        }

        protected void GetScore()
        {
            var tempCards = new List<Card>(Hand);
            tempCards.Sort((c1, c2) => c1.FaceNum.CompareTo(c2.FaceNum));

            //Determines if hand is a straight
            #region Straight Logic

            var maxCardValue = tempCards.Max(tc => tc.FaceNum);
            var minCardValue = tempCards.Min(tc => tc.FaceNum);
            var containsAce = tempCards.Any(tc => tc.Face == CardFaces.Ace);
            var containsKing = tempCards.Any(tc => tc.Face == CardFaces.King);
            if ((maxCardValue - minCardValue == 4) || (containsKing && containsAce))
            {
                var straight = true;
                //If hand contains both Ace and King, start at 1 to skip the ace which is the first card in the list
                var startValue = containsAce && containsKing ? 1 : 0;
                for (var i = startValue; i < tempCards.Count - 1; i++)
                {
                    if (tempCards[i].FaceNum + 1 != tempCards[i + 1].FaceNum)
                    {
                        straight = false;
                        break;
                    }
                }

                if (straight)
                {
                    HandValue = PokerHandValues.Straight;
                    HighCard = GetHighCardValueForNonSet(tempCards);
                }
            }

            #endregion

            //Determines if hand is a flush or straightflush (dependent on results of above region)
            #region Flush Logic

            if (tempCards.All(tc => tc.Suit == tempCards[0].Suit))
            {
                HandValue = HandValue == PokerHandValues.Straight
                    ? (containsKing && containsAce ? PokerHandValues.RoyalFlush : PokerHandValues.StraightFlush)
                    : PokerHandValues.Flush;
                HighCard = GetHighCardValueForNonSet(tempCards);
            }

            #endregion

            //Determines if hand contains a set (fourKind, fullHouse, pair, etc)
            #region Set Logic
            if (HandValue == PokerHandValues.None)
            {
                Card tempCard = null;
                for (int i = 0; i < tempCards.Count - 1; i++)
                {
                    if (tempCards[i].FaceNum == tempCards[i + 1].FaceNum)
                    {
                        if (HandValue == PokerHandValues.ThreeKind)
                        {
                            if (tempCards[i + 1].FaceNum == tempCard.FaceNum)
                            {
                                HandValue = PokerHandValues.FourKind;
                                HighSetValue = GetCardValueForSet(tempCards[i]);
                                LowSetValue = 0;
                                if ((i + 1) == tempCards.Count - 1)
                                {
                                    HighCard = GetCardValueForSet(tempCards[0]);
                                }
                                else
                                {
                                    HighCard = GetCardValueForSet(tempCards[tempCards.Count - 1]);
                                }
                            }
                            else
                            {
                                HandValue = PokerHandValues.FullHouse;
                                HighSetValue = GetCardValueForSet(tempCards[i]);
                                LowSetValue = GetCardValueForSet(tempCards[i - 1]);
                                FullHouseType = FullHouseTypes.ThreeLow;
                            }
                        }
                        else if (HandValue == PokerHandValues.TwoPair)
                        {
                            HandValue = PokerHandValues.FullHouse;
                            HighSetValue = GetCardValueForSet(tempCards[i]);
                            LowSetValue = GetCardValueForSet(tempCards[i - 2]);
                            FullHouseType = FullHouseTypes.ThreeHigh;
                        }
                        else if (HandValue == PokerHandValues.Pair)
                        {
                            if (tempCards[i + 1].FaceNum == tempCard.FaceNum)
                            {
                                HandValue = PokerHandValues.ThreeKind;
                                HighSetValue = GetCardValueForSet(tempCards[i]);
                                LowSetValue = 0;
                                if ((i + 1) == tempCards.Count - 1)
                                {
                                    HighCard = GetCardValueForSet(containsAce && !tempCard.IsAce() ? tempCards[0] : tempCards[1]);
                                }
                                else
                                {
                                    HighCard =
                                        GetCardValueForSet(containsAce && !tempCard.IsAce() ? tempCards[0] : tempCards.Last());
                                }
                            }
                            else
                            {
                                HandValue = PokerHandValues.TwoPair;
                                HighSetValue = GetCardValueForSet(tempCards[i]);
                                LowSetValue = GetCardValueForSet(tempCard);
                                if ((i + 1) == tempCards.Count - 1)
                                {
                                    if (tempCards.Min(c => c.FaceNum) == tempCard.FaceNum)
                                    {
                                        HighCard = GetCardValueForSet(tempCards.Middle());
                                    }
                                    else
                                    {
                                        HighCard = GetCardValueForSet(tempCards.First());
                                    }
                                }
                                else
                                {
                                    HighCard = GetCardValueForSet(tempCards.Last());
                                }
                            }
                        }
                        else if (HandValue == PokerHandValues.None)
                        {
                            HandValue = PokerHandValues.Pair;
                            tempCard = tempCards[i];
                            HighSetValue = GetCardValueForSet(tempCards[i]);
                            if ((i + 1) == tempCards.Count - 1)
                            {
                                HighCard = GetCardValueForSet(containsAce ? tempCards[0] : tempCards[i - 1]);
                            }
                            else
                            {
                                HighCard = GetCardValueForSet(containsAce && !tempCard.IsAce() ? tempCards[0] : tempCards.Last());
                            }
                        }
                    }
                }
            }
            #endregion

            //Sets hand to highCard value if applicable and sets numeric value of high card for comparison
            #region High Card
            if (HandValue == PokerHandValues.None)
            {
                HandValue = PokerHandValues.HighCard;
                HighCard = GetHighCardValueForNonSet(tempCards);
            }
            #endregion

        }

        protected void GetScoreByCombo()
        {
            var tempCards = new List<Card>(Hand);
            tempCards.Sort((c1, c2) => c1.FaceNum.CompareTo(c2.FaceNum));
        }

        protected int GetHighCardValueForNonSet(List<Card> tempCards)
        {
            return tempCards.Any(tc => tc.Face == CardFaces.Ace) ? 14 : tempCards.Max(tc => tc.FaceNum);
        }

        protected int GetCardValueForSet(Card card)
        {
            return card.Face == CardFaces.Ace ? 14 : card.FaceNum;
        }
    }
}
