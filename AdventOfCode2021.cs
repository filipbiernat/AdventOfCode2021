namespace AdventOfCode2021
{
    class AdventOfCode2021
    {
        public static void Main()
        {
            Console.WriteLine("Advent Of Code 2021");
            //Execute(new Day1.Day1A(), new Day1.Day1B());
            //Execute(new Day2.Day2A(), new Day2.Day2B());
            //Execute(new Day3.Day3A(), new Day3.Day3B());
            //Execute(new Day4.Day4A(), new Day4.Day4B());
            //Execute(new Day5.Day5A(), new Day5.Day5B());
            Execute(new Day6.Day6A(), new Day6.Day6B());
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
            Console.WriteLine("\r\nRunning {0}:\r\n", day.GetType().Name);
            day.Run();
        }
    }
}
