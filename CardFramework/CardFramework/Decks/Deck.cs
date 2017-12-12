using System;
using System.Collections.Generic;

namespace CardFramework.Decks
{
    public abstract class Deck
    {
        public List<Card> Cards { get; protected set; }

        public bool IsEmpty => Cards.Count == 0;

        protected Deck()
        {
            Cards = new List<Card>();
        }

        public virtual Deck Shuffle()
        {
            Random random = new Random();
            Card card;
            int index;


            for (int i = 0; i < Cards.Count; i++)
            {
                index = random.Next(Cards.Count);

                card = Cards[i];
                Cards[i] = Cards[index];
                Cards[index] = card;
            }

            return this;
        }

        public virtual void ReOrder()
        {
            Cards.Sort((c1, c2) => c1.Index.CompareTo(c2.Index));
        }

        public virtual string Display()
        {
            var result = string.Empty;
            Cards.ForEach(c => result += c.ToString() + Environment.NewLine);
            return result;
        }

        public virtual List<Card> DealCards(int numToDeal = 1)
        {
            var cards = new List<Card>();

            for (int i = 0; i < numToDeal; i++)
            {
                if (cards.Count > 0)
                {
                    cards.Add(cards[0]);
                    cards.RemoveAt(0);
                }
            }

            return cards;
        }

        protected void CreateCards(List<string> faces, List<string> suits, int numCards = 1)
        {
            var index = 0;
            foreach (var suit in suits)
            {
                var faceNum = 1;
                foreach (var face in faces)
                {
                    for (int i = 0; i < numCards; i++)
                    {
                        Cards.Add(new Card(face, faceNum, suit, index++));
                    }
                    faceNum++;
                }
            }
        }

        protected abstract List<string> GetFaces();

        protected virtual List<string> GetSuits()
        {
            return new List<string>
                       {
                           CardSuits.Clubs,
                           CardSuits.Hearts,
                           CardSuits.Spades,
                           CardSuits.Diamonds
                       };
        }
    }
}
