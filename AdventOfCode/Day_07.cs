using System.Globalization;
using AoCHelper;

namespace AdventOfCode;

public class Day_07 : BaseDay

{
    private readonly long[] _numbers;
    private readonly IDictionary<long, long> _positions;

    public Day_07()
    {
        _numbers =
            File
                .ReadAllLines(InputFilePath)
                .Single()
                .Split(',')
                .Select(s => long.Parse(s, CultureInfo.InvariantCulture))
                .ToArray();

        _positions =
            _numbers
                .Distinct()
                .ToDictionary(
                    n => n,
                    n => _numbers.LongCount(x => x == n));
    }

    public override ValueTask<string> Solve_1()
    {
        long FuelCost(long x)
            => _positions
                .Keys
                .Aggregate<long, long>(
                    0,
                    (acc, el) =>
                        el < x
                            ? acc + _positions[el] * (x - el)
                            : acc + _positions[el] * (el - x));

        (long x, long cost) best = (x: _numbers.Min(), cost: FuelCost(_numbers.Min()));
        for (long i = _numbers.Min(); i < _numbers.Max(); i++)
        {
            long ncost = FuelCost(i);
            if (ncost < best.cost)
            {
                best = (x: i, cost: ncost);
            }
        }

        return new($"Best x: {best.x}, Fuel cost: {best.cost}");
    }

    public override ValueTask<string> Solve_2()
    {
        IDictionary<long, long> costCache = new Dictionary<long, long>();

        long MoveCost(long delta)
        {
            delta = delta < 0 ? -delta : delta;
            return costCache.ContainsKey(delta)
                ? costCache[delta]
                : (delta == 0 ? 0 : costCache[delta] = MoveCost(delta - 1) + delta);
        }

        long FuelCost(long x)
            => _positions
                .Keys
                .Aggregate<long, long>(0, (acc, el) => acc + _positions[el] * MoveCost(x - el));

        (long x, long cost) best = (x: _numbers.Min(), cost: FuelCost(_numbers.Min()));
        for (long i = _numbers.Min(); i < _numbers.Max(); i++)
        {
            long ncost = FuelCost(i);
            if (ncost < best.cost)
            {
                best = (x: i, cost: ncost);
            }
        }

        return new($"Best x: {best.x}, Fuel cost: {best.cost}");
    }
}