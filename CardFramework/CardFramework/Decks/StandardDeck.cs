using System;
using System.Collections.Generic;
using System.Linq;

namespace CardFramework.Decks
{
    public sealed class CardFaces
    {
        public const string Ace = "Ace";
        public const string Two = "Two";
        public const string Three = "Three";
        public const string Four = "Four";
        public const string Five = "Five";
        public const string Six = "Six";
        public const string Seven = "Seven";
        public const string Eight = "Eight";
        public const string Nine = "Nine";
        public const string Ten = "Ten";
        public const string Jack = "Jack";
        public const string Queen = "Queen";
        public const string King = "King";
    }

    public class CardSuits
    {
        public const string Clubs = "Clubs";
        public const string Hearts = "Hearts";
        public const string Spades = "Spades";
        public const string Diamonds = "Diamonds";
    }

    public class StandardDeck : Deck
    {
        public StandardDeck()
        {
            CreateCards();
        }

        protected void CreateCards()
        {
            var suits = GetSuits();
            var faces = GetFaces();
            CreateCards(faces, suits);
        }

        protected override List<string> GetFaces()
        {
            return new List<string>
                       {
                           CardFaces.Ace,
                           CardFaces.Two,
                           CardFaces.Three,
                           CardFaces.Four,
                           CardFaces.Five,
                           CardFaces.Six,
                           CardFaces.Seven,
                           CardFaces.Eight,
                           CardFaces.Nine,
                           CardFaces.Ten,
                           CardFaces.Jack,
                           CardFaces.Queen,
                           CardFaces.King
                       };
        }
    }
}
