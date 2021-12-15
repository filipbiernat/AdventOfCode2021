using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day15
{
    public class Day15A : IDay
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
            Console.WriteLine(output);
        }

        private static int BellmanFord(int verticesCount, List<GraphEdge> edges)
        {
            int[] distance = Enumerable.Repeat(int.MaxValue, verticesCount).ToArray();
            distance[0] = 0;
            for (int i = 1; i <= verticesCount - 1; ++i)
            {
                for (int j = 0; j < edges.Count; ++j)
                {
                    int u = edges[j].SourceId;
                    int v = edges[j].DestinationId;
                    int weight = edges[j].Weight;
                    if (distance[u] != int.MaxValue && distance[u] + weight < distance[v])
                    {
                        distance[v] = distance[u] + weight;
                    }
                }
            }
            for (int i = 0; i < edges.Count; ++i)
            {
                int u = edges[i].SourceId;
                int v = edges[i].DestinationId;
                int weight = edges[i].Weight;
                if (distance[u] != int.MaxValue && distance[u] + weight < distance[v])
                {
                    Console.WriteLine("Graph contains negative weight cycle.");
                }
            }
            return distance.Last();
        }
    }
}
