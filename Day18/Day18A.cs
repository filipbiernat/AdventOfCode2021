using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day18
{
    public class Day18A : IDay
    {
        public void Run()
        {
            // The homework assignment involves adding up a list of snailfish numbers (your puzzle input).
            // The snailfish numbers are each listed on a separate line.
            string[] input = File.ReadAllLines(@"..\..\..\Day18\Day18.txt");

            // Add the first snailfish number and the second, then add that result and the third, then add that result and the
            // fourth, and so on until all numbers in the list have been used once.
            // To check whether it's the right answer, the snailfish teacher only checks the magnitude of the final sum. 
            int output = input
                .Select(line => new SnailfishNumber(line))
                .Aggregate((lhs, rhs) => lhs + rhs)
                .GetMagnitude();

            // What is the magnitude of the final sum?
            Console.WriteLine("Solution: {0}.", output);
        }
    }
}
