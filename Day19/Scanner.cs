using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace AdventOfCode2021.Day19
{
    using Vector = Vector<double>;

    public class Scanner
    {
        private readonly List<Vector> Beacons = new();
        private readonly List<double> SquaredDistancesBetweenBeacons = new();

        private List<Vector> ReorientedBeacons = new();
        private List<Scanner> NearbyScanners = new();

        private Vector Position = Vector.Build.Dense(3);
        private bool Localized = false;

        private static readonly OrientationMatrices OrientationMatrices = new();
        private static readonly int BeaconsToMatch = 12;

        public Scanner(string input)
        {
            input.Split("\r\n").Skip(1).ToList().ForEach(line => Beacons.Add(MakeBeacon(line)));
            ReorientedBeacons = Beacons;

            // SSD: Sum of Squared Difference, i.e. the squared L2-norm (Euclidean) of the difference.
            SquaredDistancesBetweenBeacons = Beacons
                .SelectMany(first => Beacons.Select(second => Math.Round(Distance.SSD(first, second))))
                .OrderBy(distance => distance)
                .ToList();
        }

        public void SetAsLocalized() => Localized = true;
        public Vector GetPosition() => Position;

        public void FindNearbyScanners(List<Scanner> scanners)
        {
            NearbyScanners = scanners
                .Where(scanner => scanner != this)
                .Where(scanner => IsNearbyScanner(scanner))
                .ToList();
        }

        public HashSet<Vector> AssembleMapOfBeacons()
        {
            // Unfortunately, while each scanner can report the positions of all detected beacons relative to itself, the scanners
            // do not know their own position. You'll need to determine the positions of the beacons and scanners yourself.
            HashSet<Vector> mapOfBeacons = ReorientedBeacons.ToHashSet();
            NearbyScanners
                .Where(nearbyScanner => !nearbyScanner.Localized)
                .Where(nearbyScanner => nearbyScanner.LocalizeAgainstAnotherScanner(this))
                .ToList()
                .ForEach(nearbyScanner => mapOfBeacons.UnionWith(nearbyScanner.AssembleMapOfBeacons()));
            return mapOfBeacons;
        }

        private bool LocalizeAgainstAnotherScanner(Scanner anotherScanner)
        {
            // Unfortunately, there's a second problem: the scanners also don't know their rotation or facing direction. Due to
            // magnetic alignment, each scanner is rotated some integer number of 90-degree turns around all of the x, y, and z
            // axes.That is, one scanner might call a direction positive x, while another scanner might call that direction
            // negative y. Or, two scanners might agree on which direction is positive x, but one scanner might be upside-down
            // from the perspective of the other scanner. In total, each scanner could be in any of 24 different orientations:
            // facing positive or negative x, y, or z, and considering any of four directions "up" from that facing.
            Parallel.ForEach(OrientationMatrices.Get(), (orientationMatrix, outerLoopState) =>
            {
                List<List<Vector>> deltasForBeacons = Beacons
                    .Select(beacon => beacon * orientationMatrix)
                    .Select(rotatedBeacon => anotherScanner.ReorientedBeacons
                        .Select(otherBeacon => otherBeacon - rotatedBeacon)
                        .ToList())
                    .ToList();

                // The scanners and beacons map a single contiguous 3d region. This region can be reconstructed by finding pairs
                // of scanners that have overlapping detection regions such that there are at least 12 beacons that both scanners
                // detect within the overlap. By establishing 12 common beacons, you can precisely determine where the scanners
                // are relative to each other, allowing you to reconstruct the beacon map one scanner at a time.
                IEnumerable<Vector> uniqueDeltas = deltasForBeacons.SelectMany(delta => delta).Distinct();
                foreach (Vector delta in uniqueDeltas)
                {
                    int numberOfListsWhichContainThisDelta = deltasForBeacons
                        .Select(list => list.Contains(delta))
                        .Count(contains => contains);
                    if (numberOfListsWhichContainThisDelta >= BeaconsToMatch)
                    {
                        ReorientedBeacons = Beacons.Select(beacon => beacon * orientationMatrix + delta).ToList();
                        Position = delta;
                        Localized = true;
                        outerLoopState.Break();
                    }
                }
            });
            return Localized;
        }

        private bool IsNearbyScanner(Scanner scanner)
        {
            int minimumSameSquaredDistancesBetweenBeacons = BeaconsToMatch * (BeaconsToMatch - 1) / 2;
            int sameSquaredDistancesBetweenBeacons = SquaredDistancesBetweenBeacons
                .Intersect(scanner.SquaredDistancesBetweenBeacons)
                .Count();
            return sameSquaredDistancesBetweenBeacons >= minimumSameSquaredDistancesBetweenBeacons;
        }

        private static Vector MakeBeacon(string input)
        {
            double[] coords = input.Split(',').Select(double.Parse).ToArray();
            return Vector.Build.DenseOfArray(coords);
        }
    }
}
