using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day21
{
    public class Day21B : IDay
    {
        public List<Tuple<int, long>> TripleRollOutcomes = new();
        private Dictionary<int, long> WinCountsForPlayers = new();

        public void Run()
        {
            string[] input = File.ReadAllLines(@"..\..\..\Day21\Day21.txt");
            List<Player> players = input.Select(line => new Player(line)).ToList();
            WinCountsForPlayers = players.ToDictionary(player => player.Id, _ => 0L);

            // Using your given starting positions, determine every possible outcome.
            FindTripleRollOutcomes();
            NextTurnWithDiracDice(nextPlayer: players.First(), anotherPlayer: players.Last());

            // Find the player that wins in more universes; in how many universes does that player win?
            long output = WinCountsForPlayers.Values.ToList().Max();
            Console.WriteLine("Solution: {0}.", output);
        }

        private void NextTurnWithDiracDice(Player nextPlayer, Player anotherPlayer, long numberOfUniverses = 1)
        {
            if (anotherPlayer.IsWinner(ScoreToWin))
            {
                WinCountsForPlayers[anotherPlayer.Id] += numberOfUniverses;
            }
            else
            {
                // An informational brochure in the compartment explains that this is a quantum die: when you roll it, the
                // universe splits into multiple copies, one copy for each possible outcome of the die. In this case, rolling
                // the die always splits the universe into three copies: one where the outcome of the roll was 1, one where it
                // was 2, and one where it was 3.
                foreach ((int rolledValues, long numberOfUniversesForRolledValues) in TripleRollOutcomes)
                {
                    // On each player's turn, the player rolls the die three times.
                    Player currentPlayer = new(nextPlayer);
                    currentPlayer.TakeTurn(rolledValues);

                    NextTurnWithDiracDice(anotherPlayer, currentPlayer, numberOfUniverses * numberOfUniversesForRolledValues);
                }
            }
        }

        private void FindTripleRollOutcomes()
        {
            List<int> numbersToRoll = new() { 1, 2, 3 };

            TripleRollOutcomes = numbersToRoll
                .SelectMany(firstRoll => numbersToRoll
                    .SelectMany(secondRoll => numbersToRoll
                        .Select(thirdRoll => firstRoll + secondRoll + thirdRoll)))
                .GroupBy(sumOfRolledNumbers => sumOfRolledNumbers)
                .Select(sumOfRolledNumbers => Tuple.Create(sumOfRolledNumbers.Key, (long)sumOfRolledNumbers.Count()))
                .ToList();
        }

        // The game is played the same as before, although to prevent things from getting too far out of hand, the game now ends
        // when either player's score reaches at least 21.
        private static readonly int ScoreToWin = 21;
    }
}
