using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CardFramework.Decks;

namespace CardFramework.Helpers
{
    public static class Extensions
    {
        public static T Middle<T>(this List<T> value)
        {
            return value.Count%2 == 0 ? value[value.Count/2] : value[(value.Count - 1)/2];
        }

        public static bool IsAce(this Card value)
        {
            return value.Face == CardFaces.Ace;
        }

        public static bool IsEmpty<T>(this ICollection<T> value)
        {
            return value == null || value.Count == 0;
        }

        public static bool IsNull(this object value)
        {
            return value == null || value is DBNull || value == DBNull.Value;
        }

        public static string GetOppositeSuit(this string suitName)
        {
            switch (suitName)
            {
                case CardSuits.Clubs:
                    return CardSuits.Spades;
                case "Hearts":
                    return CardSuits.Diamonds;
                case "Spades":
                    return CardSuits.Clubs;
                case "Diamonds":
                    return CardSuits.Hearts;
                default:
                    return null;
            }
        }

        public static T RandomItem<T>(this ICollection<T> value)
        {
            var random = new Random();
            var index = random.Next(value.Count);

            return value.ElementAt(index);
        }
    }
}
