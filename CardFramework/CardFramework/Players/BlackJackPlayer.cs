using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CardFramework.Decks;

namespace CardFramework
{
    public class BlackJackPlayer : Player
    {
        protected List<Card> Hand;
        public int NumCards { get; protected set; }

        public BlackJackPlayer()
        {
            NumCards = 2;
        }

        public override void ScoreHand()
        {
            throw new NotImplementedException();
        }

        public override string DisplayHand()
        {
            throw new NotImplementedException();
        }
    }
}
