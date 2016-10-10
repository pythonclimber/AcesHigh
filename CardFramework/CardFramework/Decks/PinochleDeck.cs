using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardFramework.Decks
{
    public class PinochleDeck : Deck
    {
        public PinochleDeck()
        {
            CreateCards();
        }

        protected void CreateCards()
        {
            var faces = GetFaces();
            var suits = GetSuits();
            CreateCards(faces, suits, 2);
        }

        protected override List<string> GetFaces()
        {
            return new List<string>
                       {
                           CardFaces.Nine,
                           CardFaces.Jack,
                           CardFaces.Queen,
                           CardFaces.King,
                           CardFaces.Ten,
                           CardFaces.Ace
                       };
        }
    }
}
