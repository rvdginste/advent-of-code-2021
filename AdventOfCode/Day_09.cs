using System.Collections;
using System.Globalization;
using AoCHelper;

namespace AdventOfCode
{
    public class Day_09 : BaseDay
    {
        private readonly int _dimX;
        private readonly int _dimY;
        private readonly int[,] _input;

        public Day_09()
        {
            string[] lines = File.ReadAllLines(InputFilePath);
            _dimX = lines[0].Length + 2;
            _dimY = lines.Length + 2;

            _input = new int[_dimX, _dimY];
            for (int i = 0; i < _dimX; i++)
            {
                for (int j = 0; j < _dimY; j++)
                {
                    if (i == 0 || j == 0 || i == (_dimX - 1) || j == (_dimY - 1))
                    {
                        _input[i, j] = 9;
                    }
                    else
                    {
                        _input[i, j] = int.Parse(lines[j - 1][(i - 1)..i], NumberStyles.Integer);
                    }
                }
            }
        }

        public override ValueTask<string> Solve_1()
        {
            int RiskForLowPoint(int x, int y)
            {
                int val = _input[x, y];
                bool low = _input[x - 1, y] > val;
                low = low && _input[x + 1, y] > val;
                low = low && _input[x, y - 1] > val;
                low = low && _input[x, y + 1] > val;

                return !low ? 0 : val + 1;
            }

            int sum = 0;
            for (int i = 1; i < _dimX - 1; i++)
            {
                for (int j = 1; j < _dimY - 1; j++)
                {
                    sum += RiskForLowPoint(i, j);
                }
            }

            return new($"Sum risks = {sum}");
        }

        public override ValueTask<string> Solve_2()
        {
            bool IsLowPoint(int x, int y)
            {
                int val = _input[x, y];
                bool low = _input[x - 1, y] > val;
                low = low && _input[x + 1, y] > val;
                low = low && _input[x, y - 1] > val;
                low = low && _input[x, y + 1] > val;
                return low;
            }

            IEnumerable<(int x, int y)> FindHigherNeighbours(int x, int y)
            {
                int val = _input[x, y];
                if (_input[x - 1, y] >= val && _input[x - 1, y] < 9) yield return (x: x - 1, y);
                if (_input[x + 1, y] >= val && _input[x + 1, y] < 9) yield return (x: x + 1, y);
                if (_input[x, y - 1] >= val && _input[x, y - 1] < 9) yield return (x, y - 1);
                if (_input[x, y + 1] >= val && _input[x, y + 1] < 9) yield return (x, y + 1);
            }

            int SizeOfBasin((int x, int y) lowPoint)
            {
                HashSet<(int x, int y)> basinPoints = new() { lowPoint };
                Stack<(int x, int y)> stack = new();
                stack.Push(lowPoint);
                while (stack.Count > 0)
                {
                    (int x, int y) = stack.Pop();
                    foreach ((int x, int y) higherNeighbour in FindHigherNeighbours(x, y))
                    {
                        if (basinPoints.Add(higherNeighbour))
                        {
                            stack.Push(higherNeighbour);
                        }
                    }
                }

                return basinPoints.Count;
            }

            List<(int x, int y)> lowPoints = new();
            for (int i = 1; i < _dimX - 1; i++)
            {
                for (int j = 1; j < _dimY - 1; j++)
                {
                    if (IsLowPoint(i, j))
                    {
                        lowPoints.Add((i, j));
                    }
                }
            }

            int[] basins = lowPoints.Select(SizeOfBasin).OrderByDescending(x => x).ToArray();
            int multi = basins[0] * basins[1] * basins[2];

            return new($"Multi: {multi}");
        }
    }
}
