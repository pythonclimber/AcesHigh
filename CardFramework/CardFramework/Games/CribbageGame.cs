using CardFramework.Decks;

namespace CardFramework {
    public class CribbageGame : Game {
        protected CribPlayer player;
        protected CribPlayer compPlayer;
        protected StandardDeck deck;

        public CribbageGame() {
            player = new CribPlayer();
            compPlayer = new CribPlayer(true, false);
            deck = new StandardDeck();
        }

        public void DealCards() {
            //for (int i = 0; i < player.NumCards.Count; i++) {
            //    player.ReceiveCard(deck.DealCards(1));
            //    compPlayer.ReceiveCard(deck.DealCards(1));
            //}
        }

        public void ScoreHand() {

        }
    }
}
