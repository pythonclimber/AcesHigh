using System.Collections.Generic;

namespace CardFramework.Decks {
    public class CardCombination {
        public int Score { get; }
        public List<Card> Cards { get; }

        public CardCombination(List<Card> cards, int score) {
            Cards = cards;
            Score = score;
        }
    }
}
