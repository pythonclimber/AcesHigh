using System.Collections.Generic;

namespace CardFramework.Decks {
    public class CardCombination {
        private List<Card> _cards;
        private int _score;

        public CardCombination(List<Card> cards, int score) {
            _cards = cards;
            _score = score;
        }
    }
}
