using CardFramework.Decks;
using System;
using System.Collections.Generic;

namespace CardFramework {
    public class BlackJackPlayer : Player {
        protected List<Card> Hand;
        public int NumCards { get; protected set; }

        public BlackJackPlayer() {
            NumCards = 2;
        }

        public override string DisplayHand() {
            throw new NotImplementedException();
        }
    }
}
