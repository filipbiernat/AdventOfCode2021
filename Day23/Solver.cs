using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day23
{
    public static class Solver
    {
        // The amphipods would like you to find a way to organize the amphipods that requires the least total energy.
        public static int Solve(Burrow burrow)
        {
            // Use cache dictionary for optimization.
            if (SolverCache.ContainsKey(burrow))
            {
                return SolverCache[burrow];
            }

            // Check if the algorithm is done.
            if (burrow.IsFilledCorrectly())
            {
                // Add the result to the cache dictionary.
                SolverCache[burrow] = 0;
                return 0;
            }

            // Calculate cost for the next moves.
            int minimumCost = int.MaxValue;
            foreach (Move move in FindMoves(burrow))
            {
                // Make sure that the new cost does not exceed maximum size of type int.
                long newCost = (long)move.Cost + Solve(move.BurrowAfterMove);
                move.Cost = (int)Math.Min(newCost, int.MaxValue);
                // Find minimum cost of the move.
                minimumCost = Math.Min(move.Cost, minimumCost);
            }

            // Add the result to the cache dictionary.
            SolverCache[burrow] = minimumCost;
            return minimumCost;
        }

        public static List<Move> FindMoves(Burrow burrow)
        {
            // First, try to find the moves from hallway to a room.
            List<Move> movesFromHallwayToRoom = FindMovesFromHallwayToRoom(burrow);
            if (movesFromHallwayToRoom.Any())
            {
                return movesFromHallwayToRoom;
            }
            // If there is no move from the hallway to a room, explore the opposite direction.
            return FindMovesFromRoomToHallway(burrow);
        }

        // Amphipods will never move from the hallway into a room unless that room is their destination room and that room contains
        // no amphipods which do not also have that room as their own destination. If an amphipod's starting room is not its
        // destination room, it can stay in that room until it leaves the room. (For example, an Amber amphipod will not move from
        // the hallway into the right three rooms, and will only move into the leftmost room if that room is empty or if it only
        // contains other Amber amphipods.)
        private static List<Move> FindMovesFromHallwayToRoom(Burrow burrow)
        {
            List<Move> moves = new();

            int hallwayPosition = -1;
            // Find moves from different hallway positions.
            foreach (Field amphipodToMove in burrow.Hallway.Fields)
            {
                ++hallwayPosition;

                // Skip empty hallway fields.
                if (amphipodToMove == Field.Empty)
                {
                    continue;
                }

                // Each amphipod can move only to the correct room for its type.
                // Find room for this amphipod's type. Skip if some other amphipods inside.
                Room room = burrow.FindRoomByAmphipodType(amphipodToMove);
                if (!room.AreAllAmphipodsOfTheTargetKind())
                {
                    continue;
                }

                // Calculate cost of the move.
                int roomPosition = burrow.FindRoomPositionByAmphipodType(amphipodToMove);
                int cost = CalculateCostOfTheMove(amphipodToMove, burrow, roomPosition, hallwayPosition, fromHallwayToRoom: true);

                // Skip if the move costs infinity.
                if (cost == int.MaxValue)
                {
                    continue;
                }

                // Make move.
                Burrow burrowAfterMove = new(burrow);
                burrowAfterMove.MakeMoveFromHallwayToRoom(hallwayPosition, amphipodToMove);
                moves.Add(new Move(cost, burrowAfterMove));
            }
            return moves;
        }

        // Once an amphipod stops moving in the hallway, it will stay in that spot until it can move into a room. (That is, once
        // any amphipod starts moving, any other amphipods currently in the hallway are locked in place and will not move again
        // until they can move fully into a room.)
        private static List<Move> FindMovesFromRoomToHallway(Burrow burrow)
        {
            List<Move> moves = new();

            // Find moves from different rooms.
            int roomPosition = -1;
            foreach (Room room in burrow.Rooms)
            {
                ++roomPosition;

                // Skip if all the right amphipods inside.
                if (room.AreAllAmphipodsOfTheTargetKind())
                {
                    continue;
                }

                // Check the type of the first amphipod in the room.
                Field amphipodToMove = room.Amphipods.First();

                // Find moves to different hallway positions.
                for (int hallwayPosition = 0; hallwayPosition < burrow.Hallway.Fields.Count; ++hallwayPosition)
                {
                    // Skip occupied hallway fields.
                    if (burrow.Hallway.Fields[hallwayPosition] != Field.Empty)
                    {
                        continue;
                    }

                    // Calculate cost of the move.
                    int cost = CalculateCostOfTheMove(amphipodToMove, burrow, roomPosition, hallwayPosition, fromHallwayToRoom: false);

                    // Skip if the move costs infinity.
                    if (cost == int.MaxValue)
                    {
                        continue;
                    }

                    // Make move.
                    Burrow burrowAfterMove = new(burrow);
                    burrowAfterMove.MakeMoveFromRoomToHallway(amphipodToMove, hallwayPosition, roomPosition);
                    moves.Add(new Move(cost, burrowAfterMove));
                }
            }
            return moves;
        }

        public static int CalculateCostOfTheMove(Field amphipodToMove,
            Burrow burrow, int roomPosition, int hallwayPosition, bool fromHallwayToRoom)
        {
            // Hallway path depends on whether the room is to the left or to the right of the hallway position.
            // The target hallway position itself is not included here in this range.
            // There are 2 fields (0 and 1) between room position 0 and hallway position 0.
            // Hallway fields:  0 | 1 | X | 2 | X | 3 | X | 4 | X | 5 | 6
            // Rooms:                   A       B       C       D
            int from = Math.Min(roomPosition + 2, hallwayPosition + 1);
            int to = Math.Max(hallwayPosition, roomPosition + 2);

            // If hallway path is not clear, set cost as infinity.
            if (!burrow.Hallway.IsHallwayPathClear(from, to))
            {
                return int.MaxValue;
            }

            int steps = RoomToHallwayDistance[roomPosition][hallwayPosition] +
                burrow.Rooms[roomPosition].GetNumberOfEmptyFields() +
                (fromHallwayToRoom ? 1 : 0);
            return steps * EnergyPerStep[amphipodToMove];
        }

        private static readonly Dictionary<Burrow, int> SolverCache = new();

        private static readonly List<List<int>> RoomToHallwayDistance = new()
        {
            // In our hallway representation we skip forbidden fields. There are only seven valid fields.
            // Hallway fields:  0 | 1 | X | 2 | X | 3 | X | 4 | X | 5 | 6
            // Rooms:                   A       B       C       D
            new List<int>() { 2, 1, 1, 3, 5, 7, 8 }, // Distance to room 0 from different hallway positions.
            new List<int>() { 4, 3, 1, 1, 3, 5, 6 }, // Distance to room 1 from different hallway positions.
            new List<int>() { 6, 5, 3, 1, 1, 3, 4 }, // Distance to room 2 from different hallway positions.
            new List<int>() { 8, 7, 5, 3, 1, 1, 2 }  // Distance to room 3 from different hallway positions.
        };

        private static readonly Dictionary<Field, int> EnergyPerStep = new()
        {
            // Amber amphipods require 1 energy per step, Bronze amphipods require 10 energy, Copper amphipods require 100, and
            // Desert ones require 1000.
            { Field.Amber, 1 },
            { Field.Bronze, 10 },
            { Field.Copper, 100 },
            { Field.Desert, 1000 },
        };
    }
}
