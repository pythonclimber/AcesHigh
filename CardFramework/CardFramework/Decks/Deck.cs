using System;
using System.Collections.Generic;

namespace CardFramework.Decks
{
    public abstract class Deck
    {
        #region Public Members

        public List<Card> Cards { get; protected set; }

        #endregion

        #region Properties

        /// <summary>
        /// Returns false if the deck still contains cards and true if the deck is empty
        /// </summary>
        public bool IsEmpty
        {
            get { return Cards.Count == 0; }
        }

        #endregion

        #region Construction Zone

        /// <summary>
        /// Default Constructor
        /// </summary>
        protected Deck()
        {
            Cards = new List<Card>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Shuffles the deck
        /// </summary>
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

        /// <summary>
        /// Puts the deck back in order.
        /// Should rarely be needed as this rarely happens in games
        /// </summary>
        public virtual void ReOrder()
        {
            Cards.Sort((c1, c2) => c1.Index.CompareTo(c2.Index));
        }

        /// <summary>
        /// Displays the entire deck in a console app
        /// Used for testing, but not much afterwards
        /// </summary>
        public virtual string Display()
        {
            var result = string.Empty;
            Cards.ForEach(c => result += c.ToString() + "\n");
            return result;
        }

        /// <summary>
        /// Used in dealing to get a single card from the deck then remove that card.
        /// </summary>
        /// <returns>Card</returns>
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

        #endregion

        #region Protected Methods

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

        #endregion
    }
}
