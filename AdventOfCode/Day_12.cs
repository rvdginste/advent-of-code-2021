using AoCHelper;
// ReSharper disable VirtualMemberCallInConstructor
// ReSharper disable InconsistentNaming

namespace AdventOfCode;

public class Day_12 : BaseDay
{
    private readonly IDictionary<string, ISet<string>> _paths;

    public Day_12()
    {
        _paths = new Dictionary<string, ISet<string>>();
        foreach (string line in File.ReadAllLines(InputFilePath).Where(x => !string.IsNullOrWhiteSpace(x)))
        {
            string[] path = line.Trim().Split('-');
            string p1 = path[0];
            string p2 = path[1];
            if (!_paths.ContainsKey(p1)) _paths[p1] = new HashSet<string>();
            _paths[p1].Add(p2);
            if (!_paths.ContainsKey(p2)) _paths[p2] = new HashSet<string>();
            _paths[p2].Add(p1);
        }
    }

    public override ValueTask<string> Solve_1()
    {
        IEnumerable<LinkedList<string>> FindPaths(LinkedList<string> path, HashSet<string> crossedSmallCaves)
        {
            string[] options =
                _paths[path.Last!.Value]
                    .Where(s => !string.Equals(s, "start", StringComparison.InvariantCulture))
                    .Where(s => !crossedSmallCaves.Contains(s))
                    .ToArray();
            foreach (string option in options)
            {
                LinkedList<string> clone = new(path);
                clone.AddLast(option);
                if (string.Equals(option, "end", StringComparison.InvariantCulture))
                {
                    yield return clone;
                }
                else
                {
                    HashSet<string> newCrossedSmallCaves =
                        string.Equals(option, option.ToLowerInvariant(), StringComparison.InvariantCulture)
                            ? new(crossedSmallCaves) { option }
                            : (HashSet<string>)crossedSmallCaves;
                    foreach (var foundPath in FindPaths(clone, newCrossedSmallCaves))
                        yield return foundPath;
                }
            }
        }

        LinkedList<string>[] paths =
            FindPaths(
                new LinkedList<string>(new[] { "start" }),
                new HashSet<string>())
                .ToArray();

        return new($"Nr paths: {paths.Count()}");
    }

    public override ValueTask<string> Solve_2()
    {
        IEnumerable<LinkedList<string>> FindPaths(
            LinkedList<string> path,
            HashSet<string> crossedSmallCaves,
            bool extraSmall)
        {
            string[] options =
                _paths[path.Last!.Value]
                    .Where(s => !string.Equals(s, "start", StringComparison.InvariantCulture))
                    .Where(s => !extraSmall || !crossedSmallCaves.Contains(s))
                    .ToArray();
            foreach (string option in options)
            {
                LinkedList<string> clone = new(path);
                clone.AddLast(option);
                if (string.Equals(option, "end", StringComparison.InvariantCulture))
                {
                    yield return clone;
                }
                else
                {
                    bool newExtraSmall = extraSmall;
                    HashSet<string> newCrossedSmallCaves = crossedSmallCaves;
                    if (string.Equals(option, option.ToLowerInvariant(), StringComparison.InvariantCulture))
                    {
                        newCrossedSmallCaves = new(crossedSmallCaves);
                        if (!newExtraSmall && newCrossedSmallCaves.Contains(option))
                        {
                            newExtraSmall = true;
                        }
                        else
                        {
                            newCrossedSmallCaves.Add(option);
                        }
                    }
                    foreach (var foundPath in FindPaths(clone, newCrossedSmallCaves, newExtraSmall))
                        yield return foundPath;
                }
            }
        }

        LinkedList<string>[] paths =
            FindPaths(
                    new LinkedList<string>(new[] { "start" }),
                    new HashSet<string>(),
                    false)
                .ToArray();

        return new($"Nr paths: {paths.Count()}");
    }
}