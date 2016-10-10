using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardFramework.Decks
{
    public class StandardCardFace : ICardFace
    {
        public KeyValuePair<int, string> Face { get; set; }

        public KeyValuePair<int, string> Suit { get; set; }
    }
}
