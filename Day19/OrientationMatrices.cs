using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace AdventOfCode2021.Day19
{
    using Matrix = Matrix<double>;

    public class OrientationMatrices
    {
        private readonly List<Matrix> Matrices;

        public OrientationMatrices()
        {
            Matrix matrixOfRotationOverX = Matrix.Build.DenseOfArray(RotationOverX);
            Matrix matrixOfRotationOverY = Matrix.Build.DenseOfArray(RotationOverY);
            Matrix matrixOfRotationOverZ = Matrix.Build.DenseOfArray(RotationOverZ);

            HashSet<Matrix> set = new();
            Matrix matrixRotatingOverX = Matrix.Build.DenseIdentity(3);
            foreach (int rotationCountOverX in Enumerable.Range(0, 4))
            {
                matrixRotatingOverX *= matrixOfRotationOverX;
                Matrix matrixRotatingOverY = matrixRotatingOverX;
                foreach (int rotationCountOverY in Enumerable.Range(0, 4))
                {
                    matrixRotatingOverY *= matrixOfRotationOverY;
                    Matrix matrixRotatingOverZ = matrixRotatingOverY;
                    foreach (int rotationCountOverZ in Enumerable.Range(0, 4))
                    {
                        matrixRotatingOverZ *= matrixOfRotationOverZ;
                        set.Add(matrixRotatingOverZ);
                    }
                }
            }
            Matrices = set.ToList();
        }

        public List<Matrix> Get() => Matrices;

        private static readonly double[,] RotationOverX = { { 1, 0, 0 }, { 0, 0, -1 }, { 0, 1, 0 } };
        private static readonly double[,] RotationOverY = { { 0, 0, 1 }, { 0, 1, 0 }, { -1, 0, 0 } };
        private static readonly double[,] RotationOverZ = { { 0, -1, 0 }, { 1, 0, 0 }, { 0, 0, 1 } };
    }
}
