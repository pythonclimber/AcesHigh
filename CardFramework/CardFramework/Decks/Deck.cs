using System;
using System.Collections.Generic;

namespace CardFramework.Decks {
    public abstract class Deck {
        private readonly Random _random;

        public List<Card> Cards { get; protected set; }

        public bool IsEmpty => Cards.Count == 0;

        protected Deck() {
            Cards = new List<Card>();
            _random = new Random();
        }

        public virtual Deck Shuffle() {
            for (var i = 0; i < Cards.Count; i++) {
                var index = _random.Next(Cards.Count);

                var card = Cards[i];
                Cards[i] = Cards[index];
                Cards[index] = card;
            }

            return this;
        }

        public virtual void ReOrder() {
            Cards.Sort((c1, c2) => c1.Index.CompareTo(c2.Index));
        }

        public virtual string Display() {
            var result = string.Join(Environment.NewLine, Cards);
            return result;
        }

        public virtual List<Card> DealCards(int numToDeal = 1) {
            var cards = new List<Card>();

            for (var i = 0; i < numToDeal; i++) {
                if (Cards.Count == 0) {
                    throw new ApplicationException("Cannot deal cards if deck is empty");
                }

                cards.Add(cards[0]);
                cards.RemoveAt(0);
            }

            return cards;
        }

        protected virtual void CreateCards(List<string> faces, List<string> suits, int numCards = 1) {
            var index = 0;
            foreach (var suit in suits) {
                var faceNum = 1;
                foreach (var face in faces) {
                    for (int i = 0; i < numCards; i++) {
                        Cards.Add(new Card(face, faceNum, suit, index++));
                    }
                    faceNum++;
                }
            }
        }

        protected abstract List<string> GetFaces();

        protected virtual List<string> GetSuits() {
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
