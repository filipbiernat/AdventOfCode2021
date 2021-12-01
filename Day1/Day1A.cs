using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day1
{
    public class Day1A : IDay
    {
        public void Run()
        {
            // Each line is a measurement of the sea floor depth as the sweep looks further and further away from the submarine.
            string[] input = File.ReadAllLines(@"..\..\..\Day1\Day1.txt");
            List<int> depthMeasurements = input.Select(int.Parse).ToList();

            // The first order of business is to figure out how quickly the depth increases.
            int output = Enumerable.Range(0, depthMeasurements.Count - 1)
                  .Where(i => depthMeasurements[i + 1] > depthMeasurements[i])
                  .Count();

            // How many measurements are larger than the previous measurement?
            Console.WriteLine("Solution: {0}.", output);
        }
    }
}
