using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day24
{
    public class Day24B : IDay
    {
        public void Run()
        {
            // Once you have built a replacement ALU, you can install it in the submarine, which will immediately resume what it
            // was doing when the ALU failed: validating the submarine's model number. To do this, the ALU will run the MOdel
            // Number Automatic Detector program (MONAD, your puzzle input).
            List<string> input = File.ReadAllLines(@"..\..\..\Day24\Day24.txt").ToList();
            ArithmeticLogicUnit.ConfigureListOfOperations(input);

            // As the submarine starts booting up things like the Retro Encabulator, you realize that maybe you don't need all
            // these submarine features after all.
            string output = string.Join("", FindTheSmallestNumberAcceptedByMonad(new(), new(new List<long> { 0 })));

            // What is the smallest model number accepted by MONAD?
            Console.WriteLine("Solution: {0}.", output);
        }

        private static Stack<int> FindTheSmallestNumberAcceptedByMonad(ArithmeticLogicUnit previousAlu, Stack<long> previousStackOfZs)
        {
            // Submarine model numbers are always fourteen-digit numbers consisting only of digits 1 through 9.
            for (int digit = 1; digit < 10; ++digit)
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
                    Stack<int> digits = FindTheSmallestNumberAcceptedByMonad(alu, stackOfZs);
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
