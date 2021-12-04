using AoCHelper;

using System.Globalization;

#pragma warning disable CA1707
#pragma warning disable IDE0061
namespace AdventOfCode
{
    public class Day_04 : BaseDay
    {
        private readonly int[] _numbers;
        private readonly int[][,] _boards;
        private readonly bool[][,] _marks01;
        private readonly bool[][,] _marks02;

        public Day_04()
        {
            string[] _input = File.ReadAllLines(InputFilePath);
            _numbers = _input[0].Split(',').Select(s => int.Parse(s, CultureInfo.InvariantCulture)).ToArray();

            int nrBoards = (_input.Length - 1) / 6;
            _boards = new int[nrBoards][,];
            _marks01 = new bool[nrBoards][,];
            _marks02 = new bool[nrBoards][,];

            for (int i = 0; i < nrBoards; i++)
            {
                int[][] board =
                    _input[((i * 6) + 2)..((i * 6) + 7)]
                    .Select(s => s.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n, CultureInfo.InvariantCulture)).ToArray())
                    .ToArray();

                _boards[i] = new int[5, 5];
                _marks01[i] = new bool[5, 5];
                _marks02[i] = new bool[5, 5];

                for (int k = 0; k < 5; k++)
                {
                    for (int l = 0; l < 5; l++)
                    {
                        _boards[i][k, l] = board[k][l];
                    }
                }
            }
        }

        public override ValueTask<string> Solve_1()
        {
            bool IsWin(int idx)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (Enumerable.Range(0, 5).Aggregate(true, (acc, el) => acc && _marks01[idx][i, el])
                        || Enumerable.Range(0, 5).Aggregate(true, (acc, el) => acc && _marks01[idx][el, i]))
                    {
                        return true;
                    }
                }

                return false;
            }

            int CountUnmarked(int idx)
            {
                int sum = 0;
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (!_marks01[idx][i, j])
                        {
                            sum += _boards[idx][i, j];
                        }
                    }
                }

                return sum;
            }

            void SetMarkFor(int number)
            {
                for (int idx = 0; idx < _boards.Length; idx++)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            if (_boards[idx][i, j] == number)
                            {
                                _marks01[idx][i, j] = true;
                            }
                        }
                    }
                }
            }

            int? PlayAndCheck(int number)
            {
                SetMarkFor(number);
                for (int idx = 0; idx < _boards.Length; idx++)
                {
                    if (IsWin(idx))
                    {
                        return idx;
                    }
                }

                return null;
            }

            int? winningBoard = null;
            int num = -1;
            while (winningBoard == null)
            {
                num++;
                winningBoard = PlayAndCheck(_numbers[num]);
            }

            int unmarked = CountUnmarked(winningBoard.Value);
            int number = _numbers[num];

            return new($"Idx: {winningBoard}, Unmarked: {unmarked}, Number: {number}, result: {unmarked * number}");
        }

        public override ValueTask<string> Solve_2()
        {
            bool IsWin(int idx)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (Enumerable.Range(0, 5).Aggregate(true, (acc, el) => acc && _marks02[idx][i, el])
                        || Enumerable.Range(0, 5).Aggregate(true, (acc, el) => acc && _marks02[idx][el, i]))
                    {
                        return true;
                    }
                }

                return false;
            }

            int CountUnmarked(int idx)
            {
                int sum = 0;
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (!_marks02[idx][i, j])
                        {
                            sum += _boards[idx][i, j];
                        }
                    }
                }

                return sum;
            }

            void SetMarkFor(int number)
            {
                for (int idx = 0; idx < _boards.Length; idx++)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            if (_boards[idx][i, j] == number)
                            {
                                _marks02[idx][i, j] = true;
                            }
                        }
                    }
                }
            }

            ISet<int> remaining = new HashSet<int>(Enumerable.Range(0, _boards.Length));
            int last = -1;
            int num = -1;
            while (remaining.Any())
            {
                num++;

                SetMarkFor(_numbers[num]);
                foreach (int idx in remaining.ToArray())
                {
                    if (IsWin(idx))
                    {
                        remaining.Remove(idx);
                        last = idx;
                    }
                }
            }

            int unmarked = CountUnmarked(last);
            int number = _numbers[num];

            return new($"Idx: {last}, Unmarked: {unmarked}, Number: {number}, result: {unmarked * number}");
        }
    }
}
