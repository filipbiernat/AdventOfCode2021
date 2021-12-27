using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day21
{
    public class Player
    {
        public int Id;
        public int Space;
        public int Score;

        public Player(string input)
        {
            Id = PlayerCount++;

            // Each player's starting space is chosen randomly (your puzzle input).
            Space = (int)char.GetNumericValue(input.ToCharArray().Last());

            // Players' scores start at 0.
            Score = 0;
        }

        public Player(Player other)
        {
            Id = other.Id;
            Space = other.Space;
            Score = other.Score;
        }

        public void TakeTurn(int rolledValues)
        {
            // The player moves their pawn that many times forward around the track (that is, moving clockwise on spaces in order
            // of increasing value, wrapping back around to 1 after 10).
            Space = (Space + rolledValues - 1) % TrackSize + 1;
            // After each player moves, they increase their score by the value of the space their pawn stopped on.
            Score += Space;
        }

        public bool IsWinner(int scoreToWin) => Score >= scoreToWin;

        // This game consists of (...) a game board with a circular track containing ten spaces marked 1 through 10 clockwise.
        private static readonly int TrackSize = 10;

        private static int PlayerCount = 0;
    }
}
