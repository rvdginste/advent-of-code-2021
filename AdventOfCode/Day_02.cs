using AoCHelper;

using System.Globalization;

#pragma warning disable CA1707
namespace AdventOfCode
{
    public class Day_02 : BaseDay
    {
        private readonly (string command, int val)[] _input;

        public Day_02()
        {
            _input =
                File
                .ReadLines(InputFilePath)
                .Select(
                    l => {
                        string[] pair = l.Split();
                        return (command: pair[0], val: int.Parse(pair[1], CultureInfo.InvariantCulture));
                    })
                .ToArray();
        }

        public override ValueTask<string> Solve_1()
        {
            (int pos, int depth) =
                _input
                .Aggregate<(string command, int val), (int pos, int depth)>(
                    (0, 0),
                    (acc, el) => el.command switch
                    {
                        "forward" => (pos: acc.pos + el.val, acc.depth),
                        "down" => (acc.pos, depth: acc.depth + el.val),
                        "up" => (acc.pos, depth: acc.depth - el.val),
                        _ => throw new ArgumentOutOfRangeException(nameof(el.command), $"Not a valid command: {el.command}")
                    });

            return new($"Position {pos}, Depth {depth} => {pos * depth}");
        }

        public override ValueTask<string> Solve_2()
        {
            (int pos, int depth, int aim) =
                _input
                .Aggregate<(string command, int val), (int pos, int depth, int aim)>(
                    (0, 0, 0),
                    (acc, el) => el.command switch
                    {
                        "forward" => (pos: acc.pos + el.val, depth: acc.depth + (acc.aim * el.val), acc.aim),
                        "down" => (acc.pos, acc.depth, aim: acc.aim + el.val),
                        "up" => (acc.pos, acc.depth, aim: acc.aim - el.val),
                        _ => throw new ArgumentOutOfRangeException(nameof(el.command), $"Not a valid command: {el.command}")
                    });

            return new($"Position {pos}, Depth {depth} => {pos * depth}");
        }
    }
}
