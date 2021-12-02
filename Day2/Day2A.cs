using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day2
{
    public class Day2A : IDay
    {
        public void Run()
        {
            // The submarine seems to already have a planned course (your puzzle input).
            string[] input = File.ReadAllLines(@"..\..\..\Day2\Day2.txt");

            IEnumerable<(string direction, int xUnits)> commands = input
                .Select(line => line.Split(separator: " ", count: 2))
                .Select(command => (direction: command[0], xUnits: int.Parse(command[1])));

            // Your horizontal position and depth both start at 0. 
            int horizontalPosition = 0;
            int depth = 0;

            foreach ((string direction, int xUnits) in commands)
            {
                if (direction == "down")
                {   // down X increases the depth by X units.
                    depth += xUnits;
                }
                else if (direction == "up")
                {   // up X decreases the depth by X units.
                    depth -= xUnits;
                }
                else
                {   // forward X increases the horizontal position by X units.
                    horizontalPosition += xUnits;
                }
            }

            // What do you get if you multiply your final horizontal position by your final depth?
            Console.WriteLine("Solution: {0}.", horizontalPosition * depth);
        }
    }
}
