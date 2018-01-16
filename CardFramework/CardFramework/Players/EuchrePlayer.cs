using CardFramework.Decks;
using CardFramework.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CardFramework {
    public class EuchrePlayer : Player {
        public List<Card> Hand { get; protected set; }

        public EuchrePlayer MyPartner { get; protected set; }

        public bool IsDealer { get; internal set; }

        public string Name { get; protected set; }

        public bool IsTrumpCaller { get; internal set; }

        public EuchrePlayer(bool isComp, EuchrePlayer myPartner) : base(isComp, 0) {
            MyPartner = myPartner;
            Hand = new List<Card>();
        }

        public Card PlayCard(string trumpSuit, EuchrePlayer trickWinner, List<Card> playedThisHand, List<Card> playedThisTrick) {
            //If my partner is already winning the trick, then I don't want to out-play him unless I have to
            var tryingToWin = !IsTrickWinnerMyPartner(trickWinner);
            //If my partner IsComputer then I am one of the live player's opponents
            var isLivePlayerOpponent = MyPartner.IsComputer;

            if (playedThisTrick.Count == 0 && playedThisHand.Count == 0) {
                var rightBower = Hand.FirstOrDefault(c => c.Face == CardFaces.Jack.Key && c.Suit == trumpSuit);
                var leftBower =
                    Hand.FirstOrDefault(c => c.Face == CardFaces.Jack.Key && c.Suit == trumpSuit.GetOppositeSuit());
                if (rightBower != null) {
                    return rightBower;
                }

                if (MyPartner.IsTrumpCaller && leftBower != null) {
                    return leftBower;
                }

                var nonTrumpAce = Hand.FirstOrDefault(c => c.Face == CardFaces.AceHigh.Key && c.Suit != trumpSuit);
                if (nonTrumpAce != null) {
                    return nonTrumpAce;
                }

                var nonTrumpKing = Hand.FirstOrDefault(c => c.Face == CardFaces.King.Key && c.Suit != trumpSuit);
                if (nonTrumpKing != null) {
                    return nonTrumpKing;
                }

                var nonTrumpLowCard = Hand.FirstOrDefault(c => c.Suit != trumpSuit);
                if (nonTrumpLowCard != null) {
                    return nonTrumpLowCard;
                }

                return Hand.RandomItem();
            } else if (playedThisTrick.Count == 0 && playedThisHand.Count > 0) {
                return Hand.RandomItem();
            } else {
                return Hand.RandomItem();
            }
        }

        private bool IsTrickWinnerMyPartner(EuchrePlayer trickWinner) {
            var result = trickWinner == MyPartner;
            return result;
        }

        public override string DisplayHand() {
            throw new NotImplementedException();
        }
    }
}
