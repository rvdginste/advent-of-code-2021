using System.Globalization;
using AoCHelper;

namespace AdventOfCode;

public class Day_15: BaseDay
{
    private readonly int _dimX;
    private readonly int _dimY;
    private readonly int[,] _input;

    public Day_15()
    {
        string[] lines = File.ReadAllLines(InputFilePath);
        _dimX = lines[0].Length;
        _dimY = lines.Length;
        _input = new int[_dimX, _dimY];
        for (int i = 0; i < _dimX; i++)
        for (int j = 0; j < _dimY; j++)
            _input[i, j] = int.Parse(lines[j][i..(i + 1)], CultureInfo.InvariantCulture);
    }

    public override ValueTask<string> Solve_1()
    {
        bool[,] visited = new bool[_dimX, _dimY];
        int[,] cost = new int[_dimX, _dimY];
        for (int i = 0; i < _dimX; i++)
        for (int j = 0; j < _dimY; j++)
            cost[i, j] = int.MaxValue;

        IEnumerable<(int x, int y)> FindNeighbours((int x, int y) p)
        {
            if (p.x > 0 && !visited[p.x - 1, p.y]) yield return (x: p.x - 1, p.y);
            if (p.x < _dimX - 1 && !visited[p.x + 1, p.y]) yield return (x: p.x + 1, p.y);
            if (p.y > 0 && !visited[p.x, p.y - 1]) yield return (p.x, y: p.y - 1);
            if (p.y < _dimY - 1 && !visited[p.x, p.y + 1]) yield return (p.x, p.y + 1);
        }

        void Visit((int x, int y) origin)
        {
            cost[origin.x, origin.y] = 0;
            HashSet<(int x, int y)> visits = new() { origin };

            while (visits.Any())
            {
                (int x, int y) p = visits.OrderBy(point => cost[point.x, point.y]).First();
                visits.Remove(p);
                visited[p.x, p.y] = true;

                foreach ((int x, int y) np in FindNeighbours(p))
                {
                    visits.Add(np);
                    int newCost = cost[p.x, p.y] + _input[np.x, np.y];
                    if (cost[np.x, np.y] > newCost) cost[np.x, np.y] = newCost;
                }
            }
        }

        (int x, int y) source = (x: 0, y: 0);
        (int x, int y) destination = (x: _dimX - 1, y: _dimY - 1);
        Visit(source);
        int costDestination = cost[destination.x, destination.y];
        return new($"Minimal cost: {costDestination}");
    }

    public override ValueTask<string> Solve_2()
    {
        int bigDimX = 5 * _dimX;
        int bigDimY = 5 * _dimY;
        int[,] input = new int[bigDimX, bigDimY];
        bool[,] visited = new bool[bigDimX, bigDimY];
        int[,] cost = new int[bigDimX, bigDimY];
        for (int i = 0; i < bigDimX; i++)
        for (int j = 0; j < bigDimY; j++)
        {
            int im = i % _dimX;
            int id = i / _dimX;
            int jm = j % _dimY;
            int jd = j / _dimY;
            input[i, j] = _input[im, jm] + id + jd;
            if (input[i, j] > 9) input[i, j] -= 9;
            cost[i, j] = int.MaxValue;
        }

        IEnumerable<(int x, int y)> FindNeighbours((int x, int y) p)
        {
            if (p.x > 0 && !visited[p.x - 1, p.y]) yield return (x: p.x - 1, p.y);
            if (p.x < bigDimX - 1 && !visited[p.x + 1, p.y]) yield return (x: p.x + 1, p.y);
            if (p.y > 0 && !visited[p.x, p.y - 1]) yield return (p.x, y: p.y - 1);
            if (p.y < bigDimY - 1 && !visited[p.x, p.y + 1]) yield return (p.x, p.y + 1);
        }

        void Visit((int x, int y) origin)
        {
            cost[origin.x, origin.y] = 0;
            HashSet<(int x, int y)> visits = new() { origin };

            while (visits.Any())
            {
                (int x, int y) p = visits.OrderBy(point => cost[point.x, point.y]).First();
                visits.Remove(p);
                visited[p.x, p.y] = true;

                foreach ((int x, int y) np in FindNeighbours(p))
                {
                    visits.Add(np);
                    int newCost = cost[p.x, p.y] + input[np.x, np.y];
                    if (cost[np.x, np.y] > newCost) cost[np.x, np.y] = newCost;
                }
            }
        }

        (int x, int y) source = (x: 0, y: 0);
        (int x, int y) destination = (x: bigDimX - 1, y: bigDimY - 1);
        Visit(source);
        int costDestination = cost[destination.x, destination.y];
        return new($"Minimal cost: {costDestination}");
    }
}