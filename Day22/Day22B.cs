using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day22
{
    public class Day22B : IDay
    {
        public void Run()
        {
            // To reboot the reactor, you just need to set all of the cubes to either on or off by following a list of reboot
            // steps (your puzzle input).
            string[] input = File.ReadAllLines(@"..\..\..\Day22\Day22.txt");
            List<RebootStep> rebootSteps = input.Select(step => new RebootStep(step))
                .ToList();

            // At the start of the reboot process, they are all off.
            List<Cuboid> cuboids = new();

            // Execute the reboot steps.
            rebootSteps.ForEach(step => step.ApplyStep(ref cuboids));

            // Afterward, considering all cubes, how many cubes are on?
            long output = cuboids.Select(cuboid => cuboid.GetCubesCount()).Sum();
            Console.WriteLine("Solution: {0}.", output);
        }
    }
}
