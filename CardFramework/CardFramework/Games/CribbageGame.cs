using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CardFramework.Decks;

namespace CardFramework
{
    public class CribbageGame : Game
    {
        protected CribPlayer player;
        protected CribPlayer compPlayer;
        protected CribHand crib;
        protected StandardDeck deck;

        public CribbageGame()
        {
            player = new CribPlayer();
            compPlayer = new CribPlayer(true);
            crib = new CribHand(true);
            deck = new StandardDeck();
        }

        public void AddCardsToCrib(List<Card> cardsToAdd)
        {
            crib.Cards.AddRange(cardsToAdd);
            compPlayer.CompPassToCrib(crib);
        }

        public void DealCards()
        {
            for (int i = 0; i < player.Hand.NumCards; i++)
            {
                player.ReceiveCard(deck.DealCards(1));
                compPlayer.ReceiveCard(deck.DealCards(1));
            }
        }

        public void ScoreHand()
        {
            
        }
    }
}
