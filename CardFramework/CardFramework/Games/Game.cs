using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardFramework
{
    public abstract class Game
    {
        public int NumPlayers { get; protected set; }

        public Game() : this(2)
        {
            
        }

        public Game(int numPlayers)
        {
            this.NumPlayers = numPlayers;
        }
    }
}
