namespace AdventOfCode2021.Day18
{
    public partial class SnailfishNumber
    {
        private bool CheckForARegularNumberToBeSplit()
        {
            // If any regular number is 10 or greater, the leftmost such regular number splits.
            if (Left == null || Right == null)
            {
                if (Value >= 10)
                {
                    Split();
                    return true;
                }
                return false;
            }
            bool leftSplit = Left.CheckForARegularNumberToBeSplit();
            if (leftSplit)
            {
                return true;
            }
            return Right.CheckForARegularNumberToBeSplit();
        }

        private void Split()
        {
            // To split a regular number, replace it with a pair; the left element of the pair should be the regular number
            // divided by two and rounded down, while the right element of the pair should be the regular number divided by
            // two and rounded up.
            Left = new SnailfishNumber((Value / 2).ToString(), this);
            Right = new SnailfishNumber((Value - Left.Value).ToString(), this);
            Value = int.MinValue;
        }
    }
}
