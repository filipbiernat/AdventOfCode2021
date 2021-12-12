using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day12
{
    public class Day12B : IDay
    {
        private readonly Dictionary<string, List<string>> CaveMap = new();

        public void Run()
        {
            // The sensors are still mostly working, and so you build a rough map of the remaining caves (your puzzle input).
            string[] input = File.ReadAllLines(@"..\..\..\Day12\Day12.txt");
            // This is a list of how all of the caves are connected.
            input.Select(connection => connection.Split("-"))
                .ToList()
                .ForEach(caves =>
                {
                    AddConnectionToTheCaveMap(caves[0], caves[1]);
                    AddConnectionToTheCaveMap(caves[1], caves[0]);
                });

            // How many paths through this cave system are there?
            int output = CountPaths("start", new HashSet<string>());
            Console.WriteLine("Solution: {0}.", output);
        }

        private int CountPaths(string cave, HashSet<string> visitedSmallCaves, bool smallCaveVisitedTwice = false)
        {
            // Your goal is to find the number of distinct paths that start at start, end at end.
            if (cave == "end")
            {
                return 1;
            }
            // There are two types of caves: big caves (written in uppercase, like A) and small caves (written in lowercase,
            // like b). It would be a waste of time to visit any small cave more than once, but big caves are large enough
            // that it might be worth visiting them multiple times. So, all paths you find should visit small caves at most
            // once, and can visit big caves any number of times.
            if (visitedSmallCaves.Contains(cave))
            {
                // After reviewing the available paths, you realize you might have time to visit a single small cave twice.
                // Specifically, big caves can be visited any number of times, a single small cave can be visited at most
                // twice, and the remaining small caves can be visited at most once. However, the caves named start and end
                // can only be visited exactly once each: once you leave the start cave, you may not return to it.
                if (!smallCaveVisitedTwice && cave != "start")
                {
                    smallCaveVisitedTwice = true;
                }
                else
                {
                    return 0;
                }
            }
            if (cave.ToCharArray().All(character => char.IsLower(character)))
            {
                visitedSmallCaves.Add(cave);
            }
            // The only way to know if you've found the best path is to find all of them.
            return CaveMap[cave].Select(nextCave => CountPaths(nextCave,
                new HashSet<string>(visitedSmallCaves), smallCaveVisitedTwice)).Sum();
        }

        private void AddConnectionToTheCaveMap(string firstCave, string secondCave)
        {
            if (!CaveMap.ContainsKey(firstCave))
            {
                CaveMap[firstCave] = new();
            }
            CaveMap[firstCave].Add(secondCave);
        }
    }
}
