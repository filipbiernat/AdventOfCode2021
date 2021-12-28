using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day22
{
    public class RebootStep
    {
        private readonly Cuboid CuboidForRebootStep;

        public RebootStep(string input)
        {
            // Each step specifies a cuboid (the set of all cubes that have coordinates which fall within ranges for x, y, and z)
            // and whether to turn all of the cubes in that cuboid on or off.
            MatchCollection matchCollection = new Regex(@"on|off|(-?[0-9]\d*)").Matches(input);

            CuboidForRebootStep = new(
                on: matchCollection.ElementAt(0).Value == "on",
                x0: long.Parse(matchCollection.ElementAt(1).Value),
                x1: long.Parse(matchCollection.ElementAt(2).Value),
                y0: long.Parse(matchCollection.ElementAt(3).Value),
                y1: long.Parse(matchCollection.ElementAt(4).Value),
                z0: long.Parse(matchCollection.ElementAt(5).Value),
                z1: long.Parse(matchCollection.ElementAt(6).Value));
        }

        public void ApplyStep(ref List<Cuboid> cuboids)
        {
            AddOverlappingCuboidsIfAny(ref cuboids);
            AddCuboidForThisRebootStepIfOn(ref cuboids);
        }

        private void AddOverlappingCuboidsIfAny(ref List<Cuboid> cuboids)
        {
            List<Cuboid> overlappingCuboids = cuboids.Where(cuboid => CuboidForRebootStep.AreCuboidsOverlapping(cuboid))
                .Select(cuboid => CuboidForRebootStep.CalculateOverlappingCuboid(cuboid))
                .ToList();
            cuboids.AddRange(overlappingCuboids);
        }

        private void AddCuboidForThisRebootStepIfOn(ref List<Cuboid> cuboids)
        {
            if (CuboidForRebootStep.on)
            {
                cuboids.Add(CuboidForRebootStep);
            }
        }

        public bool IsValidForInitializationProcedure() => CuboidForRebootStep.IsValidForInitializationProcedure();
    }
}
