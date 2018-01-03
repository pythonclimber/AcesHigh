using System.Collections.Generic;

namespace CardFramework.Decks {


    public class EuchreDeck : Deck {
        public EuchreDeck() {
            CreateCards();
        }

        protected void CreateCards() {
            var suits = GetSuits();
            var faces = GetFaces();
            CreateCards(faces, suits);
        }


        protected override List<string> GetFaces() {
            return new List<string>
                       {
                           CardFaces.Nine,
                           CardFaces.Ten,
                           CardFaces.Jack,
                           CardFaces.Queen,
                           CardFaces.King,
                           CardFaces.Ace
                       };
        }
    }
}
