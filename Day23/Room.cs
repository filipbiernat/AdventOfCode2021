using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day23
{
    public class Room
    {
        public List<Field> Amphipods;
        public Field TargetAmphipodType;
        public int Capacity;

        public Room(Field targetAmphipodType)
        {
            Amphipods = new();
            TargetAmphipodType = targetAmphipodType;
            Capacity = 0;
        }

        public Room(Room other)
        {
            Amphipods = new(other.Amphipods);
            TargetAmphipodType = other.TargetAmphipodType;
            Capacity = other.Capacity;
        }

        public void AddAmphipod(string text)
        {
            Amphipods.Add(TextToAmphipods[text]);
            ++Capacity;
        }

        public void MoveAmphipodToTheRoom() => Amphipods.Insert(0, TargetAmphipodType);
        public void MoveAmphipodToTheHallway() => Amphipods.RemoveAt(0);
        public bool IsFilledCorrectly() => AreAllAmphipodsOfTheTargetKind() && Amphipods.Count == Capacity;
        public bool AreAllAmphipodsOfTheTargetKind() => Amphipods.All(field => field == TargetAmphipodType);
        public int GetNumberOfEmptyFields() => Capacity - Amphipods.Count;

        public override int GetHashCode() => TargetAmphipodType.GetHashCode() << 32 ^
            Amphipods.Select((field, index) => field.GetHashCode() << index).Sum();
        public override bool Equals(object? obj) => (obj != null) &&
            Enumerable.SequenceEqual(Amphipods, ((Room)obj).Amphipods) &&
            TargetAmphipodType.Equals(((Room)obj).TargetAmphipodType);

        private static readonly Dictionary<string, Field> TextToAmphipods = new()
        {
            { "A", Field.Amber },
            { "B", Field.Bronze },
            { "C", Field.Copper },
            { "D", Field.Desert }
        };
    }
}
