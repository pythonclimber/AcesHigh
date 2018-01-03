using System.Collections.Generic;
using System.Drawing;


namespace CardFramework.Decks {
    public class Card {
        public string Suit { get; protected set; }

        public string Face { get; protected set; }

        public int FaceNum { get; protected set; }

        public Image Image { get; protected set; }

        public int Index { get; protected set; }


        public Card(string face, int faceNum, string suit)
            : this(face, faceNum, suit, -1) {

        }

        public Card(string face, int faceNum, string suit, int index) {
            const string sExt = ".png";

            Face = face;
            FaceNum = faceNum;
            Suit = suit;
            Index = index;

            var imageSuit = Suit.Substring(0, 1).ToLower();
            var imageFace = FaceNum > 10 ? Face.Substring(0, 1).ToLower() : FaceNum.ToString();
            var fileName = imageSuit + imageFace + sExt;

            Image = Image.FromFile(fileName);
        }

        public string ToString(bool isShort = false) {
            return !isShort ? $"The {Face,-5} of {Suit,-8}" : $"{Face,-5} {Suit,-8}";
        }

        public int FaceValue(bool aceHigh = false) {
            if (Face == CardFaces.Ace && aceHigh) {
                return 11;
            }

            return FaceNum >= 10 ? 10 : FaceNum;
        }
    }

    public class CardComparer : IEqualityComparer<Card> {
        public bool Equals(Card x, Card y) {
            //if x and y are literally the same object, return true
            if (ReferenceEquals(x, y))
                return true;

            //if either or both are null, return false
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                return false;

            //otherwise, return comparison.
            return x.Face == y.Face && x.Suit == y.Suit;
        }

        public int GetHashCode(Card card) {
            if (ReferenceEquals(card, null))
                return 0;

            var faceCode = card.Face == null ? 0 : card.Face.GetHashCode();

            var suitCode = card.Suit == null ? 0 : card.Suit.GetHashCode();

            return faceCode ^ suitCode;
        }
    }
}
