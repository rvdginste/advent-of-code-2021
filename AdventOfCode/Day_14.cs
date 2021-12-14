using AoCHelper;

namespace AdventOfCode;

public class Day_14 : BaseDay
{
    private readonly string _template;
    private readonly Dictionary<(char a, char b), char> _mappings;

    public Day_14()
    {
        using StreamReader reader = File.OpenText(InputFilePath);
        string line = reader.ReadLine();
        _template = line;

        line = reader.ReadLine();
        line = reader.ReadLine();
        _mappings = new();
        while (!string.IsNullOrWhiteSpace(line))
        {
            string[] pair = line.Split(" -> ");
            _mappings[(a: pair[0][0], b: pair[0][1])] = pair[1][0];
            line = reader.ReadLine();
        }
    }

    public override ValueTask<string> Solve_1()
    {
        char[] ProcessOneStep(char[] input)
        {
            List<char> result = new() { input[0] };
            for (int i = 0; i < input.Length - 1; i++)
            {
                result.Add(_mappings[(a: input[i], b: input[i + 1])]);
                result.Add(input[i + 1]);
            }

            return result.ToArray();
        }

        int cnt = 10;
        char[] work = _template.ToCharArray();
        while (cnt > 0)
        {
            work = ProcessOneStep(work);
            cnt--;
        }

        Dictionary<char, int> counts =
            work
                .GroupBy(x => x)
                .ToDictionary(
                    g => g.Key,
                    g => g.Count());
        int min = counts.OrderBy(kv => kv.Value).First().Value;
        int max = counts.OrderByDescending(kv => kv.Value).First().Value;
        return new($"Diff: {max-min}");
    }

    public override ValueTask<string> Solve_2()
    {
        Dictionary<(char a, char b), long> pairs = new();
        Dictionary<char, long> counts = new();

        void Initialise(char[] input)
        {
            if (!counts.ContainsKey(input[0])) counts[input[0]] = 0;
            counts[input[0]] += 1;
            for (int i = 0; i < input.Length - 1; i++)
            {
                (char a, char b) p = (a: input[i], b: input[i + 1]);
                if (!pairs.ContainsKey(p)) pairs[p] = 0;
                pairs[p] += 1;
                if (!counts.ContainsKey(p.b)) counts[p.b] = 0;
                counts[p.b] += 1;
            }
        }

        Initialise(_template.ToCharArray());

        void ProcessOneStep()
        {
            Dictionary<(char a, char b), long> temp = new();
            foreach ((char a, char b) p in pairs.Keys)
            {
                char x = _mappings[p];
                if (!counts.ContainsKey(x)) counts[x] = 0;
                counts[x] += pairs[p];

                (char a, char b) pax = (a: p.a, b: x);
                (char a, char b) pxb = (a: x, b: p.b);
                if (!temp.ContainsKey(pax)) temp[pax] = 0;
                temp[pax] += pairs[p];
                if (!temp.ContainsKey(pxb)) temp[pxb] = 0;
                temp[pxb] += pairs[p];
            }

            pairs = temp;
        }

        int cnt = 40;
        while (cnt > 0)
        {
            ProcessOneStep();
            cnt--;
        }

        long min = counts.OrderBy(kv => kv.Value).First().Value;
        long max = counts.OrderByDescending(kv => kv.Value).First().Value;
        return new($"Diff: {max-min}");
    }
}