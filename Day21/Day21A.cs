using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day21
{
    public class Day21A : IDay
    {
        public void Run()
        {
            string[] input = File.ReadAllLines(@"..\..\..\Day21\Day21.txt");
            List<Player> players = input.Select(line => new Player(line)).ToList();

            // Play a practice game using the deterministic 100-sided die.
            DeterministicDie die = new();
            for (bool isGameWon = false; !isGameWon;)
            {
                // Players take turns moving.
                foreach (Player player in players)
                {
                    // On each player's turn, the player rolls the die three times.
                    player.TakeTurn(die.Roll() + die.Roll() + die.Roll());
                    // The game immediately ends as a win for any player whose score reaches at least 1000.
                    if (player.IsWinner(ScoreToWin))
                    {
                        isGameWon = true;
                        break;
                    }
                }
            }

            // The moment either player wins, what do you get if you multiply the score of the losing player by the number of
            // times the die was rolled during the game?
            int scoreOfTheLosingPlayer = players[0].IsWinner(ScoreToWin) ? players[1].Score : players[0].Score;
            int output = scoreOfTheLosingPlayer * die.RollsCount;
            Console.WriteLine("Solution: {0}.", output);
        }

        private class DeterministicDie
        {
            public int RollsCount = 0;

            // This die always rolls 1 first, then 2, then 3, and so on up to 100, after which it starts over at 1 again.
            private int LastRolled = 0;
            public int Roll()
            {
                ++RollsCount;
                return LastRolled = LastRolled % Sides + 1;
            }

            // Since the first game is a practice game, the submarine opens a compartment labeled deterministic dice and a
            // 100-sided die falls out.
            private static readonly int Sides = 100;
        }

        // The game immediately ends as a win for any player whose score reaches at least 1000.
        private static readonly int ScoreToWin = 1000;
    }
}
