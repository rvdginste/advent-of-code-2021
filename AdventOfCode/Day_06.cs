using System.Globalization;
using AoCHelper;

namespace AdventOfCode;

public class Day_06 : BaseDay

{
    private readonly int[] _numbers;

    public Day_06()
    {
        _numbers =
            File
                .ReadAllLines(InputFilePath)
                .Single()
                .Split(',')
                .Select(s => int.Parse(s, CultureInfo.InvariantCulture))
                .ToArray();
    }

    public override ValueTask<string> Solve_1()
    {
        IDictionary<int, long> state =
            Enumerable
                .Range(0, 9)
                .ToDictionary(
                    n => n,
                    n => (long)_numbers.Count(x => x == n));

        void AdvanceDay()
        {
            IDictionary<int, long> delta = Enumerable.Range(0, 9).ToDictionary(n => n, n => (long)0);
            foreach (int key in state.Keys)
            {
                switch (key)
                {
                    case 0:
                        delta[0] -= state[0];
                        delta[8] += state[0];
                        delta[6] += state[0];
                        break;
                    default:
                        delta[key] -= state[key];
                        delta[key - 1] += state[key];
                        break;
                }
            }

            foreach (int key in state.Keys)
            {
                state[key] += delta[key];
            }
        }

        for (int i = 0; i < 80; i++)
        {
            AdvanceDay();
        }

        long nr = state.Values.Sum();

        return new($"Number of fish: {nr}");
    }

    public override ValueTask<string> Solve_2()
    {
        IDictionary<int, long> state =
            Enumerable
                .Range(0, 9)
                .ToDictionary(
                    n => n,
                    n => (long)_numbers.Count(x => x == n));

        void AdvanceDay()
        {
            IDictionary<int, long> delta = Enumerable.Range(0, 9).ToDictionary(n => n, n => (long)0);
            foreach (int key in state.Keys)
            {
                switch (key)
                {
                    case 0:
                        delta[0] -= state[0];
                        delta[8] += state[0];
                        delta[6] += state[0];
                        break;
                    default:
                        delta[key] -= state[key];
                        delta[key - 1] += state[key];
                        break;
                }
            }

            foreach (int key in state.Keys)
            {
                state[key] += delta[key];
            }
        }

        for (int i = 0; i < 256; i++)
        {
            AdvanceDay();
        }

        long nr = state.Values.Sum();

        return new($"Number of fish: {nr}");
    }
}