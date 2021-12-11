using System.Globalization;
using AoCHelper;

#pragma warning disable IDE0011
#pragma warning disable IDE0055

namespace AdventOfCode
{
    public class Day_11 : BaseDay
    {
        private readonly int _dimX;
        private readonly int _dimY;
        private readonly int[,] _input;

        public Day_11()
        {
            string[] lines = File.ReadAllLines(InputFilePath);
            _dimX = lines[0].Length + 2;
            _dimY = lines.Length + 2;
            _input = new int[_dimX, _dimY];
            for (int i = 0; i < _dimX; i++)
            for (int j = 0; j < _dimY; j++)
                _input[i, j] =
                    i == 0 || j == 0 || i == (_dimX - 1) || j == (_dimY - 1)
                        ? 9
                        : int.Parse(lines[j - 1][(i - 1)..i], CultureInfo.InvariantCulture);
        }

        public override ValueTask<string> Solve_1()
        {
            int[,] workarea = (int[,])_input.Clone();

            static IEnumerable<(int x, int y)> FindNeighbours((int x, int y) point)
            {
                (int x, int y) = point;
                yield return (x: x - 1, y: y - 1);
                yield return (x: x - 1, y );
                yield return (x: x - 1, y: y + 1);
                yield return (x, y: y - 1);
                yield return (x, y: y + 1);
                yield return (x: x + 1, y: y - 1);
                yield return (x: x + 1, y);
                yield return (x: x + 1, y: y + 1);
            }

            void BumpEnergy(int x, int y, Stack<(int x, int y)> stack)
            {
                workarea[x, y] += 1;
                if (workarea[x, y] == 10)
                {
                    stack.Push((x, y));
                }
            }

            int ProcessOneStep()
            {
                int flashes = 0;
                Stack<(int x, int y)> flashers = new();
                HashSet<(int x, int y)> toReset = new();

                // for-each increase 1 + keep track of the flashes
                for (int i = 1; i < _dimX - 1; i++)
                    for (int j = 1; j < _dimY - 1; j++)
                        BumpEnergy(i, j, flashers);

                // check flashers
                while (flashers.TryPop(out (int x, int y) point))
                {
                    (int x, int y) = point;
                    if ((x == 0) || (y == 0) || (x == _dimX - 1) || (y == _dimY - 1)) continue;
                    flashes++;
                    toReset.Add(point);
                    foreach((int x, int y) p in FindNeighbours(point))
                        BumpEnergy(p.x, p.y, flashers);
                }

                // reset flashers to zero
                foreach ((int x, int y) in toReset)
                    workarea[x, y] = 0;

                return flashes;
            }

            int steps = 0;
            int sum = 0;
            while(steps++ < 100)
            {
                sum += ProcessOneStep();
            }

            return new($"Flashes: {sum}");
        }

        public override ValueTask<string> Solve_2()
        {
            int[,] workarea = (int[,])_input.Clone();

            static IEnumerable<(int x, int y)> FindNeighbours((int x, int y) point)
            {
                (int x, int y) = point;
                yield return (x: x - 1, y: y - 1);
                yield return (x: x - 1, y );
                yield return (x: x - 1, y: y + 1);
                yield return (x, y: y - 1);
                yield return (x, y: y + 1);
                yield return (x: x + 1, y: y - 1);
                yield return (x: x + 1, y);
                yield return (x: x + 1, y: y + 1);
            }

            void BumpEnergy(int x, int y, Stack<(int x, int y)> stack)
            {
                workarea[x, y] += 1;
                if (workarea[x, y] == 10)
                {
                    stack.Push((x, y));
                }
            }

            int ProcessOneStep()
            {
                int flashes = 0;
                Stack<(int x, int y)> flashers = new();
                HashSet<(int x, int y)> toReset = new();

                // for-each increase 1 + keep track of the flashes
                for (int i = 1; i < _dimX - 1; i++)
                    for (int j = 1; j < _dimY - 1; j++)
                        BumpEnergy(i, j, flashers);

                // check flashers
                while (flashers.TryPop(out (int x, int y) point))
                {
                    (int x, int y) = point;
                    if ((x == 0) || (y == 0) || (x == _dimX - 1) || (y == _dimY - 1)) continue;
                    flashes++;
                    toReset.Add(point);
                    foreach((int x, int y) p in FindNeighbours(point))
                        BumpEnergy(p.x, p.y, flashers);
                }

                // reset flashers to zero
                foreach ((int x, int y) in toReset)
                    workarea[x, y] = 0;

                return flashes;
            }

            int steps = 0;
            int flashes = 0;
            while(flashes != 100)
            {
                steps++;
                flashes = ProcessOneStep();
            }

            return new($"Steps: {steps}");
        }
    }
}
