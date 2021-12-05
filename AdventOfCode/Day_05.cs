using AoCHelper;

using System.Globalization;

#pragma warning disable CA1707
#pragma warning disable IDE0061
namespace AdventOfCode
{
    public class Day_05 : BaseDay
    {
        private readonly (int x1, int y1, int x2, int y2)[] _lines;

        public Day_05()
        {
            _lines =
                File
                    .ReadAllLines(InputFilePath)
                    .Select(l =>
                    {
                        int[] numbers =
                            l.Split(" -> ")
                                .SelectMany(c => c.Split(','))
                                .Select(x => int.Parse(x, CultureInfo.InvariantCulture))
                                .ToArray();
                        return (x1: numbers[0], y1: numbers[1], x2: numbers[2], y2: numbers[3]);
                    })
                    .ToArray();
        }

        public override ValueTask<string> Solve_1()
        {
            IDictionary<(int x, int y), int> hits = new Dictionary<(int x, int y), int>();

            void RegisterHit(int x, int y)
            {
                if (hits.ContainsKey((x, y)))
                {
                    hits[(x, y)] += 1;
                }
                else
                {
                    hits[(x, y)] = 1;
                }

            }

            foreach ((int x1, int y1, int x2, int y2) in _lines)
            {
                if (x1 == x2) // horizontal
                {
                    int step = y1 < y2 ? 1 : -1;
                    int current = y1 - step;
                    do
                    {
                        current += step;
                        RegisterHit(x1, current);
                    }
                    while (current != y2);
                }
                else if (y1 == y2) // vertical
                {
                    int step = x1 < x2 ? 1 : -1;
                    int current = x1 - step;
                    do
                    {
                        current += step;
                        RegisterHit(current, y1);
                    }
                    while (current != x2);
                }
            }


            int result = hits.Values.Count(v => v > 1);

            return new($"Nr points: {result}");
        }

        public override ValueTask<string> Solve_2()
        {
            IDictionary<(int x, int y), int> hits = new Dictionary<(int x, int y), int>();

            void RegisterHit(int x, int y)
            {
                if (hits.ContainsKey((x, y)))
                {
                    hits[(x, y)] += 1;
                }
                else
                {
                    hits[(x, y)] = 1;
                }

            }

            foreach ((int x1, int y1, int x2, int y2) in _lines)
            {
                if (x1 == x2) // horizontal
                {
                    int step = y1 < y2 ? 1 : -1;
                    int current = y1 - step;
                    do
                    {
                        current += step;
                        RegisterHit(x1, current);
                    }
                    while (current != y2);
                }
                else if (y1 == y2) // vertical
                {
                    int step = x1 < x2 ? 1 : -1;
                    int current = x1 - step;
                    do
                    {
                        current += step;
                        RegisterHit(current, y1);
                    }
                    while (current != x2);
                }
                else // must be diagonal
                {
                    int stepX = x1 < x2 ? 1 : -1;
                    int stepY = y1 < y2 ? 1 : -1;
                    int currentX = x1 - stepX;
                    int currentY = y1 - stepY;
                    do
                    {
                        currentX += stepX;
                        currentY += stepY;
                        RegisterHit(currentX, currentY);
                    }
                    while (currentX != x2);
                }
            }


            int result = hits.Values.Count(v => v > 1);

            return new($"Nr points: {result}");
        }
    }
}
