using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CardFramework.Helpers;

namespace CardFramework
{
    public class BlackJackHand : Hand
    {
        public int HandValue { get; set; }
        public BlackJackHand()
        {
            NumCards = 2;
        }

        //Returns the total of all cards in the hand.
        public override void ScoreHand()
        {
            HandValue = Cards.Sum(c => c.FaceValue());
        }

        public override string DisplayHand()
        {
            throw new NotImplementedException();
        }
    }
}
