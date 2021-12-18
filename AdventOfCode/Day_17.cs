using System.Globalization;
using AoCHelper;

namespace AdventOfCode;

public class Day_17 : BaseDay
{
    private readonly int _minX;
    private readonly int _maxX;
    private readonly int _minY;
    private readonly int _maxY;

    public Day_17()
    {
        string[] lines = File.ReadAllLines(InputFilePath);
        string[] parts = lines[0].Split(new[] { ',', ' ' });
        int[] rangeX =
            parts
                .Single(s => s.StartsWith("x="))
                .Substring(2)
                .Split("..")
                .Select(s => int.Parse(s, CultureInfo.InvariantCulture))
                .ToArray();
        int[] rangeY =
            parts
                .Single(s => s.StartsWith("y="))
                .Substring(2)
                .Split("..")
                .Select(s => int.Parse(s, CultureInfo.InvariantCulture))
                .ToArray();
        _minX = rangeX[0];
        _maxX = rangeX[1];
        _minY = rangeY[0];
        _maxY = rangeY[1];
    }

    public override ValueTask<string> Solve_1()
    {
        Dictionary<int, HashSet<int>> candidatesX = new();

        IEnumerable<int> GeneratorX(int velocityX)
        {
            int position = 0;
            int velocity = velocityX;
            while (true)
            {
                position += velocity;
                yield return position;
                if (velocity > 0) velocity--;
            }
        }

        IEnumerable<(int x, int y)> GeneratorXY(int x, int y)
        {
            int positionX = 0;
            int positionY = 0;
            int velocityX = x;
            int velocityY = y;
            while (true)
            {
                positionX += velocityX;
                positionY += velocityY;
                yield return (x: positionX, y: positionY);
                if (velocityX > 0) velocityX--;
                velocityY--;
            }
        }

        void DetermineCandidatesX()
        {
            for (int x = 1; x <= _maxX; x++)
            {
                int tick = 0;
                int previousPositionX = -1;
                foreach (int positionX in GeneratorX(x))
                {
                    if (previousPositionX == positionX)
                    {
                        if ((_minX <= positionX) && (positionX <= _maxX)) candidatesX[x].Add(1500);
                        break;
                    }

                    previousPositionX = positionX;
                    tick++;

                    if (positionX < _minX) continue;
                    if (positionX > _maxX) break;
                    if (!candidatesX.ContainsKey(x)) candidatesX[x] = new();
                    candidatesX[x].Add(tick);
                }
            }
        }

        DetermineCandidatesX();

        int highestY = 0;

        void Validate(int x, int y)
        {
            int highest = 0;
            foreach ((int x, int y) position in GeneratorXY(x, y))
            {
                if (position.y > highest) highest = position.y;

                if (position.x < _minX) continue;
                if (position.x > _maxX) break;
                if (position.y < _minY) break;

                if (position.y <= _maxY)
                {
                    if (highestY < highest) highestY = highest;
                }
            }
        }

        // check possible y for all possible x
        foreach (int x in candidatesX.Keys)
        {
            // define range of min and max steps
            int tMin = candidatesX[x].Min();
            int tMax = candidatesX[x].Max();

            // define min and max y possibilities for tMin and tMax
            int yMinTMin = (_minY + (tMin * (tMin - 1) / 2)) / tMin;
            int yMaxTMin = (_maxY + (tMin * (tMin - 1) / 2)) / tMin;
            int yMinTMax = (_minY + (tMax * (tMax - 1) / 2)) / tMax;
            int yMaxTMax = (_maxY + (tMax * (tMax - 1) / 2)) / tMax;

            // range y
            int[] range = { yMinTMin, yMaxTMin, yMinTMax, yMaxTMax };
            int startY = range.Min() - 1;
            int endY = range.Max() + 1;

            for (int y = startY; y <= endY; y++)
            {
                Validate(x, y);
            }
        }

        return new($"Highest: {highestY}");
    }

    public override ValueTask<string> Solve_2()
    {
        Dictionary<int, HashSet<int>> candidatesX = new();

        IEnumerable<int> GeneratorX(int velocityX)
        {
            int position = 0;
            int velocity = velocityX;
            while (true)
            {
                position += velocity;
                yield return position;
                if (velocity > 0) velocity--;
            }
        }

        IEnumerable<(int x, int y)> GeneratorXY(int x, int y)
        {
            int positionX = 0;
            int positionY = 0;
            int velocityX = x;
            int velocityY = y;
            while (true)
            {
                positionX += velocityX;
                positionY += velocityY;
                yield return (x: positionX, y: positionY);
                if (velocityX > 0) velocityX--;
                velocityY--;
            }
        }

        void DetermineCandidatesX()
        {
            for (int x = 1; x <= _maxX; x++)
            {
                int tick = 0;
                int previousPositionX = -1;
                foreach (int positionX in GeneratorX(x))
                {
                    if (previousPositionX == positionX)
                    {
                        if ((_minX <= positionX) && (positionX <= _maxX)) candidatesX[x].Add(1500);
                        break;
                    }

                    previousPositionX = positionX;
                    tick++;

                    if (positionX < _minX) continue;
                    if (positionX > _maxX) break;
                    if (!candidatesX.ContainsKey(x)) candidatesX[x] = new();
                    candidatesX[x].Add(tick);
                }
            }
        }

        DetermineCandidatesX();

        int nrOfOptions = 0;

        void Validate(int x, int y)
        {
            foreach ((int x, int y) position in GeneratorXY(x, y))
            {
                if (position.x < _minX) continue;
                if (position.x > _maxX) break;
                if (position.y < _minY) break;

                if (position.y <= _maxY)
                {
                    nrOfOptions++;
                    break;
                }
            }
        }

        // check possible y for all possible x
        foreach (int x in candidatesX.Keys)
        {
            // define range of min and max steps
            int tMin = candidatesX[x].Min();
            int tMax = candidatesX[x].Max();

            // define min and max y possibilities for tMin and tMax
            int yMinTMin = (_minY + (tMin * (tMin - 1) / 2)) / tMin;
            int yMaxTMin = (_maxY + (tMin * (tMin - 1) / 2)) / tMin;
            int yMinTMax = (_minY + (tMax * (tMax - 1) / 2)) / tMax;
            int yMaxTMax = (_maxY + (tMax * (tMax - 1) / 2)) / tMax;

            // range y
            int[] range = { yMinTMin, yMaxTMin, yMinTMax, yMaxTMax };
            int startY = range.Min() - 1;
            int endY = range.Max() + 1;

            for (int y = startY; y <= endY; y++)
            {
                Validate(x, y);
            }
        }

        return new($"NrOfOptions: {nrOfOptions}");
    }
}