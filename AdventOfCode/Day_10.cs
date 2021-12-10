using System.ComponentModel;
using System.Globalization;
using AoCHelper;

namespace AdventOfCode
{
    public class Day_10 : BaseDay
    {
        private readonly string[] _input;

        public Day_10()
        {
            _input = File.ReadAllLines(InputFilePath);
        }

        public override ValueTask<string> Solve_1()
        {
            long RateLine(string line)
            {
                Dictionary<char, char> expected =
                    new()
                    {
                        { ')', '(' },
                        { ']', '[' },
                        { '}', '{' },
                        { '>', '<' }
                    };

                Dictionary<char, long> score =
                    new()
                    {
                        { ')', 3 },
                        { ']', 57 },
                        { '}', 1197 },
                        { '>', 25137 }
                    };

                Stack<char> stack = new();
                foreach (char c in line)
                {
                    switch (c)
                    {
                        case '(' or '[' or '{' or '<':
                            stack.Push(c);
                            break;
                        case ')' or ']' or '}' or '>':
                            bool popped = stack.TryPop(out char pop);
                            if (!popped || pop != expected[c])
                                return score[c];
                            break;
                        default:
                            throw new InvalidEnumArgumentException("Systems down... systems down... =:(");
                    }
                }

                return 0;
            }

            long score = _input.Aggregate<string, long>(0, (acc, el) => acc + RateLine(el));

            return new($"Score = {score}");
        }

        public override ValueTask<string> Solve_2()
        {
            long? RateLine(string line)
            {
                Dictionary<char, char> expected =
                    new()
                    {
                        { ')', '(' },
                        { ']', '[' },
                        { '}', '{' },
                        { '>', '<' }
                    };

                Dictionary<char, long> score =
                    new()
                    {
                        { '(', 1 },
                        { '[', 2 },
                        { '{', 3 },
                        { '<', 4 }
                    };

                Stack<char> stack = new();
                foreach (char c in line)
                    switch (c)
                    {
                        case '(' or '[' or '{' or '<':
                            stack.Push(c);
                            break;
                        case ')' or ']' or '}' or '>':
                            bool popped = stack.TryPop(out char pop);
                            if (!popped || pop != expected[c])
                                return null;
                            break;
                        default:
                            throw new InvalidEnumArgumentException("Systems down... systems down... =:(");
                    }

                long result = 0;
                while (stack.TryPop(out char c))
                {
                    result = (result * 5) + score[c];
                }

                return result;
            }

            long[] scores = _input.Select(RateLine).Where(x => x != null).Select(x => x.Value).OrderBy(x => x).ToArray();
            long middleScore = scores[scores.Length / 2];

            return new($"Middle score: {middleScore}");
        }
    }
}