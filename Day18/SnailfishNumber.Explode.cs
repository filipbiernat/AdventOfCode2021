namespace AdventOfCode2021.Day18
{
    public partial class SnailfishNumber
    {
        private bool CheckForAPairToExplode(int nestingLevel = 0)
        {
            // If any pair is nested inside four pairs, the leftmost such pair explodes.
            if (Left != null && Right != null)
            {
                if (nestingLevel >= 4)
                {
                    Explode();
                    return true;
                }
                bool leftExploded = Left.CheckForAPairToExplode(nestingLevel + 1);
                if (leftExploded)
                {
                    return true;
                }
                return Right.CheckForAPairToExplode(nestingLevel + 1);
            }
            return false;
        }

        private void Explode()
        {
            if (Left != null && Right != null && Top != null)
            {
                // Exploding pairs will always consist of two regular numbers.
                // To explode a pair, the pair's left value is added to the first regular number to the left of the exploding
                // pair (if any).
                AddToTheFirstRegularNumberToTheLeft(Left.Value);
                // And the pair's right value is added to the first regular number to the right of the exploding pair (if any).
                AddToTheFirstRegularNumberToTheRight(Right.Value);
                // Then, the entire exploding pair is replaced with the regular number 0.
                Value = 0;
                Left = null;
                Right = null;
            }
        }

        private void AddToTheFirstRegularNumberToTheLeft(int value)
        {
            if (Top != null)
            { // Do not visit the same subtree again. Otherwise, move one level to the top.
                if (Top.Left != null && Top.Left != this)
                { // Find most right element of the subtree on the left.
                    SnailfishNumber node;
                    for (node = Top.Left; node.Right != null; node = node.Right) ;
                    node.Value += value;
                }
                else
                {
                    Top.AddToTheFirstRegularNumberToTheLeft(value);
                }
            }
        }

        private void AddToTheFirstRegularNumberToTheRight(int value)
        {
            if (Top != null)
            { // Do not visit the same subtree again. Otherwise, move one level to the top.
                if (Top.Right != null && Top.Right != this)
                { // Find most left element of the subtree on the right.
                    SnailfishNumber node;
                    for (node = Top.Right; node.Left != null; node = node.Left) ;
                    node.Value += value;
                }
                else
                {
                    Top.AddToTheFirstRegularNumberToTheRight(value);
                }
            }
        }
    }
}
