using AoCHelper;

using System.Globalization;
using System.Linq;

#pragma warning disable CA1707
namespace AdventOfCode
{
    public class Day_01 : BaseDay
    {
        private readonly int[] _input;

        public Day_01()
        {
            _input =
                File
                .ReadLines(InputFilePath)
                .Select(l => int.Parse(l, CultureInfo.InvariantCulture))
                .ToArray();
        }

        public override ValueTask<string> Solve_1()
        {
            (int _, int cnt) =
                _input
                .Aggregate<int, (int prev, int cnt)>(
                    (int.MaxValue, 0),
                    (acc, el) => el > acc.prev ? (el, acc.cnt + 1) : (el, acc.cnt));

            return new($"{cnt}");
        }

        public override ValueTask<string> Solve_2()
        {
            int cnt = 0;
            for (int i = 3; i < _input.Length; i++)
            {
                if (_input[i] > _input[i - 3])
                {
                    cnt++;
                }
            }

            return new($"{cnt}");
        }
    }
}
