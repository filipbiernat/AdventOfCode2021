using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace AdventOfCode2021.Day5
{
    public class Day5A : IDay
    {
        public void Run()
        {
            // You come across a field of hydrothermal vents on the ocean floor! 
            // They tend to form in lines; the submarine helpfully produces a list of nearby lines of vents for you to review. 
            string[] input = File.ReadAllLines(@"..\..\..\Day5\Day5.txt");
            IEnumerable<Point> hydrothermalVentsPoints = input
                .Select(line => line
                    .Split(new string[] { "->", "," }, StringSplitOptions.TrimEntries)
                    .Select(int.Parse)
                    .ToList())
                // For now, only consider horizontal and vertical lines: lines where either x1 = x2 or y1 = y2.
                .Where(elem => (elem[0] == elem[2]) || (elem[1] == elem[3]))
                .SelectMany(elem => GetPointsInBetween(new Point(elem[0], elem[1]), new Point(elem[2], elem[3])));

            // To avoid the most dangerous areas, you need to determine the number of points where at least two lines overlap. 
            int output = hydrothermalVentsPoints
                .GroupBy(elem => elem)
                .Where(group => group.Count() >= 2)
                .Select(elem => elem.Key)
                .ToList()
                .Count;

            // Consider only horizontal and vertical lines. At how many points do at least two lines overlap?
            Console.WriteLine("Solution: {0}.", output);
        }

        private static List<Point> GetPointsInBetween(Point pointA, Point pointB)
        {
            Size delta = new (Math.Sign(pointB.X - pointA.X), Math.Sign(pointB.Y - pointA.Y));
            List<Point> pointsInBetween = new() { pointA };
            while (pointsInBetween.Last() != pointB)
            {
                pointsInBetween.Add(pointsInBetween.Last() + delta);
            }
            return pointsInBetween;
        }
    }
}
