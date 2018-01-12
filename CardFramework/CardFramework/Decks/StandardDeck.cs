using System.Collections.Generic;

namespace CardFramework.Decks {
    public sealed class CardFaces {
        public static readonly KeyValuePair<string, int> Ace = new KeyValuePair<string, int>("Ace", 1);
        public static readonly KeyValuePair<string, int> Two = new KeyValuePair<string, int>("Two", 2);
        public static readonly KeyValuePair<string, int> Three = new KeyValuePair<string, int>("Three", 3);
        public static readonly KeyValuePair<string, int> Four = new KeyValuePair<string, int>("Four", 4);
        public static readonly KeyValuePair<string, int> Five = new KeyValuePair<string, int>("Five", 5);
        public static readonly KeyValuePair<string, int> Six = new KeyValuePair<string, int>("Six", 6);
        public static readonly KeyValuePair<string, int> Seven = new KeyValuePair<string, int>("Seven", 7);
        public static readonly KeyValuePair<string, int> Eight = new KeyValuePair<string, int>("Eight", 8);
        public static readonly KeyValuePair<string, int> Nine = new KeyValuePair<string, int>("Nine", 9);
        public static readonly KeyValuePair<string, int> Ten = new KeyValuePair<string, int>("Ten", 10);
        public static readonly KeyValuePair<string, int> Jack = new KeyValuePair<string, int>("Jack", 11);
        public static readonly KeyValuePair<string, int> Queen = new KeyValuePair<string, int>("Queen", 12);
        public static readonly KeyValuePair<string, int> King = new KeyValuePair<string, int>("King", 13);
        public static readonly KeyValuePair<string, int> AceHigh = new KeyValuePair<string, int>("AceHigh", 14);
    }

    public class CardSuits {
        public const string Clubs = "Clubs";
        public const string Hearts = "Hearts";
        public const string Spades = "Spades";
        public const string Diamonds = "Diamonds";
    }

    public class StandardDeck : Deck {
        public StandardDeck() {
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
                           CardFaces.Ace.Key,
                           CardFaces.Two.Key,
                           CardFaces.Three.Key,
                           CardFaces.Four.Key,
                           CardFaces.Five.Key,
                           CardFaces.Six.Key,
                           CardFaces.Seven.Key,
                           CardFaces.Eight.Key,
                           CardFaces.Nine.Key,
                           CardFaces.Ten.Key,
                           CardFaces.Jack.Key,
                           CardFaces.Queen.Key,
                           CardFaces.King.Key
                       };
        }
    }
}
