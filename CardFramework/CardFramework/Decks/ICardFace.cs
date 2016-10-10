using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardFramework.Decks
{
    public interface ICardFace
    {
        KeyValuePair<int, string> Face { get; set; }
        KeyValuePair<int, string> Suit { get; set; }
    }
}
