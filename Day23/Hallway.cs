using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day23
{
    public class Hallway
    {
        // Amphipods will never stop on the space immediately outside any room.
        // They can move into that space so long as they immediately continue moving.
        // (Specifically, this refers to the four open spaces in the hallway that are directly above an amphipod starting position.)
        public List<Field> Fields;

        // In our representation of Hallway we skip forbidden fields. There are only seven valid fields.
        // Hallway fields:  0 | 1 | X | 2 | X | 3 | X | 4 | X | 5 | 6
        // Rooms:                   A       B       C       D
        public Hallway() => Fields = new()
        {
            Field.Empty,
            Field.Empty,
            Field.Empty,
            Field.Empty,
            Field.Empty,
            Field.Empty,
            Field.Empty
        };

        public Hallway(Hallway other) => Fields = new(other.Fields);

        public void MoveAmphipodToTheRoom(int hallwayPosition) => Fields[hallwayPosition] = Field.Empty;
        public void MoveAmphipodToTheHallway(int hallwayPosition, Field amphipodType) => Fields[hallwayPosition] = amphipodType;
        public bool IsHallwayPathClear(int from, int to) => Fields.GetRange(from, to - from).All(field => field == Field.Empty);

        public override int GetHashCode() => Fields.Select((field, index) => field.GetHashCode() << index).Sum();
        public override bool Equals(object? obj) => (obj != null) && Enumerable.SequenceEqual(Fields, ((Hallway)obj).Fields);
    }
}
