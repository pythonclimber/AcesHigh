using System;

namespace CardFramework {
    public class BlackJackHand : Hand {
        public int HandValue { get; set; }
        public BlackJackHand() {
            NumCards = 2;
        }

        public override string DisplayHand() {
            throw new NotImplementedException();
        }
    }
}
