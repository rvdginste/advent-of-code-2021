using System.Collections;
using AoCHelper;

namespace AdventOfCode;

public class Day_16 : BaseDay
{
    private readonly BitArray _input;

    public Day_16()
    {
        string[] lines = File.ReadAllLines(InputFilePath);
        string bits = lines[0];
        int size = 4 * bits.Length;
        _input = new BitArray(size);
        int idx = 0;
        foreach (char c in bits)
        {
            switch (c)
            {
                case '0':
                    _input[idx++] = false;
                    _input[idx++] = false;
                    _input[idx++] = false;
                    _input[idx++] = false;
                    break;
                case '1':
                    _input[idx++] = false;
                    _input[idx++] = false;
                    _input[idx++] = false;
                    _input[idx++] = true;
                    break;
                case '2':
                    _input[idx++] = false;
                    _input[idx++] = false;
                    _input[idx++] = true;
                    _input[idx++] = false;
                    break;
                case '3':
                    _input[idx++] = false;
                    _input[idx++] = false;
                    _input[idx++] = true;
                    _input[idx++] = true;
                    break;
                case '4':
                    _input[idx++] = false;
                    _input[idx++] = true;
                    _input[idx++] = false;
                    _input[idx++] = false;
                    break;
                case '5':
                    _input[idx++] = false;
                    _input[idx++] = true;
                    _input[idx++] = false;
                    _input[idx++] = true;
                    break;
                case '6':
                    _input[idx++] = false;
                    _input[idx++] = true;
                    _input[idx++] = true;
                    _input[idx++] = false;
                    break;
                case '7':
                    _input[idx++] = false;
                    _input[idx++] = true;
                    _input[idx++] = true;
                    _input[idx++] = true;
                    break;
                case '8':
                    _input[idx++] = true;
                    _input[idx++] = false;
                    _input[idx++] = false;
                    _input[idx++] = false;
                    break;
                case '9':
                    _input[idx++] = true;
                    _input[idx++] = false;
                    _input[idx++] = false;
                    _input[idx++] = true;
                    break;
                case 'A':
                    _input[idx++] = true;
                    _input[idx++] = false;
                    _input[idx++] = true;
                    _input[idx++] = false;
                    break;
                case 'B':
                    _input[idx++] = true;
                    _input[idx++] = false;
                    _input[idx++] = true;
                    _input[idx++] = true;
                    break;
                case 'C':
                    _input[idx++] = true;
                    _input[idx++] = true;
                    _input[idx++] = false;
                    _input[idx++] = false;
                    break;
                case 'D':
                    _input[idx++] = true;
                    _input[idx++] = true;
                    _input[idx++] = false;
                    _input[idx++] = true;
                    break;
                case 'E':
                    _input[idx++] = true;
                    _input[idx++] = true;
                    _input[idx++] = true;
                    _input[idx++] = false;
                    break;
                case 'F':
                    _input[idx++] = true;
                    _input[idx++] = true;
                    _input[idx++] = true;
                    _input[idx++] = true;
                    break;
            }
        }
    }

    public override ValueTask<string> Solve_1()
    {
        int scanner = 0;

        long ReadVersion()
            => ((_input[scanner++] ? 1 : 0) << 2)
               + ((_input[scanner++] ? 1 : 0) << 1)
               + (_input[scanner++] ? 1 : 0);

        long ReadTypeID()
            => ((_input[scanner++] ? 1 : 0) << 2)
               + ((_input[scanner++] ? 1 : 0) << 1)
               + (_input[scanner++] ? 1 : 0);

        long ReadLiteralValue()
        {
            bool next = true;
            long number = 0;
            while (next)
            {
                next = _input[scanner++];
                number = (number << 1) + (_input[scanner++] ? 1 : 0);
                number = (number << 1) + (_input[scanner++] ? 1 : 0);
                number = (number << 1) + (_input[scanner++] ? 1 : 0);
                number = (number << 1) + (_input[scanner++] ? 1 : 0);
            }

            return number;
        }

        bool ReadLengthTypeID()
            => _input[scanner++];

        long ReadTotalLengthInBits()
        {
            long number = 0;
            number = _input[scanner++] ? 1 : 0;
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            return number;
        }

        long ReadNumberOfSubPackets()
        {
            long number = 0;
            number = _input[scanner++] ? 1 : 0;
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            return number;
        }

        long ReadOperatorPacket()
        {
            long sum = 0;
            bool type = ReadLengthTypeID();
            if (!type)
            {
                long length = ReadTotalLengthInBits();
                long end = scanner + length;
                while (scanner < end)
                {
                    sum += ReadPacket();
                }
            }
            else
            {
                long nrSubPackets = ReadNumberOfSubPackets();
                long counter = nrSubPackets;
                while (counter-- > 0)
                {
                    sum += ReadPacket();
                }
            }

            return sum;
        }

        long sum = 0;

        long ReadPacket()
        {
            long version = ReadVersion();
            sum += version;
            long type = ReadTypeID();
            if (type == 4)
            {
                ReadLiteralValue();
            }
            else
            {
                ReadOperatorPacket();
            }

            return sum;
        }

        sum = ReadPacket();

        return new($"Sum: {sum}");
    }

    public override ValueTask<string> Solve_2()
    {
        int scanner = 0;

        long ReadVersion()
            => ((_input[scanner++] ? 1 : 0) << 2)
               + ((_input[scanner++] ? 1 : 0) << 1)
               + (_input[scanner++] ? 1 : 0);

        long ReadTypeID()
            => ((_input[scanner++] ? 1 : 0) << 2)
               + ((_input[scanner++] ? 1 : 0) << 1)
               + (_input[scanner++] ? 1 : 0);

        long ReadLiteralValue()
        {
            bool next = true;
            long number = 0;
            while (next)
            {
                next = _input[scanner++];
                number = (number << 1) + (_input[scanner++] ? 1 : 0);
                number = (number << 1) + (_input[scanner++] ? 1 : 0);
                number = (number << 1) + (_input[scanner++] ? 1 : 0);
                number = (number << 1) + (_input[scanner++] ? 1 : 0);
            }

            return number;
        }

        bool ReadLengthTypeID()
            => _input[scanner++];

        long ReadTotalLengthInBits()
        {
            long number = 0;
            number = _input[scanner++] ? 1 : 0;
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            return number;
        }

        long ReadNumberOfSubPackets()
        {
            long number = 0;
            number = _input[scanner++] ? 1 : 0;
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            number = (number << 1) + (_input[scanner++] ? 1 : 0);
            return number;
        }

        long ReadOperatorPacket(long type)
        {
            List<long> values = new();
            bool lengthType = ReadLengthTypeID();
            if (!lengthType)
            {
                long length = ReadTotalLengthInBits();
                long end = scanner + length;
                while (scanner < end)
                {
                    values.Add(ReadPacket());
                }
            }
            else
            {
                long nrSubPackets = ReadNumberOfSubPackets();
                long counter = nrSubPackets;
                while (counter-- > 0)
                {
                    values.Add(ReadPacket());
                }
            }

            switch (type)
            {
                case 0:
                    return values.Sum();
                case 1:
                    return values.Aggregate(1L, (acc, el) => acc * el);
                case 2:
                    return values.Min();
                case 3:
                    return values.Max();
                case 5:
                    return values.First() > values.Last() ? 1 : 0;
                case 6:
                    return values.First() < values.Last() ? 1 : 0;
                case 7:
                    return values.First() == values.Last() ? 1 : 0;
                default:
                    return -int.MaxValue;
            }
        }

        long ReadPacket()
        {
            long version = ReadVersion();
            long type = ReadTypeID();
            return type == 4 ? ReadLiteralValue() : ReadOperatorPacket(type);
        }

        long result = ReadPacket();

        return new($"Result: {result}");
    }
}