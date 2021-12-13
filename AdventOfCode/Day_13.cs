using System.Globalization;
using System.Text;
using AoCHelper;
// ReSharper disable InconsistentNaming

namespace AdventOfCode;

public class Day_13 : BaseDay
{
    private readonly HashSet<(int x, int y)> _input;
    private readonly List<(int x, int y)> _folds;

    public Day_13()
    {
        using StreamReader reader = File.OpenText(InputFilePath);
        _input = new();
        string line = reader.ReadLine();
        while (!string.IsNullOrWhiteSpace(line))
        {
            int[] pair = line.Split(',').Select(s => int.Parse(s, CultureInfo.InvariantCulture)).ToArray();
            _input.Add((x: pair[0], y: pair[1]));
            line = reader.ReadLine();
        }

        _folds = new();
        line = reader.ReadLine();
        while (!string.IsNullOrWhiteSpace(line))
        {
            string[] pair = line.Split().Last().Split('=');
            if (string.Equals(pair[0], "x", StringComparison.InvariantCulture))
                _folds.Add((x: int.Parse(pair[1], CultureInfo.InvariantCulture), y: 0));
            if (string.Equals(pair[0], "y", StringComparison.InvariantCulture))
                _folds.Add((x: 0, y: int.Parse(pair[1], CultureInfo.InvariantCulture)));
            line = reader.ReadLine();
        }
    }

    public override ValueTask<string> Solve_1()
    {
        HashSet<(int x, int y)> workarea = new(_input);

        void Fold((int x, int y) fold)
        {
            if (fold.x == 0)
            {
                int doubleFold = 2 * fold.y;
                foreach ((int x, int y) p in workarea.Where(p => p.y > fold.y).ToArray())
                {
                    workarea.Remove(p);
                    workarea.Add((x: p.x, y: doubleFold - p.y));
                }
            }

            if (fold.y == 0)
            {
                int doubleFold = 2 * fold.x;
                foreach ((int x, int y) p in workarea.Where(p => p.x > fold.x).ToArray())
                {
                    workarea.Remove(p);
                    workarea.Add((x: doubleFold - p.x, y: p.y));
                }
            }
        }

        Fold(_folds.First());

        return new($"Nr points: {workarea.Count}");
    }

    public override ValueTask<string> Solve_2()
    {
        HashSet<(int x, int y)> workarea = new(_input);

        void Fold((int x, int y) fold)
        {
            if (fold.x == 0)
            {
                int doubleFold = 2 * fold.y;
                foreach ((int x, int y) p in workarea.Where(p => p.y > fold.y).ToArray())
                {
                    workarea.Remove(p);
                    workarea.Add((x: p.x, y: doubleFold - p.y));
                }
            }

            if (fold.y == 0)
            {
                int doubleFold = 2 * fold.x;
                foreach ((int x, int y) p in workarea.Where(p => p.x > fold.x).ToArray())
                {
                    workarea.Remove(p);
                    workarea.Add((x: doubleFold - p.x, y: p.y));
                }
            }
        }

        foreach ((int x, int y) fold in _folds)
        {
            Fold(fold);
        }

        int dimX = workarea.Select(p => p.x).Max();
        int dimY = workarea.Select(p => p.y).Max();
        StringBuilder sb = new();
        for (int y = 0; y <= dimY; y++)
        {
            for (int x = 0; x <= dimX; x++)
            {
                sb.Append(workarea.Contains((x , y)) ? "#" : ".");
            }

            sb.AppendLine();
        }

        return new(sb.ToString());
    }
}