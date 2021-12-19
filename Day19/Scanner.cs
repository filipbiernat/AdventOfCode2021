using MathNet.Numerics.LinearAlgebra;

namespace AdventOfCode2021.Day19
{
    using Matrix = Matrix<double>;

    public class Scanner
    {
        public List<Matrix> Beacons = new();
        public Matrix Position = Matrix.Build.Dense(1, 3);
        public bool Localized = false;

        private Matrix OrientationMatrix = Matrix.Build.DenseIdentity(3);

        private static readonly int BeaconsToMatch = 12;
        private static readonly OrientationMatrices OrientationMatrices = new();

        public Scanner(string input)
        {
            input.Split("\r\n").Skip(1).ToList().ForEach(line => Beacons.Add(MakeBeacon(line)));
        }

        public bool LocalizeAgainst(List<Matrix> otherBeacons)
        {
            // Unfortunately, there's a second problem: the scanners also don't know their rotation or facing direction. Due to
            // magnetic alignment, each scanner is rotated some integer number of 90-degree turns around all of the x, y, and z
            // axes.That is, one scanner might call a direction positive x, while another scanner might call that direction
            // negative y. Or, two scanners might agree on which direction is positive x, but one scanner might be upside-down
            // from the perspective of the other scanner. In total, each scanner could be in any of 24 different orientations:
            // facing positive or negative x, y, or z, and considering any of four directions "up" from that facing.
            Parallel.ForEach(OrientationMatrices.Get(), (orientationMatrix, outerLoopState) =>
            {
                List<List<Matrix>> deltasForBeacons = Beacons.Select(beacon => beacon * orientationMatrix)
                    .Select(rotatedBeacon => otherBeacons.Select(otherBeacon => otherBeacon - rotatedBeacon).ToList())
                    .ToList();

                // The scanners and beacons map a single contiguous 3d region. This region can be reconstructed by finding pairs
                // of scanners that have overlapping detection regions such that there are at least 12 beacons that both scanners
                // detect within the overlap. By establishing 12 common beacons, you can precisely determine where the scanners
                // are relative to each other, allowing you to reconstruct the beacon map one scanner at a time.
                IEnumerable<Matrix> uniqueDeltas = deltasForBeacons.SelectMany(delta => delta).Distinct();
                foreach (Matrix delta in uniqueDeltas)
                {
                    int numberOfListsWhichContainThisDelta = deltasForBeacons.Select(list => list.Contains(delta))
                        .Count(contains => contains);
                    if (numberOfListsWhichContainThisDelta >= BeaconsToMatch)
                    {
                        Position = delta;
                        OrientationMatrix = orientationMatrix;
                        Localized = true;
                        outerLoopState.Break();
                        break;
                    }
                }
            });
            return Localized;
        }

        public List<Matrix> GetReorientedBeacons() => Beacons.Select(beacon => beacon * OrientationMatrix + Position).ToList();

        private static Matrix MakeBeacon(string input)
        {
            double[] coords = input.Split(',').Select(double.Parse).ToArray();
            return Matrix.Build.DenseOfColumnMajor(1, 3, coords);
        }
    }
}
