using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day23
{
    public class Day23B : IDay
    {

        public void Run()
        {
            List<string> input = File.ReadAllLines(@"..\..\..\Day23\Day23.txt").ToList();

            // As you prepare to give the amphipods your solution, you notice that the diagram they handed you was actually
            // folded up. As you unfold it, you discover an extra part of the diagram.
            // Between the first and second lines of text that contain amphipod starting positions, insert the following lines:
            input.Insert(3, "  #D#C#B#A#");
            input.Insert(4, "  #D#B#A#C#");

            // Using the initial configuration from the full diagram, what is the least energy required to organize the amphipods?
            int output = Solver.Solve(new Burrow(input));
            Console.WriteLine("Solution: {0}.", output);
        }
    }
}
