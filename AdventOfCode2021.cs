﻿namespace AdventOfCode2021
{
    class AdventOfCode2021
    {
        public static void Main()
        {
            Console.WriteLine("Advent Of Code 2021");
            Execute(new Day1.Day1A(), new Day1.Day1B());
            Execute(new Day2.Day2A(), new Day2.Day2B());
            Execute(new Day3.Day3A(), new Day3.Day3B());
            Execute(new Day4.Day4A(), new Day4.Day4B());
            Execute(new Day5.Day5A(), new Day5.Day5B());
            Execute(new Day6.Day6A(), new Day6.Day6B());
            Execute(new Day7.Day7A(), new Day7.Day7B());
            Execute(new Day8.Day8A(), new Day8.Day8B());
            Execute(new Day9.Day9A(), new Day9.Day9B());
            Execute(new Day10.Day10A(), new Day10.Day10B());
            Execute(new Day11.Day11A(), new Day11.Day11B());
            Execute(new Day12.Day12A(), new Day12.Day12B());
            Execute(new Day13.Day13A(), new Day13.Day13B());
            Execute(new Day14.Day14A(), new Day14.Day14B());
            Execute(new Day15.Day15A(), new Day15.Day15B());
            Execute(new Day16.Day16A(), new Day16.Day16B());
            Execute(new Day17.Day17A(), new Day17.Day17B());
            Execute(new Day18.Day18A(), new Day18.Day18B());
            Execute(new Day19.Day19A(), new Day19.Day19B());
            Execute(new Day20.Day20A(), new Day20.Day20B());
            Execute(new Day21.Day21A(), new Day21.Day21B());
            Execute(new Day22.Day22A(), new Day22.Day22B());
            Execute(new Day23.Day23A(), new Day23.Day23B());
            Execute(new Day24.Day24A(), new Day24.Day24B());
            Execute(new Day25.Day25A());
        }

        private static void Execute(params IDay[] days)
        {
            foreach (IDay day in days)
            {
                Execute(day);
            }
        }

        private static void Execute(IDay day)
        {
            Console.WriteLine();
            Console.WriteLine("Running {0}:", day.GetType().Name);

            System.Diagnostics.Stopwatch stopWatch = new();
            stopWatch.Start();
            day.Run();
            stopWatch.Stop();

            List<string> runTime = new();
            AppendTime(runTime, stopWatch.Elapsed.Hours, "h");
            AppendTime(runTime, stopWatch.Elapsed.Minutes, "min");
            AppendTime(runTime, stopWatch.Elapsed.Seconds, "sec");
            AppendTime(runTime, stopWatch.Elapsed.Milliseconds, "msec");
            Console.WriteLine("Run time: {0}.", string.Join(", ", runTime));
        }

        private static void AppendTime(List<string> list, int time, string unit)
        {
            if (time > 0)
            {
                list.Add(string.Format($"{time} {unit}"));
            }
        }
    }
}
