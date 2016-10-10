using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CardFramework.Decks;
using CardFramework.Helpers;

namespace CardFramework
{
    public class EuchrePlayer : Player
    {
        public List<Card> Hand { get; protected set; }

        public EuchrePlayer MyPartner { get; protected set; }

        public bool IsDealer { get; internal set; }

        public string Name { get; protected set; }

        public bool IsTrumpCaller { get; internal set; }

        public EuchrePlayer(bool isComp, EuchrePlayer myPartner) : base(isComp, 0)
        {
            MyPartner = myPartner;
            Hand = new List<Card>();
        }

        public Card PlayCard(string trumpSuit, EuchrePlayer trickWinner, List<Card> playedThisHand, List<Card> playedThisTrick)
        {
            //If my partner is already winning the trick, then I don't want to out-play him unless I have to
            var tryingToWin = !IsTrickWinnerMyPartner(trickWinner);
            //If my partner IsComputer then I am one of the live player's opponents
            var isLivePlayerOpponent = MyPartner.IsComputer;

            if (playedThisTrick.IsEmpty() && playedThisHand.IsEmpty())
            {
                var rightBower = Hand.FirstOrDefault(c => c.Face == CardFaces.Jack && c.Suit == trumpSuit);
                var leftBower =
                    Hand.FirstOrDefault(c => c.Face ==  CardFaces.Jack && c.Suit == trumpSuit.GetOppositeSuit());
                if (!rightBower.IsNull())
                {
                    return rightBower;
                }

                if (MyPartner.IsTrumpCaller && !leftBower.IsNull())
                {
                    return leftBower;
                }

                var nonTrumpAce = Hand.FirstOrDefault(c => c.Face == CardFaces.Ace && c.Suit != trumpSuit);
                if (!nonTrumpAce.IsNull())
                {
                    return nonTrumpAce;
                }

                var nonTrumpKing = Hand.FirstOrDefault(c => c.Face == CardFaces.King && c.Suit != trumpSuit);
                if (!nonTrumpKing.IsNull())
                {
                    return nonTrumpKing;
                }

                var nonTrumpLowCard = Hand.FirstOrDefault(c => c.Suit != trumpSuit);
                if (!nonTrumpLowCard.IsNull())
                {
                    return nonTrumpLowCard;
                }

                return Hand.RandomItem();
            }
            else if (playedThisTrick.IsEmpty() && !playedThisHand.IsEmpty())
            {
                return Hand.RandomItem();
            }
            else
            {
                return Hand.RandomItem();
            }
        }

        private bool IsTrickWinnerMyPartner(EuchrePlayer trickWinner)
        {
            var result = trickWinner == MyPartner;
            return result;
        }

        public override string DisplayHand()
        {
            throw new NotImplementedException();
        }

        public override void ScoreHand()
        {
            throw new NotImplementedException();
        }
    }
}
