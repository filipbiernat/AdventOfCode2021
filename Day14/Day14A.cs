using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day14
{
    public class Day14A : IDay
    {
        public void Run()
        {
            // The submarine manual contains instructions for finding the optimal polymer formula; specifically, it offers a polymer
            // template and a list of pair insertion rules (your puzzle input).
            string[] input = File.ReadAllText(@"..\..\..\Day14\Day14.txt").Split("\r\n\r\n");
            // The first line is the polymer template - this is the starting point of the process.
            List<char> polymerTemplate = input[0].ToCharArray().ToList();
            // The following section defines the pair insertion rules. A rule like AB -> C means that when elements A and B are
            // immediately adjacent, element C should be inserted between them. These insertions all happen simultaneously.
            IEnumerable<Tuple<char, char, char>> insertionRules = input[1]
                .Split("\r\n")
                .Select(rule => rule.ToCharArray())
                .Select(ruleChars => new Tuple<char, char, char>(ruleChars.ElementAt(0), ruleChars.ElementAt(1), ruleChars.Last()));

            Dictionary<char, long> quantitiesOfPolymerElements = polymerTemplate
                .GroupBy(elem => elem)
                .ToDictionary(elem => elem.Key, elem => (long)elem.Count());
            // Note that these pairs overlap: the second element of one pair is the first element of the next pair.
            Dictionary<Tuple<char, char>, long> quantitiesOfPolymerPairs = polymerTemplate
                .Zip(polymerTemplate.Skip(1), (lhsElem, rhsElem) => new Tuple<char, char>(lhsElem, rhsElem))
                .GroupBy(pair => pair)
                .ToDictionary(pair => pair.Key, pair => (long)pair.Count());

            // Apply 10 steps of pair insertion to the polymer template.
            for (int step = 0; step < 10; ++step)
            {
                Dictionary<Tuple<char, char>, long> quantitiesOfNextPolymerPairs = new(quantitiesOfPolymerPairs);
                foreach ((char lhsElem, char rhsElem, char newElem) in insertionRules)
                {
                    Tuple<char, char> currentPair = new(lhsElem, rhsElem);
                    if (quantitiesOfPolymerPairs.ContainsKey(currentPair))
                    {
                        long quantityOfCurrentPair = quantitiesOfPolymerPairs[currentPair];
                        Increase(quantitiesOfNextPolymerPairs, currentPair, -quantityOfCurrentPair);
                        Increase(quantitiesOfNextPolymerPairs, new Tuple<char, char>(lhsElem, newElem), quantityOfCurrentPair);
                        Increase(quantitiesOfNextPolymerPairs, new Tuple<char, char>(newElem, rhsElem), quantityOfCurrentPair);
                        Increase(quantitiesOfPolymerElements, newElem, quantityOfCurrentPair);
                    }
                }
                quantitiesOfPolymerPairs = quantitiesOfNextPolymerPairs;
            }

            // Find the most and least common elements in the result. What do you get if you take the quantity of the most common
            // element and subtract the quantity of the least common element?
            IOrderedEnumerable<long> elementQuantities = quantitiesOfPolymerElements
                .Select(quantity => quantity.Value)
                .OrderByDescending(quantity => quantity);
            long output = elementQuantities.First() - elementQuantities.Last();
            Console.WriteLine("Solution: {0}.", output);
        }

        private static void Increase(Dictionary<Tuple<char, char>, long> dict, Tuple<char, char> key, long value)
        {
            if (!dict.ContainsKey(key))
            {
                dict[key] = 0;
            }
            dict[key] += value;
        }

        private static void Increase(Dictionary<char, long> dict, char key, long value)
        {
            if (!dict.ContainsKey(key))
            {
                dict[key] = 0;
            }
            dict[key] += value;
        }
    }
}
