using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day8
{
    public class Day8A : IDay
    {
        public void Run()
        {
            // For each display, you watch the changing signals for a while, make a note of all ten unique signal patterns you
            // see, and then write down a single four digit output value (your puzzle input).
            string[] input = File.ReadAllLines(@"..\..\..\Day8\Day8.txt");

            // Each entry consists of ten unique signal patterns, a | delimiter, and finally the four digit output value. Within
            // an entry, the same wire/segment connections are used (but you don't know what the connections actually are). The
            // unique signal patterns correspond to the ten different ways the submarine tries to render a digit using the current
            // wire/segment connections.
            List<string[]> outputValues = input.Select(elem => elem.Split(" | ")[1].Split()).ToList();

            // For now, focus on the easy digits. Because the digits 1, 4, 7, and 8 each use a unique number of segments, you
            // should be able to tell which combinations of signals correspond to those digits.
            List<int> knownNumbersOfSegments = new() { 2, 4, 3, 7 };
            int output = outputValues.SelectMany(elem => elem)
                .Where(elem => knownNumbersOfSegments.Contains(elem.Length))
                .Count();

            // In the output values, how many times do digits 1, 4, 7, or 8 appear?
            Console.WriteLine("Solution: {0}.", output);
        }
    }
}
