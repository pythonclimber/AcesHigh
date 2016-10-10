using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CardFramework.Decks;

namespace CardFramework
{
    public class BlackJackDealer : BlackJackPlayer
    {
        private Queue<Card> Shoe { get; set; }

        public BlackJackDealer() : base()
        {
        }
    }
}
