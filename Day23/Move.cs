using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day23
{
    public class Move
    {
        public int Cost;
        public Burrow BurrowAfterMove;

        public Move(int cost, Burrow burrowAfterMove)
        {
            Cost = cost;
            BurrowAfterMove = burrowAfterMove;
        }
    }
}
