using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day3
{
    public class Day3A : IDay
    {
        public void Run()
        {
            // The diagnostic report (your puzzle input) consists of a list of binary numbers which, when decoded properly, can
            // tell you many useful things about the conditions of the submarine. 
            string[] input = File.ReadAllLines(@"..\..\..\Day3\Day3.txt");
            List<List<int>> diagnosticReport =
                input.Select(line => line.Select(character => (int)char.GetNumericValue(character)).ToList()).ToList();

            // The power consumption can then be found by multiplying the gamma rate by the epsilon rate.
            int output = FindGammaRate(diagnosticReport) * FindEpsilonRate(diagnosticReport);

            // What is the power consumption of the submarine?
            Console.WriteLine("Solution: {0}.", output);
        }

        private int FindGammaRate(List<List<int>> diagnosticReport, int position = 0)
        {
            int inversedPosition = diagnosticReport.First().Count - position - 1;
            if (inversedPosition < 0) return 0;

            // Each bit in the gamma rate can be determined by finding the most common bit in the corresponding position of all
            // numbers in the diagnostic report.
            int mostCommonBit = diagnosticReport
                .Select(line => line[position])
                .GroupBy(elem => elem)
                .OrderBy(group => group.Count())
                .Last().Key;
            return (mostCommonBit << inversedPosition) + FindGammaRate(diagnosticReport, position + 1);
        }

        private int FindEpsilonRate(List<List<int>> diagnosticReport, int position = 0)
        {
            int inversedPosition = diagnosticReport.First().Count - position - 1;
            if (inversedPosition < 0) return 0;

            // The epsilon rate is calculated in a similar way; rather than use the most common bit, the least common bit from
            // each position is used.
            int leastCommonBit = diagnosticReport
                .Select(line => line[position])
                .GroupBy(elem => elem)
                .OrderBy(group => group.Count())
                .First().Key;
            return (leastCommonBit << inversedPosition) + FindEpsilonRate(diagnosticReport, position + 1);
        }
    }
}
