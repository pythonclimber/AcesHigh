using CardFramework.Decks;
using System.Collections.Generic;

namespace CardFramework {
    public class PinochlePlayer : Player {
        private PinochleHand hand;

        public PinochlePlayer() {
            hand = new PinochleHand(12);
        }

        public override string DisplayHand() {
            return hand.DisplayHand();
        }

        public virtual void TakeCards(List<Card> cards) {
            hand.AddCards(cards);
            hand.SortHand();
        }

        public virtual int CountMeld(string trump) {
            return hand.CountMeld(trump);
        }
    }
}
