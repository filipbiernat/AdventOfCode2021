using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day18
{
    public class Day18B : IDay
    {
        public void Run()
        {
            // The homework assignment involves adding up a list of snailfish numbers (your puzzle input).
            // The snailfish numbers are each listed on a separate line.
            string[] input = File.ReadAllLines(@"..\..\..\Day18\Day18.txt");

            // You notice a second question on the back of the homework assignment:
            // What is the largest magnitude you can get from adding only two of the snailfish numbers?
            // Note that snailfish addition is not commutative - that is, x + y and y + x can produce different results.
            int output = input
                .SelectMany(lhs => input
                    .Where(rhs => rhs != lhs)
                    .Select(rhs => new SnailfishNumber(lhs) + new SnailfishNumber(rhs)))
                .Select(node => node.GetMagnitude())
                .Max();

            // What is the largest magnitude of any sum of two different snailfish numbers from the homework assignment?
            Console.WriteLine("Solution: {0}.", output);
        }
    }
}
