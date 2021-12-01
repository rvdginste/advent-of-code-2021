using AoCHelper;

namespace AdventOfCode
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                await Solver.SolveLast(new SolverConfiguration { ClearConsole = false });
            }
            else if (args.Length == 1 && args[0].Contains("all", StringComparison.CurrentCultureIgnoreCase))
            {
                await Solver.SolveAll(new SolverConfiguration { ShowConstructorElapsedTime = true, ShowTotalElapsedTimePerDay = true });
            }
            else
            {
                IEnumerable<uint> indexes = args.Select(arg => uint.TryParse(arg, out var index) ? index : uint.MaxValue);
                await Solver.Solve(indexes.Where(i => i < uint.MaxValue));
            }
        }
    }
}
