using CardFramework;
using CardFramework.Decks;
using NUnit.Framework;
using System.Collections.Generic;

namespace CardTests {
    [TestFixture]
    public class CribHandTests {
        [Test]
        public void GetScore_TwentyNine() {
            var turnCard = new Card("Five", 5, "Clubs");
            var cards = new List<Card> {
                new Card("Five", 5, "Diamonds"),
                new Card("Five", 5, "Spades"),
                new Card("Jack", 11, "Clubs"),
                new Card("Five", 5, "Hearts")
            };
            var cribHand = new CribHand(cards);

            var score = cribHand.GetScore(true, turnCard);

            Assert.AreEqual(29, score);
        }

        [Test]
        public void GetScore_StraightEight() {
            var turnCard = new Card("Two", 2, "Clubs");
            var cards = new List<Card> {
                new Card("Jack", 11, "Hearts"),
                new Card("Queen", 12, "Diamonds"),
                new Card("Queen", 12, "Spades"),
                new Card("King", 13, "Spades")
            };
            var cribHand = new CribHand(cards);

            var score = cribHand.GetScore(true, turnCard);

            Assert.AreEqual(8, score);
        }

        [Test]
        public void GetScore_ThreeOfAKind() {
            var turnCard = new Card("Jack", 11, "Clubs");
            var cards = new List<Card> {
                new Card("Jack", 11, "Hearts"),
                new Card("Jack", 11, "Diamonds"),
                new Card("Ace", 1, "Spades"),
                new Card("Two", 2, "Spades")
            };
            var cribHand = new CribHand(cards);

            var score = cribHand.GetScore(true, turnCard);

            Assert.AreEqual(6, score);
        }
    }
}
