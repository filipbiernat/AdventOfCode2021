using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day7
{
    public class Day7B : IDay
    {
        public void Run()
        {
            // You quickly make a list of the horizontal position of each crab (your puzzle input). 
            string input = File.ReadAllLines(@"..\..\..\Day7\Day7.txt").First();
            IEnumerable<(int position, int count)> horizontalPositions = input.Split(",")
                .Select(int.Parse)
                .GroupBy(elem => elem)
                .OrderBy(group => group.Key)
                .Select(group => (group.Key, group.Count()));

            // Each change of 1 step in horizontal position costs 1 more unit of fuel than the last.
            int GetFuelCost(int position) => horizontalPositions.Select(
                elem => elem.count * Enumerable.Range(1, Math.Abs(elem.position - position)).Sum()).Sum();

            // Crab submarines have limited fuel, so you need to find a way to make all of their horizontal positions match while
            // requiring them to spend as little fuel as possible.
            int output = Enumerable.Range(horizontalPositions.First().position, horizontalPositions.Last().position)
                .Select(GetFuelCost).Min();

            // Determine the horizontal position that the crabs can align to using the least fuel possible.
            // How much fuel must they spend to align to that position?
            Console.WriteLine("Solution: {0}.", output);
        }
    }
}
