using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day3
{
    public class Day3B : IDay
    {
        public void Run()
        {
            // The diagnostic report (your puzzle input) consists of a list of binary numbers which, when decoded properly, can
            // tell you many useful things about the conditions of the submarine.
            string[] input = File.ReadAllLines(@"..\..\..\Day3\Day3.txt");
            List<List<int>> diagnosticReport =
                input.Select(line => line.Select(character => (int)char.GetNumericValue(character)).ToList()).ToList();

            // Use the binary numbers in your diagnostic report to calculate the oxygen generator rating and CO2 scrubber rating,
            // then multiply them together.
            int output = FindOxygenGeneratorRating(diagnosticReport) * FindCo2ScrubberRating(diagnosticReport);

            // What is the power consumption of the submarine?
            Console.WriteLine("Solution: {0}.", output);
        }

        private int FindOxygenGeneratorRating(List<List<int>> diagnosticReport, int position = 0)
        {
            int inversedPosition = diagnosticReport.First().Count - position - 1;
            if (inversedPosition < 0) return 0;

            // To find oxygen generator rating, determine the most common value (0 or 1) in the current bit position
            int mostCommonBit = diagnosticReport
                .Select(line => line[position])
                .GroupBy(elem => elem)
                .OrderBy(group => group.Count())
                .ThenBy(g => g.Key) // If 0 and 1 are equally common, keep values with a 1 in the position being considered.
                .Last().Key;
            // and keep only numbers with that bit in that position.
            List<List<int>> numbersToKeep = diagnosticReport.Where(line => line[position] == mostCommonBit).ToList();
            return (mostCommonBit << inversedPosition) + FindOxygenGeneratorRating(numbersToKeep, position + 1);
        }

        private int FindCo2ScrubberRating(List<List<int>> diagnosticReport, int position = 0)
        {
            int inversedPosition = diagnosticReport.First().Count - position - 1;
            if (inversedPosition < 0) return 0;

            // To find CO2 scrubber rating, determine the least common value (0 or 1) in the current bit position
            int leastCommonBit = diagnosticReport
                .Select(line => line[position])
                .GroupBy(elem => elem)
                .OrderBy(group => group.Count())
                .ThenBy(g => g.Key) // If 0 and 1 are equally common, keep values with a 0 in the position being considered.
                .First().Key;
            // and keep only numbers with that bit in that position.
            List<List<int>> numbersToKeep = diagnosticReport.Where(line => line[position] == leastCommonBit).ToList();
            return (leastCommonBit << inversedPosition) + FindCo2ScrubberRating(numbersToKeep, position + 1);
        }
    }
}
