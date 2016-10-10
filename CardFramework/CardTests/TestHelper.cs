using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardFramework.Decks;

namespace CardTests
{
    internal class TestHelper
    {
        private const int SeedValue = 367;

        internal string GetRandomSuit(int seed)
        {
            var random = new Random(seed);
            switch (random.Next() % 4)
            {
                case 0:
                    return CardSuits.Clubs;
                case 1:
                    return CardSuits.Hearts;
                case 2:
                    return CardSuits.Spades;
                default:
                    return CardSuits.Diamonds;
            }
        }
    }
}
