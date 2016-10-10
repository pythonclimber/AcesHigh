using System;
using System.Collections.Generic;

namespace CardFramework.Decks
{
    public class SixDeckShoe : IDisposable
    {
        private readonly List<Card> Shoe;
        protected const int NumCards = 312;
        protected const int NumDecks = 6;

        public List<Card> Cards
        {
            get { return Shoe; }
        }

        public SixDeckShoe()
        {
            var decks = new List<StandardDeck>();

            Shoe = new List<Card>();

            for (int i = 0; i < NumDecks; i++)
            {
                decks.Add(new StandardDeck());
                decks[i].Shuffle();
                decks[i].Shuffle();
                decks[i].Shuffle();

                Shoe.AddRange(decks[i].Cards);
            }
        }

        public Card DealCard()
        {
            Card card = null;

            if (Shoe.Count > 0)
            {
                card = Shoe[0];
                Shoe.RemoveAt(0);
            }

            return card;
        }

        public void Dispose()
        {
            Shoe.Clear();
        }
    }
}
