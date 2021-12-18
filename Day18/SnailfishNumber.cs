namespace AdventOfCode2021.Day18
{
    public partial class SnailfishNumber
    {
        // Snailfish numbers aren't like regular numbers. Instead, every snailfish number is a pair - an ordered list of two
        // elements. Each element of the pair can be either a regular number or another pair.
        public int Value = int.MinValue;
        public SnailfishNumber? Top = null;
        public SnailfishNumber? Left = null;
        public SnailfishNumber? Right = null;

        public SnailfishNumber(string input, SnailfishNumber? top = null)
        {
            Top = top;
            if (input.Contains(','))
            {
                int commaPosition = FindCommaPosition(input);
                Left = new SnailfishNumber(input[1..commaPosition], this);
                Right = new SnailfishNumber(input[(commaPosition + 1)..(input.Length - 1)], this);
            }
            else
            {
                Value = int.Parse(input);
            }
        }

        public SnailfishNumber(SnailfishNumber left, SnailfishNumber right)
        {
            Top = null;
            Left = left;
            Right = right;
            Left.Top = this;
            Right.Top = this;
        }

        public int GetMagnitude()
        {
            // The magnitude of a pair is 3 times the magnitude of its left element plus 2 times the magnitude of its right
            // element. The magnitude of a regular number is just that number.
            return (Left != null && Right != null) ? (3 * Left.GetMagnitude() + 2 * Right.GetMagnitude()) : Value;
        }

        public static SnailfishNumber operator +(SnailfishNumber left, SnailfishNumber right)
        {
            // To add two snailfish numbers, form a pair from the left and right parameters of the addition operator.
            return new SnailfishNumber(left, right).Reduce();
        }

        private SnailfishNumber Reduce()
        {
            // During reduction, at most one action applies, after which the process returns to the top of the list of actions.
            bool actionApplied;
            // To reduce a snailfish number, you must repeatedly do the first action in this list that applies to the
            // snailfish number:
            do
            {
                // - If any pair is nested inside four pairs, the leftmost such pair explodes.
                actionApplied = CheckForAPairToExplode();
                if (!actionApplied)
                {
                    // - If any regular number is 10 or greater, the leftmost such regular number splits.
                    actionApplied = CheckForARegularNumberToBeSplit();
                }
            }
            while (actionApplied);
            // Once no action in the above list applies, the snailfish number is reduced.
            return this;
        }

        private static int FindCommaPosition(string input)
        {
            int commaPosition = 0;
            for (int nestingLevel = 0; commaPosition < input.Length; ++commaPosition)
            {
                char c = input[commaPosition];
                if (c == '[')
                {
                    ++nestingLevel;
                }
                else if (c == ']')
                {
                    --nestingLevel;
                }
                if (c == ',' && nestingLevel == 1)
                {
                    break;
                }
            }
            return commaPosition;
        }
    }
}
