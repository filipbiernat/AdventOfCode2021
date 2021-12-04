using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day4
{
    public class Day4B : IDay
    {
        private class BingoNumber
        {

            public int number;
            public int row;
            public int column;

            public BingoNumber(int number, int row, int column)
            {
                this.number = number;
                this.row = row;
                this.column = column;
            }
        }

        private class BingoBoard
        {
            public List<BingoNumber> Numbers = new();
            public List<BingoNumber> MarkedNumbers = new();
            public int MarksCount = 0;

            public BingoBoard(string input)
            {
                Numbers.AddRange(input.Split("\r\n")
                    .SelectMany((line, row) => line.Split()
                        .Where(elem => !string.IsNullOrWhiteSpace(elem))
                        .Select(int.Parse)
                        .Select((val, column) => new BingoNumber(val, row, column))));
            }

            public void Mark(int number)
            {
                if (!IsWinner())
                {
                    MarkedNumbers.AddRange(Numbers.Where(elem => elem.number == number));
                    ++MarksCount;
                }
            }

            public IEnumerable<BingoNumber> GetUnmarkedNumbers()
            {
                return Numbers.Except(MarkedNumbers);
            }

            public bool IsWinner()
            {
                // (Numbers may not appear on all boards.)
                // If all numbers in any row or any column of a board are marked, that board wins.
                return MarkedNumbers.Any() && (IsFullGroupMarked(elem => elem.row) || IsFullGroupMarked(elem => elem.column));
            }

            private bool IsFullGroupMarked(Func<BingoNumber, int> groupSelector)
            {
                return MarkedNumbers.GroupBy(groupSelector)
                    .Select(group => group.Count())
                    .OrderBy(elem => elem)
                    .Last() == 5;
            }
        }

        public void Run()
        {
            // The submarine has a bingo subsystem to help passengers (currently, you and the giant squid) pass the time.
            // It automatically generates a random order in which to draw numbers and a random set of boards (your puzzle input).
            string[] input = File.ReadAllText(@"..\..\..\Day4\Day4.txt").Split("\r\n\r\n");
            // Numbers are chosen at random, and the chosen number is marked on all boards on which it appears.
            List<int> numbers = input.First().Split(",").Select(Int32.Parse).ToList();
            // Bingo is played on a set of boards each consisting of a 5x5 grid of numbers.
            List<BingoBoard> boards = input.Skip(1).Select(elem => new BingoBoard(elem)).ToList();

            // Figure out which board will win last.
            // Once it wins, what would its final score be?
            foreach (int number in numbers)
            {
                boards.ForEach(s => s.Mark(number));
                if (boards.All(board => board.IsWinner())) break;
            }

            // The score of the winning board can now be calculated.
            BingoBoard winningBoard = boards.OrderByDescending(board => board.MarksCount).First();
            // Start by finding the sum of all unmarked numbers.
            int sumOfUnmarked = winningBoard.GetUnmarkedNumbers().Sum(s => s.number);
            // Then, multiply that sum by the number that was just called when the board won.
            int output = sumOfUnmarked * winningBoard.MarkedNumbers.Last().number;

            Console.WriteLine(output);
        }
    }
}
