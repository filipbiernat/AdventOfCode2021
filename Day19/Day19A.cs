using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace AdventOfCode2021.Day19
{
    using Matrix = Matrix<double>;

    public class Day19A : IDay
    {
        public void Run()
        {
            // The submarine has automatically summarized the relative positions of beacons detected by each scanner.
            string[] input = File.ReadAllText(@"..\..\..\Day19\Day19.txt").Split("\r\n\r\n");

            // The beacons and scanners float motionless in the water; they're designed to maintain the same position for long
            // periods of time. Each scanner is capable of detecting all beacons in a large cube centered on the scanner; beacons
            // that are at most 1000 units away from the scanner in each of the three axes (x, y, and z) have their precise
            // position determined relative to the scanner. However, scanners cannot detect other scanners.
            List<Scanner> scanners = input.Select(line => new Scanner(line)).ToList();
            scanners[0].Localized = true;

            // Unfortunately, while each scanner can report the positions of all detected beacons relative to itself, the scanners
            // do not know their own position. You'll need to determine the positions of the beacons and scanners yourself.
            // Assemble the full map of beacons.
            HashSet<Matrix> fullMapOfBeacons = scanners[0].Beacons.ToHashSet();
            while (scanners.Where(scanner => !scanner.Localized).Any())
            {
                foreach (Scanner scanner in scanners.Where(scanner => !scanner.Localized))
                {
                    if (scanner.LocalizeAgainst(fullMapOfBeacons.ToList()))
                    {
                        fullMapOfBeacons.UnionWith(scanner.GetReorientedBeacons());
                    }
                }
            }

            // How many beacons are there?
            int output = fullMapOfBeacons.Count();
            Console.WriteLine("Solution: {0}.", output);
        }
    }
}
