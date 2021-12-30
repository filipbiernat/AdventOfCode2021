using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day23
{
    public class Day23A : IDay
    {

        public void Run()
        {
            List<string> input = File.ReadAllLines(@"..\..\..\Day23\Day23.txt").ToList();

            // What is the least energy required to organize the amphipods?
            int output = Solver.Solve(new Burrow(input));
            Console.WriteLine("Solution: {0}.", output);
        }
    }
}
