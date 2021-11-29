namespace AdventOfCode2021
{
    class AdventOfCode2021
    {
        public static void Main()
        {
            Console.WriteLine("Advent Of Code 2021");
            Execute(new Day0.Day0(), new Day0.Day0());
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
