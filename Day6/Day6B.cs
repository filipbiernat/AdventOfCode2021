using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day6
{
    public class Day6B : IDay
    {
        public void Run()
        {
            // Realizing what you're trying to do, the submarine automatically produces a list of the ages of several hundred nearby
            // lanternfish (your puzzle input).
            string input = File.ReadAllLines(@"..\..\..\Day6\Day6.txt").First();
            List<long> fishGenerationCounts = new() { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            input.Split(",")
                .Select(long.Parse)
                .GroupBy(elem => elem)
                .ToList()
                .ForEach(group => fishGenerationCounts[(int)group.Key] = group.Count());

            for (int day = 0; day < 256; ++day)
            {
                // Each day, a 0 becomes a 6 and adds a new 8 to the end of the list, while each other number decreases by 1 if it
                // was present at the start of the day.
                long newFishCount = fishGenerationCounts.First();
                fishGenerationCounts.RemoveAt(0);
                fishGenerationCounts[6] += newFishCount;
                fishGenerationCounts.Add(newFishCount);
            }

            // How many lanternfish would there be after 256 days?
            Console.WriteLine("Solution: {0}.", fishGenerationCounts.Sum());
        }
    }
}
