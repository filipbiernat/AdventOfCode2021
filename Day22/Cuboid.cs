using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day22
{

    public class Cuboid
    {
        public readonly bool on;
        private readonly long x0, x1, y0, y1, z0, z1;

        public Cuboid(bool on, long x0, long x1, long y0, long y1, long z0, long z1)
        {
            this.on = on;
            this.x0 = x0;
            this.x1 = x1;
            this.y0 = y0;
            this.y1 = y1;
            this.z0 = z0;
            this.z1 = z1;
        }

        // The reactor core is made up of a large 3-dimensional grid made up entirely of cubes, one cube per integer
        // 3-dimensional coordinate (x,y,z).
        public long GetCubesCount() => (on ? 1 : -1) * (x1 - x0 + 1) * (y1 - y0 + 1) * (z1 - z0 + 1);

        // Each cube can be either on or off.
        public bool IsOn() => on;

        // The initialization procedure only uses cubes that have x, y, and z positions of at least -50 and at most 50.
        public bool IsValidForInitializationProcedure() =>
            new List<long> { x0, y0, z0 }.All(dimmension => dimmension >= -50) &&
            new List<long> { x1, y1, z1 }.All(dimmension => dimmension <= 50);

        public bool AreCuboidsOverlapping(Cuboid other) =>
            x0 <= other.x1 && x1 >= other.x0 &&
            y0 <= other.y1 && y1 >= other.y0 &&
            z0 <= other.z1 && z1 >= other.z0;

        public Cuboid CalculateOverlappingCuboid(Cuboid other) =>
            new(!other.on,
                Math.Max(x0, other.x0), Math.Min(x1, other.x1),
                Math.Max(y0, other.y0), Math.Min(y1, other.y1),
                Math.Max(z0, other.z0), Math.Min(z1, other.z1));
    }
}
