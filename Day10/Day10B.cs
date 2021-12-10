using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day10
{
    public class Day10B : IDay
    {
        public void Run()
        {
            // The navigation subsystem syntax is made of several lines containing chunks. There are one or more chunks on each
            // line, and chunks contain zero or more other chunks. Adjacent chunks are not separated by any delimiter; if one
            // chunk stops, the next chunk (if any) can immediately start. Every chunk must open and close with one of four legal
            // pairs of matching characters:
            // - If a chunk opens with (, it must close with ).
            // - If a chunk opens with [, it must close with ].
            // - If a chunk opens with {, it must close with }.
            // - If a chunk opens with <, it must close with >.
            // You bring up a copy of the navigation subsystem (your puzzle input).
            string[] input = File.ReadAllLines(@"..\..\..\Day10\Day10.txt");

            IEnumerable<List<char>> missingClosingCharacters = input.Select(elem => elem.ToCharArray())
                .Select(elem => FindMissingClosingCharacters(elem))
                .Where(elem => elem.Any());

            // Autocomplete tools are an odd bunch: the winner is found by sorting all of the scores and then taking the middle
            // score. (There will always be an odd number of scores to consider.)
            // What is the middle score?
            List<long> scores = missingClosingCharacters.Select(CalculateScore).OrderBy(elem => elem).ToList();
            long output = scores.ElementAt(scores.Count / 2);
            Console.WriteLine("Solution: {0}.", output);
        }

        private static List<char> FindMissingClosingCharacters(char[] line)
        {
            // Now, discard the corrupted lines. The remaining lines are incomplete.
            // Incomplete lines don't have any incorrect characters - instead, they're missing some closing characters at the end
            // of the line. To repair the navigation subsystem, you just need to figure out the sequence of closing characters
            // that complete all open chunks in the line.
            // You can only use closing characters(), ], }, or >), and you must add them in the correct order so that only legal
            // pairs are formed and all chunks end up closed.
            Dictionary<char, char> bracketPairs = new() { { '(', ')' }, { '[', ']' }, { '{', '}' }, { '<', '>' } };
            Stack<char> openedChuncks = new();
            foreach (char c in line)
            {
                if (bracketPairs.ContainsKey(c))
                {
                    openedChuncks.Push(c);
                }
                else if (c != bracketPairs[openedChuncks.Pop()])
                {
                    return new List<char>();
                }
            }
            return openedChuncks.Select(elem => bracketPairs[elem]).ToList();
        }

        private static long CalculateScore(List<char> missingClosingCharacters)
        {
            // Did you know that autocomplete tools also have contests? It's true!
            // The score is determined by considering the completion string character-by-character.
            // Start with a total score of 0.
            long score = 0;
            // Then, for each character, multiply the total score by 5 and then increase the total score by the point value
            // given for the character in the following table:
            Dictionary<char, long> scores = new() { { ')', 1 }, { ']', 2 }, { '}', 3 }, { '>', 4 } };
            missingClosingCharacters.ForEach(c => score = score * 5 + scores[c]);
            return score;
        }
    }
}
