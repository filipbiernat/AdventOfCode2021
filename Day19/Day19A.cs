using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day19
{
    public class Day19A : IDay
    {
        public void Run()
        {
            // The submarine has automatically summarized the relative positions of beacons detected by each scanner.
            string[] input = File.ReadAllText(@"..\..\..\Day19\Day19.txt").Split("\r\n\r\n");
            List<Scanner> scanners = input.Select(line => new Scanner(line)).ToList();
            scanners.First().SetAsLocalized();

            // The beacons and scanners float motionless in the water; they're designed to maintain the same position for long
            // periods of time. Each scanner is capable of detecting all beacons in a large cube centered on the scanner; beacons
            // that are at most 1000 units away from the scanner in each of the three axes (x, y, and z) have their precise
            // position determined relative to the scanner. However, scanners cannot detect other scanners.
            scanners.ForEach(scanner => scanner.FindNearbyScanners(scanners));

            // Assemble the full map of beacons.
            HashSet<MathNet.Numerics.LinearAlgebra.Vector<double>> fullMapOfBeacons = scanners.First().AssembleMapOfBeacons();

            // How many beacons are there?
            int output = fullMapOfBeacons.Count;
            Console.WriteLine("Solution: {0}.", output);
        }
    }
}
