using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Drawing;

namespace AdventOfCode2021.Day17
{
    public class Day17A : IDay
    {
        public void Run()
        {
            // For the probe to successfully make it into the trench, the probe must be on some trajectory that causes it to be
            // within a target area after any step.
            string input = File.ReadAllText(@"..\..\..\Day17\Day17.txt");
            List<int> oceanTrenchCoordinates = Regex.Matches(input, @"-?\d*\-{0,1}\d+")
                .Select(match => int.Parse(match.Value))
                .ToList();
            Rectangle oceanTrench = new(x: oceanTrenchCoordinates[0],
                y: oceanTrenchCoordinates[2],
                width: oceanTrenchCoordinates[1] - oceanTrenchCoordinates[0] + 1,
                height: oceanTrenchCoordinates[3] - oceanTrenchCoordinates[2] + 1);

            // The probe launcher on your submarine can fire the probe with any integer velocity in the x (forward) and y (upward,
            // or downward if negative) directions.
            // Find the initial velocity that causes the probe to reach the highest y position and still eventually be within the
            // target area after any step.
            int rangeLimit = 200;
            int output = Enumerable.Range(1, rangeLimit)
                .SelectMany(vx => Enumerable.Range(-rangeLimit, rangeLimit * 2 + 1)
                    .Select(vy => new Size(vx, vy)))
                .Select(velocity => FireAndGetYMax(velocity, oceanTrench))
                .Max();

            // What is the highest y position it reaches on this trajectory?
            Console.WriteLine("Solution: {0}.", output);
        }

        private static int FireAndGetYMax(Size velocity, Rectangle oceanTrench)
        {
            // If you're going to fire a highly scientific probe out of a super cool probe launcher, you might as well do it with
            // style. How high can you make the probe go while still reaching the target area?
            int yMax = int.MinValue;

            // The probe's x,y position starts at 0,0.
            Point position = new(0, 0);
            // Then, it will follow some trajectory by moving in steps.
            while (0 <= position.X && position.X < oceanTrench.Right && (position.Y >= oceanTrench.Top || velocity.Height > 0))
            {
                // The probe's x position increases by its x velocity. The probe's y position increases by its y velocity.
                position += velocity;
                yMax = Math.Max(yMax, position.Y);

                // Due to drag, the probe's x velocity changes by 1 toward the value 0; that is, it decreases by 1 if it is greater
                // than 0, increases by 1 if it is less than 0, or does not change if it is already 0.
                // Due to gravity, the probe's y velocity decreases by 1.
                velocity -= new Size(Math.Sign(velocity.Width), 1);

                // For the probe to successfully make it into the trench, the probe must be on some trajectory that causes it to be
                // within a target area after any step.
                if (oceanTrench.Contains(position))
                {
                    return yMax;
                }
            };
            return int.MinValue;
        }
    }
}
