using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day23
{
    // A group of amphipods notice your fancy submarine and flag you down. "With such an impressive shell," one amphipod says,
    // "surely you can help us with a question that has stumped our best scientists."
    // They go on to explain that a group of timid, stubborn amphipods live in a nearby burrow.
    public class Burrow
    {
        public Hallway Hallway;
        public List<Room> Rooms;

        public Burrow(List<string> input)
        {
            // They live in a burrow that consists of a hallway and four side rooms.
            // The side rooms are initially full of amphipods, and the hallway is initially empty.
            Hallway = new();

            // The amphipods would like a method to organize every amphipod into side rooms so that each side room contains one
            // type of amphipod and the types are sorted A-D going left to right, like this:
            // #############
            // #...........#
            // ###A#B#C#D###
            //   #A#B#C#D#
            //   #########
            Rooms = new()
            {
                new Room(Field.Amber),
                new Room(Field.Bronze),
                new Room(Field.Copper),
                new Room(Field.Desert)
            };
            input.Skip(2).SkipLast(1).ToList().ForEach(FillRoomsWithInputLine);
        }

        public Burrow(Burrow other)
        {
            Hallway = new(other.Hallway);
            Rooms = other.Rooms.Select(item => new Room(item)).ToList();
        }

        public Burrow MakeMoveFromHallwayToRoom(int hallwayPosition, Field amphipodType)
        {
            Hallway.MoveAmphipodToTheRoom(hallwayPosition);
            FindRoomByAmphipodType(amphipodType).MoveAmphipodToTheRoom();
            return this;
        }

        public Burrow MakeMoveFromRoomToHallway(Field amphipodType, int hallwayPosition, int roomPosition)
        {
            Rooms.ElementAt(roomPosition).MoveAmphipodToTheHallway();
            Hallway.MoveAmphipodToTheHallway(hallwayPosition, amphipodType);
            return this;
        }

        public bool IsFilledCorrectly() => Rooms.All(room => room.IsFilledCorrectly());
        public Room FindRoomByAmphipodType(Field amphipodType) =>
            Rooms.Where(room => room.TargetAmphipodType == amphipodType).First();
        public int FindRoomPositionByAmphipodType(Field amphipodType) => Rooms.IndexOf(FindRoomByAmphipodType(amphipodType));

        public override int GetHashCode() => Hallway.GetHashCode() << 32 ^
            Rooms.Select((room, index) => room.GetHashCode() << index).Sum();
        public override bool Equals(object? obj) => (obj != null) &&
                Hallway.Equals(((Burrow)obj).Hallway) &&
                Enumerable.SequenceEqual(Rooms, ((Burrow)obj).Rooms);

        private void FillRoomsWithInputLine(string inputLine)
        {
            // They give you a diagram of the situation (your puzzle input), including locations of each amphipod
            // (A, B, C, or D, each of which is occupying an otherwise open space), walls (#), and open space (.).
            // For example:
            // #############
            // #...........#
            // ###B#C#B#D###
            //   #A#D#C#A#
            //   #########
            string[] amphipods = inputLine.Split(new string[] { "#", " " }, StringSplitOptions.RemoveEmptyEntries);
            for (int room = 0; room < Rooms.Count; ++room)
            {
                Rooms[room].AddAmphipod(amphipods[room]);
            }
        }
    }

    // Four types of amphipods live there: Amber (A), Bronze (B), Copper (C), and Desert (D).
    public enum Field
    {
        Amber,
        Bronze,
        Copper,
        Desert,
        Empty
    }

}
