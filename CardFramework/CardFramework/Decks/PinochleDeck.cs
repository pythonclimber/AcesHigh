using System.Collections.Generic;

namespace CardFramework.Decks {
    public class PinochleDeck : Deck {
        public PinochleDeck() {
            CreateCards();
        }

        protected void CreateCards() {
            var faces = GetFaces();
            var suits = GetSuits();
            CreateCards(faces, suits, 2);
        }

        protected override List<string> GetFaces() {
            return new List<string>
                       {
                           CardFaces.Nine.Key,
                           CardFaces.Jack.Key,
                           CardFaces.Queen.Key,
                           CardFaces.King.Key,
                           CardFaces.Ten.Key,
                           CardFaces.AceHigh.Key
                       };
        }
    }
}
