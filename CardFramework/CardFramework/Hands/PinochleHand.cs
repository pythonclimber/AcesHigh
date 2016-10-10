using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CardFramework.Decks;

namespace CardFramework
{
    public class PinochleHand : Hand
    {
        public PinochleHand(int numCards) : this(numCards, new List<Card>())
        {
        }

        public PinochleHand(int numCards, List<Card> cards) : base(cards, numCards)
        {
            
        }

        public override void ScoreHand()
        {
            throw new NotImplementedException();
        }

        public int CountMeld(string trump)
        {
            var total = 0;

            total += CountPinochles();
            total += CountSets("Ace", 10);
            total += CountSets("King", 8);
            total += CountSets("Queen", 6);
            total += CountSets("Jack", 4);
            total += CountMarriages(trump);
            total += CountDix(trump);

            return total;
        }

        public override string DisplayHand()
        {
            var result = string.Empty;

            int index = 0;
            foreach (var card in Cards)
            {
                result += string.Format("{0}. {1}\n", ++index, card.ToString());
            }

            return result;
        }

        public virtual void AddCards(List<Card> cards)
        {
            Cards.AddRange(cards);
        }

        public virtual void SortHand()
        {
            Cards.Sort((c1, c2) =>
                       {
                           return c1.Index.CompareTo(c2.Index);
                       });
        }

        protected int CountPinochles()
        {
            var total = 0;
            var jacks = Cards.Where(c => c.Face == "Jack" && c.Suit == "Diamonds").ToList();
            var queens = Cards.Where(c => c.Face == "Queen" && c.Suit == "Spades").ToList();

            if (jacks.Count == 2 && queens.Count == 2)
            {
                total = 30;
            }
            else if (jacks.Count > 0 && queens.Count > 0)
            {
                total = 4;
            }

            return total;
        }

        protected int CountSets(string face, int value)
        {
            int total = 0;
            var cards = Cards.Where(c => c.Face == face).ToList();

            if (cards.Count == 8)
            {
                total = value*10;
            }
            else if (cards.Count > 3)
            {
                string[] suits = {"Clubs", "Hearts", "Spades", "Diamonds"};
                var matchCount = 0;
                foreach (var suit in suits)
                {
                    if (cards.Any(c => c.Suit == suit))
                    {
                        matchCount++;
                    }
                }

                if (matchCount == 4)
                {
                    total = value;
                }
            }

            return total;
        }

        protected int CountMarriages(string trump)
        {
            var total = 0;
            var cards = new List<Card>(Cards);
            var queens = cards.Where(c => c.Face == "Queen").ToList();

            foreach (var queen in queens)
            {
                var king = cards.FirstOrDefault(c => c.Face == "King" && c.Suit == queen.Suit);

                if (king != null)
                {
                    total += queen.Suit == trump ? 4 : 2;
                    cards.Remove(king);
                }
            }

            return total;
        }

        protected int CountDix(string trump)
        {
            var total = 0;

            var nines = Cards.Where(c => c.Face == "Nine" && c.Face == trump).ToList();

            total = nines.Count;

            return total;
        }
    }
}
