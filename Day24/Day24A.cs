using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day24
{
    public class Day24A : IDay
    {
        public void Run()
        {
            // Once you have built a replacement ALU, you can install it in the submarine, which will immediately resume what it
            // was doing when the ALU failed: validating the submarine's model number. To do this, the ALU will run the MOdel
            // Number Automatic Detector program (MONAD, your puzzle input).
            List<string> input = File.ReadAllLines(@"..\..\..\Day24\Day24.txt").ToList();
            ArithmeticLogicUnit.ConfigureListOfOperations(input);

            // To enable as many submarine features as possible, find the largest valid fourteen-digit model number.
            string output = string.Join("", FindTheLargestNumberAcceptedByMonad(new(), new(new List<long> { 0 })));

            // What is the largest model number accepted by MONAD?
            Console.WriteLine("Solution: {0}.", output);
        }

        private static Stack<int> FindTheLargestNumberAcceptedByMonad(ArithmeticLogicUnit previousAlu, Stack<long> previousStackOfZs)
        {
            // Submarine model numbers are always fourteen-digit numbers consisting only of digits 1 through 9.
            for (int digit = 9; digit > 0; --digit)
            {
                ArithmeticLogicUnit alu = new(previousAlu);
                Stack<long> stackOfZs = new(previousStackOfZs.Reverse());

                // When MONAD checks a hypothetical fourteen-digit model number, it uses fourteen separate inp instructions, each
                // expecting a single digit of the model number in order of most to least significant.
                int z = alu.RunOperationsForInputAndReturnZ(digit);
                if (z > 2 * stackOfZs.Peek())
                {
                    // Optimization: z was significantly increased -> record the new value.
                    stackOfZs.Push(z);
                }
                else
                {
                    // Optimization: otherwise -> check if the new value of z matches the recorded value.
                    stackOfZs.Pop();
                    if (z != stackOfZs.Peek())
                    {
                        continue;
                    }
                }

                if (alu.GetInputCount() == 14)
                {
                    // Then, after MONAD has finished running all of its instructions, it will indicate that the model number was
                    // valid by leaving a 0 in variable z. However, if the model number was invalid, it will leave some other
                    // non-zero value in z.
                    Stack<int> digits = new();
                    if (z == 0)
                    {
                        digits.Push(digit);
                    }
                    return digits;
                }
                else
                {
                    Stack<int> digits = FindTheLargestNumberAcceptedByMonad(alu, stackOfZs);
                    if (digits.Any())
                    {
                        digits.Push(digit);
                        return digits;
                    }
                }
            }
            return new();
        }
    }
}
