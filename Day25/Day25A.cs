using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day25
{
    public class Day25A : IDay
    {
        private Dictionary<Coords, char> SeaCucumbers = new();
        private Coords Edge = new(0, 0);

        public void Run()
        {
            // There are two herds of sea cucumbers sharing the same region; one always moves east (>), while the other always
            // moves south (v). Each location can contain at most one sea cucumber; the remaining locations are empty (.).
            // The submarine helpfully generates a map of the situation (your puzzle input). 
            string[] input = File.ReadAllLines(@"..\..\..\Day25\Day25.txt");
            SeaCucumbers = input
                .SelectMany((row, rowIndex) => row
                    .ToCharArray()
                    .Select((elem, colIndex) => new KeyValuePair<Coords, char>(new Coords(rowIndex, colIndex), elem)))
                .Where(pair => pair.Value != '.')
                .ToDictionary(pair => pair.Key, pair => pair.Value);
            Edge = new(input.Length, input[0].Length);

            Dictionary<Coords, char> previousStateOfSeaCucumbers = new();
            int step = 0;
            while (!AreDictionariesEqual(SeaCucumbers, previousStateOfSeaCucumbers))
            {
                previousStateOfSeaCucumbers = SeaCucumbers;
                // Every step, the sea cucumbers in the east-facing herd attempt to move forward one location.
                SeaCucumbers = SeaCucumbers
                    .Select(seaCucumber => TryMoveEast(seaCucumber))
                    .ToDictionary(pair => pair.Key, pair => pair.Value);
                // Then the sea cucumbers in the south-facing herd attempt to move forward one location. 
                SeaCucumbers = SeaCucumbers
                    .Select(seaCucumber => TryMoveSouth(seaCucumber))
                    .ToDictionary(pair => pair.Key, pair => pair.Value);
                ++step;
            }

            // Find somewhere safe to land your submarine. What is the first step on which no sea cucumbers move?
            int output = step;
            Console.WriteLine("Solution: {0}.", output);
        }

        private KeyValuePair<Coords, char> TryMoveEast(KeyValuePair<Coords, char> seaCucumber)
        {
            if (seaCucumber.Value == '>')
            {
                // Due to strong water currents in the area, sea cucumbers that move off the right edge of the map appear on the
                // left edge, and sea cucumbers that move off the bottom edge of the map appear on the top edge. 
                Coords newCoords = new(seaCucumber.Key.row, (seaCucumber.Key.column + 1) % Edge.column);
                return TryMove(seaCucumber, newCoords);
            }
            return seaCucumber;
        }

        private KeyValuePair<Coords, char> TryMoveSouth(KeyValuePair<Coords, char> seaCucumber)
        {
            if (seaCucumber.Value == 'v')
            {
                // Due to strong water currents in the area, sea cucumbers that move off the right edge of the map appear on the
                // left edge, and sea cucumbers that move off the bottom edge of the map appear on the top edge. 
                Coords newCoords = new((seaCucumber.Key.row + 1) % Edge.row, seaCucumber.Key.column);
                return TryMove(seaCucumber, newCoords);
            }
            return seaCucumber;
        }

        private KeyValuePair<Coords, char> TryMove(KeyValuePair<Coords, char> seaCucumber, Coords newCoords)
        {
            // When a herd moves forward, every sea cucumber in the herd first simultaneously considers whether there is a sea
            // cucumber in the adjacent location it's facing (even another sea cucumber facing the same direction), and then
            // every sea cucumber facing an empty location simultaneously moves into that location.
            return SeaCucumbers.ContainsKey(newCoords) ? seaCucumber : new(newCoords, seaCucumber.Value);
        }

        private static bool AreDictionariesEqual(Dictionary<Coords, char> lhs, Dictionary<Coords, char> rhs)
        {
            return lhs.Count == rhs.Count && !lhs.Except(rhs).Any();
        }

        private class Coords
        {
            public int row;
            public int column;

            public Coords(int row, int column)
            {
                this.row = row;
                this.column = column;
            }

            public static Coords operator +(Coords lhs, Coords rhs) => new(lhs.row + rhs.row, lhs.column + rhs.column);
            public override int GetHashCode() => (row.GetHashCode() << 2) ^ column.GetHashCode();
            public override bool Equals(object? obj) =>
                (obj != null) &&
                (row == ((Coords)obj).row) &&
                (column == ((Coords)obj).column);
        }
    }
}
