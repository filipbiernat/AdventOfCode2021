namespace AdventOfCode2021.Day20
{
    public class Coords
    {
        public int row;
        public int column;

        public Coords(int row, int column)
        {
            this.row = row;
            this.column = column;
        }

        public static Coords operator +(Coords lhs, Coords rhs) => new(lhs.row + rhs.row, lhs.column + rhs.column);
        public override int GetHashCode() => (row.GetHashCode() << 2) ^ column.GetHashCode();
        public override bool Equals(object? obj) =>
            (obj != null) &&
            (row == ((Coords)obj).row) &&
            (column == ((Coords)obj).column);
    }
}
