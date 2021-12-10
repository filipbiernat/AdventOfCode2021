using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day10
{
    public class Day10A : IDay
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

            // Find the first illegal character in each corrupted line of the navigation subsystem.
            IEnumerable<char> firstIllegalCharacters = input.Select(elem => elem.ToCharArray())
                .Select(elem => FindIllegalCharacter(elem))
                .Where(elem => elem != '-');

            // What is the total syntax error score for those errors?
            List<long> scores = firstIllegalCharacters.Select(CalculateScore).ToList();
            long output = scores.Sum();
            Console.WriteLine("Solution: {0}.", output);
        }

        private static char FindIllegalCharacter(char[] line)
        {
            // Some lines are incomplete, but others are corrupted.
            // Find and discard the corrupted lines first.
            // A corrupted line is one where a chunk closes with the wrong character - that is, where the characters it opens and
            // closes with do not form one of the four legal pairs listed above.
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
                    // Stop at the first incorrect closing character on each corrupted line.
                    return c;
                }
            }
            // Some of the lines aren't corrupted, just incomplete; you can ignore these lines for now.
            return '-';
        }

        private static long CalculateScore(char firstIllegalCharacter)
        {
            // Did you know that syntax checkers actually have contests to see who can get the high score for syntax errors in a
            // file? It's true! To calculate the syntax error score for a line, take the first illegal character on the line and
            // look it up in the following table:
            // - ): 3 points.
            // - ]: 57 points.
            // - }: 1197 points.
            // - >: 25137 points.
            Dictionary<char, long> scores = new() { { '-', 0 }, { ')', 3 }, { ']', 57 }, { '}', 1197 }, { '>', 25137 } };
            return scores[firstIllegalCharacter];
        }
    }
}
