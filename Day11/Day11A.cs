using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day11
{
    public class Day11A : IDay
    {
        private Dictionary<Coords, int> Octopuses = new();
        private readonly List<Coords> FlashedThisStep = new();
        private static readonly List<Coords> NeighbourCoords = new()
        {
            new Coords(-1, -1),
            new Coords(-1, 0),
            new Coords(-1, 1),
            new Coords(0, -1),
            new Coords(0, 1),
            new Coords(1, -1),
            new Coords(1, 0),
            new Coords(1, 1)
        };

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
            // Each octopus has an energy level - your submarine can remotely measure the energy level of each octopus.
            string[] input = File.ReadAllLines(@"..\..\..\Day11\Day11.txt");

            // There are 100 octopuses arranged neatly in a 10 by 10 grid.
            // Each octopus slowly gains energy over time and flashes brightly for a moment when its energy is full.
            // The energy level of each octopus is a value between 0 and 9.
            Octopuses = input
                .SelectMany((row, rowIndex) => row
                    .ToCharArray()
                    .Select((elem, colIndex) => new KeyValuePair<Coords, int>(
                        new Coords(rowIndex, colIndex), (int)char.GetNumericValue(elem))))
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            int output = 0;
            // You can model the energy levels and flashes of light in steps.
            // Given the starting energy levels of the dumbo octopuses in your cavern, simulate 100 steps.
            for (int step = 0; step < 100; ++step)
            {
                List<Coords> octopusCoords = Octopuses.Select(elem => elem.Key).ToList();
                // An octopus can only flash at most once per step.
                FlashedThisStep.Clear();
                // First, the energy level of each octopus increases by 1.
                IncreaseEnergy(octopusCoords);
                // Then, any octopus with an energy level greater than 9 flashes.
                Flash(octopusCoords);
                // Finally, any octopus that flashed during this step has its energy level set to 0.
                FlashedThisStep.ForEach(coords => Octopuses[coords] = 0);
                output += FlashedThisStep.Count;
            }

            // How many total flashes are there after 100 steps?
            Console.WriteLine("Solution: {0}.", output);
        }

        private void Flash(List<Coords> listOfCoords) => listOfCoords.ForEach(coords => Flash(coords));
        private void IncreaseEnergy(List<Coords> listOfCoords) => listOfCoords.ForEach(coords => IncreaseEnergy(coords));

        private void Flash(Coords coords)
        {
            if (Octopuses.ContainsKey(coords) && !FlashedThisStep.Contains(coords) && Octopuses[coords] > 9)
            {
                FlashedThisStep.Add(coords);
                // This increases the energy level of all adjacent octopuses by 1, including octopuses that are diagonally adjacent.
                List<Coords> adjacentCoords = NeighbourCoords.Select(neighbourCoords => coords + neighbourCoords).ToList();
                IncreaseEnergy(adjacentCoords);
                // If this causes an octopus to have an energy level greater than 9, it also flashes.
                Flash(adjacentCoords);
            }
        }

        private void IncreaseEnergy(Coords coords)
        {
            if (Octopuses.ContainsKey(coords))
            {
                Octopuses[coords] = Octopuses[coords] + 1;
            }
        }
    }
}
