using AoCHelper;

#pragma warning disable CA1707
#pragma warning disable IDE0061
namespace AdventOfCode
{
    public class Day_03 : BaseDay
    {
        private readonly string[] _input;
        private readonly int[] _numbers;

        public Day_03()
        {
            _input =
                File
                .ReadLines(InputFilePath)
                .ToArray();
            _numbers =
                _input
                .Select(s => Convert.ToInt32(s, 2))
                .ToArray();
        }

        public override ValueTask<string> Solve_1()
        {
            int size = _input[0].Length;

            int v(int val, int source, int idx)
                => (source & (1 << (size - 1 - idx))) > 0 ? val + 1 : val;

            int p(int tot, int val, int idx)
                => (val * 2 > tot) ? 1 << (size - 1 - idx) : 0;

            (int total, int[] bits) =
                _numbers
                .Aggregate<int, (int total, int[] bits)>(
                    (0, new int[size]),
                    (acc, e) =>
                    (acc.total + 1,
                     acc.bits.Select((b, i) => v(b, e, i)).ToArray()));

            int gamma = bits.Select((b, i) => p(total, b, i)).Sum();
            int epsilon = (1 << size) - 1 - gamma;

            return new($"Gamma: {gamma}, Epsilon: {epsilon}, result: {gamma * epsilon}");
        }

        public override ValueTask<string> Solve_2()
        {
            int size = _input[0].Length;

            int CalculateMask(int idx)
                => 1 << (size - 1 - idx);

            (int zero, int one) CountBit(IEnumerable<int> source, int mask)
                => source
                .Aggregate<int, (int zero, int one)>(
                    (0, 0),
                    (acc, el) => (el & mask) > 0 ? (acc.zero, acc.one + 1) : (acc.zero + 1, acc.one));

            int[] FilterBit(int[] source, int mask, bool bit)
                => source.Where(s => (bit && ((s & mask) > 0)) || (!bit && ((s & mask) == 0))).ToArray();

            // determine oxygen
            int[] candidates = _numbers;
            int index = 0;
            while (candidates.Length > 1)
            {
                int mask = CalculateMask(index++);
                (int zero, int one) = CountBit(candidates, mask);
                candidates = FilterBit(candidates, mask, one >= zero);
            }

            int oxygen = candidates[0];

            // determine co2
            candidates = _numbers;
            index = 0;
            while (candidates.Length > 1)
            {
                int mask = CalculateMask(index++);
                (int zero, int one) = CountBit(candidates, mask);
                candidates = FilterBit(candidates, mask, one < zero);
            }

            int co2 = candidates[0];

            return new($"Oxygen: {oxygen}, CO2: {co2}, result: {oxygen * co2}");
        }
    }
}
