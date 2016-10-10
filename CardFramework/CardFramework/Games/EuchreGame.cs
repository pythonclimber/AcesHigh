using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CardFramework.Decks;

namespace CardFramework
{
    public enum EuchreDealNumbers
    {
        Two = 2,
        Three = 3
    }
    public class EuchreGame
    {
        private List<EuchrePlayer> PlayerOrder;
        public EuchrePlayer LivePlayer { get; protected set; }
        public EuchrePlayer CompPlayer1 { get; protected set; }
        public EuchrePlayer CompPlayer2 { get; protected set; }
        public EuchrePlayer CompPlayer3 { get; protected set; }

        public EuchreTeam LiveTeam { get; protected set; }
        public EuchreTeam CompTeam { get; protected set; }

        public EuchreGame()
        {
            LivePlayer = new EuchrePlayer(false, CompPlayer2);
            CompPlayer1 = new EuchrePlayer(true, CompPlayer3);
            CompPlayer2 = new EuchrePlayer(true, LivePlayer);
            CompPlayer3 = new EuchrePlayer(true, CompPlayer1);
            PlayerOrder = new List<EuchrePlayer>();

            LivePlayer.IsDealer = true;
        }

        public void StartNewHand()
        {
            ArrangePlayers();
            DealHand();
        }

        public void DealHand()
        {
            var deck = new EuchreDeck();
            for (int i = 0; i < 4; i++)
            {
                deck.Shuffle();
            }

            //Always start by dealing three to the first player
            var numToDeal = (int)EuchreDealNumbers.Three;
            for (var i = 0; i < 8; i++)
            {
                PlayerOrder[i%4].Hand.AddRange(deck.DealCards(numToDeal));
                numToDeal = ToggleNumToDeal(numToDeal);
            }
        }

        private void ArrangePlayers()
        {
            if (PlayerOrder.Count == 0)
            {
                PlayerOrder.Add(CompPlayer2);
                PlayerOrder.Add(CompPlayer3);
                PlayerOrder.Add(CompPlayer1);
                PlayerOrder.Add(LivePlayer);
            }
            else
            {
                var player = PlayerOrder[3];
                PlayerOrder.RemoveAt(3);
                PlayerOrder.Insert(0, player);
            }
        }

        private int ToggleNumToDeal(int numToDeal)
        {
            return (int)(numToDeal == (int)EuchreDealNumbers.Three ? EuchreDealNumbers.Two : EuchreDealNumbers.Three);
        }
    }
}
