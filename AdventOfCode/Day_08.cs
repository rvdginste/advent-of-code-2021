using AoCHelper;

namespace AdventOfCode
{
    public class Day_08 : BaseDay
    {
        private readonly (string[] numbers, string[] values)[] _input;

        public Day_08()
        {
            _input =
                File
                .ReadAllLines(InputFilePath)
                .Select(l => l.Split('|', StringSplitOptions.TrimEntries))
                .Select(s => (numbers: s[0].Split(' ', StringSplitOptions.RemoveEmptyEntries), values: s[1].Split()))
                .ToArray();
        }

        public override ValueTask<string> Solve_1()
        {
            int count =
                _input
                .SelectMany(r => r.values)
                .Count(v => v.Length is 2 or 3 or 4 or 7);

            return new($"Occurrences: {count}");
        }

        public override ValueTask<string> Solve_2()
        {
            IDictionary<string, int> stringToDigit =
                new Dictionary<string, int>
                {
                    { "cf", 1 },
                    { "acf", 7 },
                    { "bcdf", 4 },
                    { "acdeg", 2 },
                    { "acdfg", 3 },
                    { "abdfg", 5 },
                    { "abcefg", 0 },
                    { "abdefg", 6 },
                    { "abcdfg", 9 },
                    { "abcdefg", 8 },
                };
            IDictionary<int, string> digitToString =
                new Dictionary<int, string>
                {
                    { 1, "cf" },
                    { 7, "acf" },
                    { 4, "bcdf" },
                    { 2, "acdeg" },
                    { 3, "acdfg" },
                    { 5, "abdfg" },
                    { 0, "abcefg" },
                    { 6, "abdefg" },
                    { 9, "abcdfg" },
                    { 8, "abcdefg" },
                };
            HashSet<string> validCombos = stringToDigit.Keys.ToHashSet();

            string Translate(string original, IDictionary<char, char> mapping)
                => new(original.Select(c => mapping[c]).OrderBy(c => c).ToArray());

            bool IsValidMapping(string[] numbers, IDictionary<char, char> mapping)
                => numbers.Aggregate(true, (acc, el) => acc && validCombos.Contains(Translate(el, mapping)));

            IEnumerable<IDictionary<char, char>> GeneratePermutations(IDictionary<char, char> accu, IDictionary<char, ISet<char>> candidates)
            {
                char[] newCandidateKeys = candidates.Keys.Where(k => !accu.ContainsKey(k)).ToArray();
                if (newCandidateKeys.Length == 0)
                {
                    yield return accu;
                }
                else
                {
                    char newKey = newCandidateKeys.First();
                    char[] newValues = candidates[newKey].Where(v => !accu.Values.Contains(v)).ToArray();
                    foreach (char newValue in newValues)
                    {
                        accu[newKey] = newValue;
                        foreach (IDictionary<char,char> permutation in GeneratePermutations(accu, candidates))
                        {
                            yield return permutation;
                        }

                        accu.Remove(newKey);
                    }
                }
            }

            IDictionary<char, char> FindMapping(string[] numbers)
            {
                IDictionary<char, ISet<char>> candidates =
                    new Dictionary<char, ISet<char>>
                    {
                        { 'a', new HashSet<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'g' }},
                        { 'b', new HashSet<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'g' }},
                        { 'c', new HashSet<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'g' }},
                        { 'd', new HashSet<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'g' }},
                        { 'e', new HashSet<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'g' }},
                        { 'f', new HashSet<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'g' }},
                        { 'g', new HashSet<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'g' }},
                    };

                // filter possible candidates
                foreach (IGrouping<int,int> grouping
                         in digitToString
                             .Keys
                             .GroupBy(k => digitToString[k].Length)
                             .OrderBy(g => g.Count()))
                {
                    char[] source =
                        numbers
                            .Where(n => n.Length == grouping.Key)
                            .SelectMany(n => n.ToCharArray())
                            .Distinct()
                            .OrderBy(c => c)
                            .ToArray();
                    char[] destination =
                        stringToDigit
                            .Keys
                            .Where(n => n.Length == grouping.Key)
                            .SelectMany(n => n.ToCharArray())
                            .Distinct()
                            .OrderBy(c => c)
                            .ToArray();
                    foreach (char c in candidates.Keys)
                    {
                        if (source.Contains(c))
                        {
                            candidates[c].IntersectWith(destination);
                        }
                        else
                        {
                            candidates[c].ExceptWith(destination);
                        }
                    }
                }

                // generate all possible permutations
                foreach (IDictionary<char,char> permutation in GeneratePermutations(new Dictionary<char, char>(), candidates))
                {
                    if (IsValidMapping(numbers, permutation))
                    {
                        return permutation;
                    }
                }

                return null;
            }

            long sum = 0;
            foreach ((string[] numbers, string[] values) in _input)
            {
                IDictionary<char, char> mapping = FindMapping(numbers);
                sum +=
                    values
                        .Aggregate(
                            0L,
                            (acc, el) => 10 * acc + stringToDigit[Translate(el, mapping)]);
            }

            return new($"Sum is: {sum}");
        }
    }
}
