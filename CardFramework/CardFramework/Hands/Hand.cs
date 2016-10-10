using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CardFramework.Decks;

namespace CardFramework
{
    /// <summary>
    /// Base class for player hands for all games.
    /// </summary>
    public abstract class Hand
    {
        public List<Card> Cards { get; protected set; }
        public int NumCards { get; protected set; }

        #region Public Property

        #endregion

        #region Construction

        /// <summary>
        /// Empty default constructor.  Calls base class default constructor.
        /// </summary>
        protected Hand() : this(0)
        {
        }

        protected Hand(int maxCards) : this(new List<Card>(), maxCards)
        {
        }

        protected Hand(List<Card> cards, int maxCards)
        {
            Cards = cards;
            NumCards = maxCards;
        }

        #endregion

        public abstract void ScoreHand();

        public abstract string DisplayHand();

        public override string ToString()
        {
            var result = string.Empty;
            foreach (var card in Cards)
            {
                result += card.ToString() + Environment.NewLine;
            }
            return result;
        }
    }
}
