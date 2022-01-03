using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day24
{
    public class ArithmeticLogicUnit
    {
        // The ALU is a four-dimensional processing unit: it has integer variables w, x, y, and z.
        private int W, X, Y, Z;
        private int InputCount;

        public ArithmeticLogicUnit()
        {
            // These variables all start with the value 0.
            W = 0;
            X = 0;
            Y = 0;
            Z = 0;
            InputCount = 0;
        }

        public ArithmeticLogicUnit(ArithmeticLogicUnit other)
        {
            W = other.W;
            X = other.X;
            Y = other.Y;
            Z = other.Z;
            InputCount = other.InputCount;
        }

        public static void ConfigureListOfOperations(List<string> operations)
        {
            // Split list of operations to a series of lists - one list for each input number.
            foreach (string operation in operations)
            {
                if (operation.Contains("inp"))
                {
                    OperationsForEachInput.Add(new List<string>());
                }
                OperationsForEachInput.Last().Add(operation);
            }
        }

        public int RunOperationsForInputAndReturnZ(int input)
        {
            // Check if Cache already contains result for this series of operations.
            if (Cache[input].ContainsKey(this))
            { // If so, return the available result.
                Z = Cache[input][this];
            }
            else
            { // Otherwise, run all the operations and store the result in Cache.
                ArithmeticLogicUnit cacheDictionaryKey = new(this);
                OperationsForEachInput.ElementAt(InputCount).ForEach(operation => RunOperationForInput(operation, input));
                Cache[input][cacheDictionaryKey] = Z;
            }

            ++InputCount;
            return Z;
        }

        public int GetInputCount() => InputCount;

        public override int GetHashCode() =>
            W.GetHashCode() << 16 ^
            X.GetHashCode() << 8 ^
            Y.GetHashCode() << 4 ^
            Z.GetHashCode() << 2 ^
            InputCount.GetHashCode();

        public override bool Equals(object? obj) =>
            (obj != null) &&
            W == (((ArithmeticLogicUnit)obj).W) &&
            X == (((ArithmeticLogicUnit)obj).X) &&
            Y == (((ArithmeticLogicUnit)obj).Y) &&
            Z == (((ArithmeticLogicUnit)obj).Z) &&
            InputCount == (((ArithmeticLogicUnit)obj).InputCount);

        private void RunOperationForInput(string operation, int input)
        {
            string[] operationSegments = operation.Split();

            string instruction = operationSegments[0];
            ref int a = ref Variable(operationSegments[1]);
            int b = operationSegments.Length > 2 ? VariableOrNumber(operationSegments[2]) : int.MaxValue;

            // The ALU also supports six instructions:
            switch (instruction)
            {
                // - inp a - Read an input value and write it to variable a.
                case "inp": a = input; break;
                // add a b - Add the value of a to the value of b, then store the result in variable a.
                case "add": a += b; break;
                // - mul a b - Multiply the value of a by the value of b, then store the result in variable a.
                case "mul": a *= b; break;
                // - div a b - Divide the value of a by the value of b, truncate the result to an integer, then store the result
                //   in variable a. (Here, "truncate" means to round the value toward zero.)
                case "div": a /= b; break;
                // - mod a b - Divide the value of a by the value of b, then store the remainder in variable a.
                //   (This is also called the modulo operation.)
                case "mod": a %= b; break;
                // - eql a b - If the value of a and b are equal, then store the value 1 in variable a.
                //   Otherwise, store the value 0 in variable a.
                case "eql": a = (a == b ? 1 : 0); break;
                default: throw new ArgumentOutOfRangeException("Instruction name " + instruction[0] + " out of range!");
            }
        }

        private ref int Variable(string variable)
        {
            switch(variable)
            {
                case "w": return ref W;
                case "x": return ref X;
                case "y": return ref Y;
                case "z": return ref Z;
                default: throw new ArgumentOutOfRangeException("Variable name " + variable + " out of range!");
            }

        }

        private int VariableOrNumber(string variableOrNumber)
        {
            switch (variableOrNumber)
            {
                case "w": return W;
                case "x": return X;
                case "y": return Y;
                case "z": return Z;
                default: return int.Parse(variableOrNumber);
            }
        }

        private static readonly List<List<string>> OperationsForEachInput = new();

        private static readonly Dictionary<int, Dictionary<ArithmeticLogicUnit, int>> Cache = new()
        {
            { 0, new Dictionary<ArithmeticLogicUnit, int>() },
            { 1, new Dictionary<ArithmeticLogicUnit, int>() },
            { 2, new Dictionary<ArithmeticLogicUnit, int>() },
            { 3, new Dictionary<ArithmeticLogicUnit, int>() },
            { 4, new Dictionary<ArithmeticLogicUnit, int>() },
            { 5, new Dictionary<ArithmeticLogicUnit, int>() },
            { 6, new Dictionary<ArithmeticLogicUnit, int>() },
            { 7, new Dictionary<ArithmeticLogicUnit, int>() },
            { 8, new Dictionary<ArithmeticLogicUnit, int>() },
            { 9, new Dictionary<ArithmeticLogicUnit, int>() }
        };
    }
}
