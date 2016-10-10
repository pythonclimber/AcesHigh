using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardFramework
{
    public class EuchreTeam
    {
        public EuchrePlayer Player1 { get; protected set; }
        public EuchrePlayer Player2 { get; protected set; }
        public int Score { get; protected set; }

        public EuchreTeam (EuchrePlayer p1, EuchrePlayer p2)
        {
            Score = 0;
            Player1 = p1;
            Player2 = p2;
        }

        public void AddPoints(int pts)
        {
            Score += pts;
        }
    }
}
