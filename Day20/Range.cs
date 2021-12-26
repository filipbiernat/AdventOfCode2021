namespace AdventOfCode2021.Day20
{
    public class Range
    {
        public int MinRow, MaxRow, MinColumn, MaxColumn;

        public Range(HashSet<Coords> activePixels, int delta = 0)
        {
            MinRow = activePixels.Select(coords => coords.row).Min() - delta;
            MaxRow = activePixels.Select(coords => coords.row).Max() + delta;
            MinColumn = activePixels.Select(coords => coords.column).Min() - delta;
            MaxColumn = activePixels.Select(coords => coords.column).Max() + delta;
        }

        public bool IsIn(Coords coords) => MaxRow >= coords.row && coords.row >= MinRow &&
            MaxColumn >= coords.column && coords.column >= MinColumn;

        public IEnumerable<Coords> ToEnumerable() => Enumerable.Range(MinRow, MaxRow - MinRow + 1)
                .SelectMany(row => Enumerable.Range(MinColumn, MaxColumn - MinColumn + 1)
                    .Select(column => new Coords(row, column)));
    }
}
