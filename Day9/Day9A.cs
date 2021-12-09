using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day9
{
    public class Day9A : IDay
    {
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

        public void Run()
        {
            // The submarine generates a heightmap of the floor of the nearby caves for you (your puzzle input).
            string[] input = File.ReadAllLines(@"..\..\..\Day9\Day9.txt");

            // Each number corresponds to the height of a particular location, where 9 is the highest and 0 is the lowest a
            // location can be.
            Dictionary<Coords, int> heightmap = input
                .SelectMany((row, rowIndex) => row
                    .ToCharArray()
                    .Select((elem, colIndex) => new KeyValuePair<Coords, int>(
                        new Coords(rowIndex, colIndex), (int)char.GetNumericValue(elem))))
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            // Your first goal is to find the low points - the locations that are lower than any of its adjacent locations. Most
            // locations have four adjacent locations (up, down, left, and right); locations on the edge or corner of the map
            // have three or two adjacent locations, respectively. (Diagonal locations do not count as adjacent.)
            IEnumerable<int> heightsOfLowPoints = heightmap
                .Where(mapElem => GetNeighbourPoints()
                    .Select(neighbourCoords => mapElem.Key + neighbourCoords)
                    .Where(neighbourCoords => heightmap.ContainsKey(neighbourCoords))
                    .All(neighbourCoords => mapElem.Value < heightmap[neighbourCoords]))
                .Select(mapElem => mapElem.Value);

            // The risk level of a low point is 1 plus its height.
            int output = heightsOfLowPoints.Select(height => 1 + height).Sum();

            // What is the sum of the risk levels of all low points on your heightmap?
            Console.WriteLine("Solution: {0}.", output);
        }

        private static List<Coords> GetNeighbourPoints()
        {
            return new() { new Coords(0, 1), new Coords(0, -1), new Coords(1, 0), new Coords(-1, 0) };
        }
    }
}
