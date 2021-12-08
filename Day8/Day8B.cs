using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day8
{
    public class Day8B : IDay
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
            // Using this information, you should be able to work out which combination of signal wires corresponds to each of
            // the ten digits. Then, you can decode the four digit output value.
            int output = input.Select(line => line.Split(" | ")
                    .Select(sequence => sequence.Split().Select(encNumber => string.Concat(encNumber.OrderBy(letter => letter)))))
                .Select(line => new {
                    signalPatternsMapping = MapSignalPatternsToNumbers(line.First()),
                    fourDigitValues = line.Last().Reverse()
                })
                .SelectMany(line => line.fourDigitValues.Select(
                    (digit, index) => line.signalPatternsMapping[digit] * (int)Math.Pow(10, index)))
                .Sum();

            // For each entry, determine all of the wire/segment connections and decode the four-digit output values.
            // What do you get if you add up all of the output values?
            Console.WriteLine("Solution: {0}.", output);
        }

        private static Dictionary<string, int> MapSignalPatternsToNumbers(IEnumerable<string> signalPatterns)
        {
            Dictionary<int, string> numberToPattern = new();
            Dictionary<int, List<string>> signalPatternsGroupedByLength = signalPatterns.GroupBy(elem => elem.Length)
                .ToDictionary(group => group.Key, group => group.ToList());

            // 1, 4, 7 and 8 each use a unique number of segments.
            numberToPattern[1] = signalPatternsGroupedByLength[2].First(); // Only 1 has 2 segments enabled.
            numberToPattern[7] = signalPatternsGroupedByLength[3].First(); // Only 7 has 3 segments enabled.
            numberToPattern[4] = signalPatternsGroupedByLength[4].First(); // Only 4 has 4 segments enabled.
            numberToPattern[8] = signalPatternsGroupedByLength[7].First(); // Only 8 has 7 segments enabled.

            // 9, 0 and 6 have 6 segments enabled each.
            numberToPattern[9] = signalPatternsGroupedByLength[6] // 9 contains all enabled segments of 4.
                .Where(elem => EncodedNumberContainsAnother(elem, numberToPattern[4])).First();
            signalPatternsGroupedByLength[6].Remove(numberToPattern[9]);

            numberToPattern[0] = signalPatternsGroupedByLength[6] // 0 contains all enabled segments of 7.
                .Where(elem => EncodedNumberContainsAnother(elem, numberToPattern[7])).First();
            signalPatternsGroupedByLength[6].Remove(numberToPattern[0]);

            numberToPattern[6] = signalPatternsGroupedByLength[6].First(); // Now, 6 is the only one left.

            // 2, 3 and 5 have 5 segments enabled each.
            numberToPattern[3] = signalPatternsGroupedByLength[5] // 3 contains all enabled segments of 1.
                .Where(elem => EncodedNumberContainsAnother(elem, numberToPattern[1])).First();
            signalPatternsGroupedByLength[5].Remove(numberToPattern[3]);

            numberToPattern[5] = signalPatternsGroupedByLength[5] // All enabled segments of 5 are contained by 6.
                .Where(elem => EncodedNumberContainsAnother(numberToPattern[6], elem)).First();
            signalPatternsGroupedByLength[5].Remove(numberToPattern[5]);

            numberToPattern[2] = signalPatternsGroupedByLength[5].First(); // Now, 2 is the only one left.

            return numberToPattern.ToDictionary(elem => elem.Value, elem => elem.Key); // Reverse the dictionary.
        }

        private static bool EncodedNumberContainsAnother(string encodedNumber, string anotherEncodedNumber)
        {
            return anotherEncodedNumber.ToCharArray().All(letter => encodedNumber.Contains(letter));
        }
    }
}
