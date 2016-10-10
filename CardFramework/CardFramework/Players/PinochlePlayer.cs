using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CardFramework.Decks;

namespace CardFramework
{
    public class PinochlePlayer : Player
    {
        private PinochleHand hand;

        public PinochlePlayer()
        {
            hand = new PinochleHand(12);
        }

        public override string DisplayHand()
        {
            return hand.DisplayHand();
        }

        public override void ScoreHand()
        {
            throw new NotImplementedException();
        }

        public virtual void TakeCards(List<Card> cards)
        {
            hand.AddCards(cards);
            hand.SortHand();
        }

        public virtual int CountMeld(string trump)
        {
            return hand.CountMeld(trump);
        }
    }
}
