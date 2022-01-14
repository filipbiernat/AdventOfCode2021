using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day15
{
    public class Day15B : IDay
    {
        private class GraphEdge
        {
            public int SourceId;
            public int DestinationId;
            public int Weight;
        }

        public void Run()
        {
            // The cavern is large, but has a very low ceiling, restricting your motion to two dimensions.
            // The shape of the cavern resembles a square; a quick scan of chiton density produces a map of risk level throughout
            // the cave (your puzzle input).
            string[] input = File.ReadAllLines(@"..\..\..\Day15\Day15.txt");

            // The number at each position is its risk level; to determine the total risk of an entire path, add up the risk
            // levels of each position you enter (that is, don't count the risk level of your starting position unless you enter
            // it; leaving it adds no risk to your total).
            List<List<int>> riskLevels = input
                .Select(row => row.ToCharArray()
                    .Select(level => (int)char.GetNumericValue(level))
                    .ToList())
                .ToList();

            // The entire cave is actually five times larger in both dimensions than you thought; the area you originally scanned
            // is just one tile in a 5x5 tile area that forms the full map. Your original map tile repeats to the right and
            // downward; each time the tile repeats to the right or downward, all of its risk levels are 1 higher than the tile
            // immediately up or left of it. However, risk levels above 9 wrap back around to 1.
            int rowsAtTheBeginning = riskLevels.Count;
            for (int row = 0; row < rowsAtTheBeginning; ++row)
            {
                List<int> newRow = new(riskLevels[row]);
                for (int tile = 1; tile < 5; ++tile)
                {
                    newRow = newRow.Select(level => level % 9 + 1).ToList();
                    riskLevels[row].AddRange(newRow);
                }
            }
            for (int tile = 1; tile < 5; ++tile)
            {
                for (int row = 0; row < rowsAtTheBeginning; ++row)
                {
                    List<int> newRow = new(riskLevels[row + (tile - 1) * rowsAtTheBeginning]);
                    newRow = newRow.Select(level => level % 9 + 1).ToList();
                    riskLevels.Add(newRow);
                }
            }

            // You start in the top left position, your destination is the bottom right position, and you cannot move diagonally.
            int rows = riskLevels.Count;
            int columns = riskLevels[0].Count;
            List<GraphEdge> edges = new();
            for (int row = 0; row < rows; ++row)
            {
                for (int column = 0; column < columns; ++column)
                {
                    if (row != 0)
                    {
                        edges.Add(new GraphEdge()
                        {
                            SourceId = (row - 1) * rows + column,
                            DestinationId = row * rows + column,
                            Weight = riskLevels[row][column]
                        }); // Down
                        edges.Add(new GraphEdge()
                        {
                            SourceId = row * rows + column,
                            DestinationId = (row - 1) * rows + column,
                            Weight = riskLevels[row - 1][column]
                        }); // Up
                    }
                    if (column != 0)
                    {
                        edges.Add(new GraphEdge()
                        {
                            SourceId = row * rows + (column - 1),
                            DestinationId = row * rows + column,
                            Weight = riskLevels[row][column]
                        }); // Right
                        edges.Add(new GraphEdge()
                        {
                            SourceId = row * rows + column,
                            DestinationId = row * rows + (column - 1),
                            Weight = riskLevels[row][column - 1]
                        }); // Left
                    }
                }
            }

            // What is the lowest total risk of any path from the top left to the bottom right?
            int output = BellmanFord(rows * columns, edges);
            Console.WriteLine("Solution: {0}.", output);
        }

        private static int BellmanFord(int verticesCount, List<GraphEdge> edges)
        {
            int[] distance = Enumerable.Repeat(int.MaxValue, verticesCount).ToArray();
            distance[0] = 0;

            bool distanceImproved;
            do
            {
                distanceImproved = false;
                foreach (GraphEdge edge in edges)
                {
                    if (distance[edge.SourceId] != int.MaxValue &&
                        distance[edge.SourceId] + edge.Weight < distance[edge.DestinationId])
                    {
                        distance[edge.DestinationId] = distance[edge.SourceId] + edge.Weight;
                        distanceImproved = true;
                    }
                }
            }
            while (distanceImproved);

            return distance.Last();
        }
    }
}
