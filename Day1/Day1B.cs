using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day1
{
    public class Day1B : IDay
    {
        public void Run()
        {
            // Each line is a measurement of the sea floor depth as the sweep looks further and further away from the submarine.
            string[] input = File.ReadAllLines(@"..\..\..\Day1\Day1.txt");
            List<int> depthMeasurements = input.Select(int.Parse).ToList();

            // Your goal now is to count the number of times the sum of measurements in this sliding window increases from the previous sum.
            int output = Enumerable.Range(0, depthMeasurements.Count - 3)
                  .Where(i => depthMeasurements[i + 3] > depthMeasurements[i])
                  .Count();

            // Consider sums of a three-measurement sliding window. How many sums are larger than the previous sum?
            Console.WriteLine("Solution: {0}.", output);
        }
    }
}
