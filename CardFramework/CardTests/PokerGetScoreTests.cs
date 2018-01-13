using CardFramework;
using CardFramework.Decks;
using NUnit.Framework;

namespace CardTests {
    [TestFixture]
    public class PokerGetScoreTests {
        #region Straight Tests

        [Test]
        public void Pokerplayer_Straight_DescendingOrder() {
            var player = new PokerPlayer();

            player.Hand.Add(new Card(CardFaces.Five.Key, 5, CardSuits.Clubs, -1));
            player.Hand.Add(new Card(CardFaces.Three.Key, 3, CardSuits.Hearts, -1));
            player.Hand.Add(new Card(CardFaces.Six.Key, 6, CardSuits.Diamonds, -1));
            player.Hand.Add(new Card(CardFaces.Four.Key, 4, CardSuits.Spades, -1));
            player.Hand.Add(new Card(CardFaces.Seven.Key, 7, CardSuits.Clubs, -1));

            player.ScoreHand();

            Assert.AreEqual(player.HandValue, PokerHandValues.Straight);
        }

        [Test]
        public void Pokerplayer_Straight_AscendingOrder() {
            var player = new PokerPlayer();

            player.Hand.Add(new Card(CardFaces.Five.Key, 5, CardSuits.Clubs, -1));
            player.Hand.Add(new Card(CardFaces.Three.Key, 3, CardSuits.Hearts, -1));
            player.Hand.Add(new Card(CardFaces.Six.Key, 6, CardSuits.Diamonds, -1));
            player.Hand.Add(new Card(CardFaces.Four.Key, 4, CardSuits.Spades, -1));
            player.Hand.Add(new Card(CardFaces.Seven.Key, 7, CardSuits.Clubs, -1));

            player.ScoreHand();

            Assert.AreEqual(player.HandValue, PokerHandValues.Straight);
        }

        [Test]
        public void Pokerplayer_Straight_RandomOrder() {
            var player = new PokerPlayer();

            player.Hand.Add(new Card(CardFaces.Five.Key, 5, CardSuits.Clubs, -1));
            player.Hand.Add(new Card(CardFaces.Three.Key, 3, CardSuits.Hearts, -1));
            player.Hand.Add(new Card(CardFaces.Six.Key, 6, CardSuits.Diamonds, -1));
            player.Hand.Add(new Card(CardFaces.Four.Key, 4, CardSuits.Spades, -1));
            player.Hand.Add(new Card(CardFaces.Seven.Key, 7, CardSuits.Clubs, -1));

            player.ScoreHand();

            Assert.AreEqual(player.HandValue, PokerHandValues.Straight);
        }

        [Test]
        public void Pokerplayer_Straight_NearMiss() {
            var player = new PokerPlayer();

            player.Hand.Add(new Card(CardFaces.Five.Key, 5, CardSuits.Clubs, -1));
            player.Hand.Add(new Card(CardFaces.Two.Key, 2, CardSuits.Hearts, -1));
            player.Hand.Add(new Card(CardFaces.Six.Key, 6, CardSuits.Diamonds, -1));
            player.Hand.Add(new Card(CardFaces.Four.Key, 4, CardSuits.Spades, -1));
            player.Hand.Add(new Card(CardFaces.Seven.Key, 7, CardSuits.Clubs, -1));

            player.ScoreHand();

            Assert.AreNotEqual(player.HandValue, PokerHandValues.Straight);
            Assert.AreEqual(player.HandValue, PokerHandValues.HighCard);
            Assert.AreEqual(player.HighCard, 7);
        }

        #endregion

        #region Flush Tests

        [TestCase(CardSuits.Clubs)]
        [TestCase(CardSuits.Hearts)]
        [TestCase(CardSuits.Spades)]
        [TestCase(CardSuits.Diamonds)]
        public void Pokerplayer_Flush(string suit) {
            var player = new PokerPlayer();

            player.Hand.Add(new Card(CardFaces.Five.Key, 5, suit));
            player.Hand.Add(new Card(CardFaces.Two.Key, 2, suit));
            player.Hand.Add(new Card(CardFaces.Eight.Key, 8, suit));
            player.Hand.Add(new Card(CardFaces.Jack.Key, 11, suit));
            player.Hand.Add(new Card(CardFaces.Queen.Key, 12, suit));

            player.ScoreHand();

            Assert.AreEqual(PokerHandValues.Flush, player.HandValue);
            Assert.AreEqual(12, player.HighCard);
        }

        [TestCase(CardSuits.Clubs)]
        [TestCase(CardSuits.Hearts)]
        [TestCase(CardSuits.Spades)]
        [TestCase(CardSuits.Diamonds)]
        public void Pokerplayer_StraightFlush(string suit) {
            var player = new PokerPlayer();

            player.Hand.Add(new Card(CardFaces.Ten.Key, 10, suit));
            player.Hand.Add(new Card(CardFaces.Nine.Key, 9, suit));
            player.Hand.Add(new Card(CardFaces.Eight.Key, 8, suit));
            player.Hand.Add(new Card(CardFaces.Jack.Key, 11, suit));
            player.Hand.Add(new Card(CardFaces.Queen.Key, 12, suit));

            player.ScoreHand();

            Assert.AreEqual(player.HandValue, PokerHandValues.StraightFlush);
            Assert.AreEqual(player.HighCard, 12);
        }

        [TestCase(CardSuits.Clubs)]
        [TestCase(CardSuits.Hearts)]
        [TestCase(CardSuits.Spades)]
        [TestCase(CardSuits.Diamonds)]
        public void Pokerplayer_RoyalFlush(string suit) {
            var player = new PokerPlayer();

            player.Hand.Add(new Card(CardFaces.Ten.Key, 10, suit));
            player.Hand.Add(new Card(CardFaces.Ace.Key, 1, suit));
            player.Hand.Add(new Card(CardFaces.King.Key, 13, suit));
            player.Hand.Add(new Card(CardFaces.Jack.Key, 11, suit));
            player.Hand.Add(new Card(CardFaces.Queen.Key, 12, suit));

            player.ScoreHand();

            Assert.AreEqual(player.HandValue, PokerHandValues.StraightFlush);
            Assert.AreEqual(player.HighCard, 14);
        }

        #endregion

        #region Set Tests

        [TestCase("Six", 6)]
        [TestCase("Ace", 1)]
        public void Pokerplayer_Pair_NotHighestCards(string highCardFace, int cardValue) {
            var player = new PokerPlayer();
            var highCardValue = cardValue == 1 ? 14 : cardValue;

            player.Hand.Add(new Card(CardFaces.Ten.Key, 10, CardSuits.Clubs));
            player.Hand.Add(new Card(CardFaces.Ten.Key, 10, CardSuits.Hearts));
            player.Hand.Add(new Card(CardFaces.Five.Key, 5, CardSuits.Clubs));
            player.Hand.Add(new Card(CardFaces.Three.Key, 3, CardSuits.Clubs));
            player.Hand.Add(new Card(highCardFace, cardValue, CardSuits.Clubs));

            player.ScoreHand();

            Assert.AreEqual(PokerHandValues.Pair, player.HandValue);
            Assert.AreEqual(highCardValue, player.HighCard);
            Assert.AreEqual(10, player.HighSetValue);
        }

        [TestCase("Queen", 12)]
        [TestCase("Ace", 1)]
        public void Pokerplayer_Pair_HighestCards(string highCardFace, int cardValue) {
            var player = new PokerPlayer();
            var highCardValue = cardValue == 1 ? 14 : cardValue;

            player.Hand.Add(new Card(CardFaces.Ten.Key, 10, CardSuits.Clubs));
            player.Hand.Add(new Card(CardFaces.King.Key, 13, CardSuits.Hearts));
            player.Hand.Add(new Card(highCardFace, cardValue, CardSuits.Clubs));
            player.Hand.Add(new Card(CardFaces.King.Key, 13, CardSuits.Clubs));
            player.Hand.Add(new Card(CardFaces.Three.Key, 3, CardSuits.Clubs));

            player.ScoreHand();

            Assert.AreEqual(PokerHandValues.Pair, player.HandValue);
            Assert.AreEqual(highCardValue, player.HighCard);
            Assert.AreEqual(13, player.HighSetValue);
        }

        [Test]
        public void Pokerplayer_TwoPair_LowestCards() {
            var player = new PokerPlayer();

            player.Hand.Add(new Card(CardFaces.Two.Key, 2, CardSuits.Clubs));
            player.Hand.Add(new Card(CardFaces.Two.Key, 2, CardSuits.Hearts));
            player.Hand.Add(new Card(CardFaces.Five.Key, 5, CardSuits.Clubs));
            player.Hand.Add(new Card(CardFaces.Five.Key, 5, CardSuits.Diamonds));
            player.Hand.Add(new Card(CardFaces.King.Key, 13, CardSuits.Spades));

            player.ScoreHand();

            Assert.AreEqual(PokerHandValues.TwoPair, player.HandValue);
            Assert.AreEqual(13, player.HighCard);
            Assert.AreEqual(5, player.HighSetValue);
            Assert.AreEqual(2, player.LowSetValue);
        }

        [TestCase("Two", 2)]
        [TestCase("Ace", 14)]
        public void Pokerplayer_TwoPair_HighestCards(string highCardFace, int highCardValue) {
            var player = new PokerPlayer();

            player.Hand.Add(new Card(highCardFace, highCardValue, CardSuits.Clubs));
            player.Hand.Add(new Card(CardFaces.King.Key, 13, CardSuits.Hearts));
            player.Hand.Add(new Card(CardFaces.Five.Key, 5, CardSuits.Clubs));
            player.Hand.Add(new Card(CardFaces.King.Key, 13, CardSuits.Spades));
            player.Hand.Add(new Card(CardFaces.Five.Key, 5, CardSuits.Diamonds));

            player.ScoreHand();

            Assert.AreEqual(PokerHandValues.TwoPair, player.HandValue);
            Assert.AreEqual(highCardValue, player.HighCard);
            Assert.AreEqual(13, player.HighSetValue);
            Assert.AreEqual(5, player.LowSetValue);
        }

        [Test]
        public void Pokerplayer_TwoPair_SplitCards() {
            var player = new PokerPlayer();

            player.Hand.Add(new Card(CardFaces.Two.Key, 2, CardSuits.Clubs));
            player.Hand.Add(new Card(CardFaces.Two.Key, 2, CardSuits.Hearts));
            player.Hand.Add(new Card(CardFaces.Five.Key, 5, CardSuits.Clubs));
            player.Hand.Add(new Card(CardFaces.King.Key, 13, CardSuits.Spades));
            player.Hand.Add(new Card(CardFaces.King.Key, 13, CardSuits.Diamonds));

            player.ScoreHand();

            Assert.AreEqual(PokerHandValues.TwoPair, player.HandValue);
            Assert.AreEqual(5, player.HighCard);
            Assert.AreEqual(13, player.HighSetValue);
            Assert.AreEqual(2, player.LowSetValue);
        }

        [TestCase("King", 13)]
        [TestCase("Ace", 1)]
        [TestCase("Four", 4)]
        public void Pokerplayer_ThreeKind_NotHighestCards(string highCardFace, int cardValue) {
            var player = new PokerPlayer();
            var highCardValue = cardValue == 1 ? 14 : cardValue;

            player.Hand.Add(new Card(CardFaces.Five.Key, 5, CardSuits.Clubs));
            player.Hand.Add(new Card(CardFaces.Three.Key, 3, CardSuits.Hearts));
            player.Hand.Add(new Card(CardFaces.Five.Key, 5, CardSuits.Diamonds));
            player.Hand.Add(new Card(highCardFace, cardValue, CardSuits.Spades));
            player.Hand.Add(new Card(CardFaces.Five.Key, 5, CardSuits.Diamonds));

            player.ScoreHand();

            Assert.AreEqual(PokerHandValues.ThreeOfAKind, player.HandValue);
            Assert.AreEqual(5, player.HighSetValue);
            Assert.AreEqual(highCardValue, player.HighCard);
        }

        [TestCase("Three", 3)]
        [TestCase("Ace", 1)]
        public void Pokerplayer_ThreeKind_HighestCards(string highCardFace, int cardValue) {
            var player = new PokerPlayer();
            var highCardValue = cardValue == 1 ? 14 : cardValue;

            player.Hand.Add(new Card(CardFaces.King.Key, 13, CardSuits.Clubs));
            player.Hand.Add(new Card(CardFaces.Two.Key, 2, CardSuits.Hearts));
            player.Hand.Add(new Card(CardFaces.King.Key, 13, CardSuits.Hearts));
            player.Hand.Add(new Card(CardFaces.King.Key, 13, CardSuits.Spades));
            player.Hand.Add(new Card(highCardFace, cardValue, CardSuits.Diamonds));

            player.ScoreHand();

            Assert.AreEqual(PokerHandValues.ThreeOfAKind, player.HandValue);
            Assert.AreEqual(13, player.HighSetValue);
            Assert.AreEqual(highCardValue, player.HighCard);
        }

        [Test]
        public void Pokerplayer_FullHouse_ThreeHigh() {
            var player = new PokerPlayer();

            player.Hand.Add(new Card(CardFaces.Five.Key, 5, CardSuits.Clubs));
            player.Hand.Add(new Card(CardFaces.Two.Key, 2, CardSuits.Hearts));
            player.Hand.Add(new Card(CardFaces.Five.Key, 5, CardSuits.Hearts));
            player.Hand.Add(new Card(CardFaces.Two.Key, 2, CardSuits.Spades));
            player.Hand.Add(new Card(CardFaces.Five.Key, 5, CardSuits.Diamonds));

            player.ScoreHand();

            Assert.AreEqual(PokerHandValues.FullHouse, player.HandValue);
            Assert.AreEqual(5, player.HighSetValue);
            Assert.AreEqual(2, player.LowSetValue);
            Assert.AreEqual(FullHouseTypes.ThreeHigh, player.FullHouseType);
        }

        [Test]
        public void Pokerplayer_FullHouse_ThreeLow() {
            var player = new PokerPlayer();

            player.Hand.Add(new Card(CardFaces.Five.Key, 5, CardSuits.Clubs));
            player.Hand.Add(new Card(CardFaces.Two.Key, 2, CardSuits.Hearts));
            player.Hand.Add(new Card(CardFaces.Five.Key, 5, CardSuits.Hearts));
            player.Hand.Add(new Card(CardFaces.Two.Key, 2, CardSuits.Spades));
            player.Hand.Add(new Card(CardFaces.Two.Key, 2, CardSuits.Diamonds));

            player.ScoreHand();

            Assert.AreEqual(PokerHandValues.FullHouse, player.HandValue);
            Assert.AreEqual(5, player.HighSetValue);
            Assert.AreEqual(2, player.LowSetValue);
            Assert.AreEqual(FullHouseTypes.ThreeLow, player.FullHouseType);
        }

        [Test]
        public void Pokerplayer_FourKind_Low() {
            var player = new PokerPlayer();

            player.Hand.Add(new Card(CardFaces.Two.Key, 2, CardSuits.Clubs));
            player.Hand.Add(new Card(CardFaces.Two.Key, 2, CardSuits.Hearts));
            player.Hand.Add(new Card(CardFaces.Three.Key, 3, CardSuits.Hearts));
            player.Hand.Add(new Card(CardFaces.Two.Key, 2, CardSuits.Spades));
            player.Hand.Add(new Card(CardFaces.Two.Key, 2, CardSuits.Diamonds));

            player.ScoreHand();

            Assert.AreEqual(PokerHandValues.FourOfAKind, player.HandValue);
            Assert.AreEqual(2, player.HighSetValue);
            Assert.AreEqual(3, player.HighCard);
        }

        [TestCase("Queen", 12)]
        [TestCase("Ace", 14)]
        public void Pokerplayer_FourKind_High(string highCardFace, int highCardValue) {
            var player = new PokerPlayer();

            player.Hand.Add(new Card(CardFaces.King.Key, 13, CardSuits.Clubs));
            player.Hand.Add(new Card(CardFaces.King.Key, 13, CardSuits.Hearts));
            player.Hand.Add(new Card(highCardFace, highCardValue, CardSuits.Hearts));
            player.Hand.Add(new Card(CardFaces.King.Key, 13, CardSuits.Spades));
            player.Hand.Add(new Card(CardFaces.King.Key, 13, CardSuits.Diamonds));

            player.ScoreHand();

            Assert.AreEqual(PokerHandValues.FourOfAKind, player.HandValue);
            Assert.AreEqual(13, player.HighSetValue);
            Assert.AreEqual(highCardValue, player.HighCard);
        }

        [Test]
        public void Pokerplayer_FourKind_Aces() {
            var player = new PokerPlayer();

            player.Hand.Add(new Card(CardFaces.Ace.Key, 1, CardSuits.Clubs));
            player.Hand.Add(new Card(CardFaces.Ace.Key, 1, CardSuits.Hearts));
            player.Hand.Add(new Card(CardFaces.King.Key, 13, CardSuits.Hearts));
            player.Hand.Add(new Card(CardFaces.Ace.Key, 1, CardSuits.Spades));
            player.Hand.Add(new Card(CardFaces.Ace.Key, 1, CardSuits.Diamonds));

            player.ScoreHand();

            Assert.AreEqual(PokerHandValues.FourOfAKind, player.HandValue);
            Assert.AreEqual(14, player.HighSetValue);
            Assert.AreEqual(13, player.HighCard);
        }

        [TestCase("Ace", 1)]
        [TestCase("King", 13)]
        [TestCase("Jack", 11)]
        public void Pokerplayer_HighCard_Ace(string highCardFace, int cardValue) {
            var player = new PokerPlayer();
            var highCardValue = cardValue == 1 ? 14 : cardValue;

            player.Hand.Add(new Card(CardFaces.Seven.Key, 7, CardSuits.Clubs));
            player.Hand.Add(new Card(CardFaces.Ten.Key, 10, CardSuits.Hearts));
            player.Hand.Add(new Card(CardFaces.Five.Key, 5, CardSuits.Clubs));
            player.Hand.Add(new Card(CardFaces.Three.Key, 3, CardSuits.Clubs));
            player.Hand.Add(new Card(highCardFace, cardValue, CardSuits.Clubs));

            player.ScoreHand();

            Assert.AreEqual(PokerHandValues.HighCard, player.HandValue);
            Assert.AreEqual(highCardValue, player.HighCard);
        }

        #endregion
    }
}
