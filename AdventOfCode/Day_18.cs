using AoCHelper;

namespace AdventOfCode;

public class Day_18 : BaseDay
{
    private readonly string[] _input;

    public Day_18()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        Node root = null;
        foreach (string line in _input)
        {
            if (root == null)
            {
                root = Generator(line);
                Reduce(root);
                continue;
            }

            root = root.Add(Generator(line));
            Reduce(root);
        }

        return new($"Magnitude: {root!.Magnitude()}");
    }

    public override ValueTask<string> Solve_2()
    {
        int maxMagnitude = 0;
        for (int i = 0; i < _input.Length; i++)
        for (int j = 0; j < _input.Length; j++)
        {
            if (i == j) continue;
            Node root = Generator(_input[i]).Add(Generator(_input[j]));
            Reduce(root);
            int magnitude = root.Magnitude();
            if (magnitude > maxMagnitude) maxMagnitude = magnitude;
        }

        return new($"Max magnitude: {maxMagnitude}");
    }

    abstract class Node
    {
        public Pair Parent { get; set; }
        public abstract Pair FindLeftMostNestedPair(int level);
        public abstract Number FindLeftMostHighNumber();
        public abstract Number FindNumberLeftOf(Node source);
        public abstract Number FindNumberRightOf(Node source);
        public abstract Number FindLeftMostNumber();
        public abstract Number FindRightMostNumber();
        public abstract int Magnitude();

        public Pair Add(Node otherNode)
        {
            Pair pair = new Pair(this, otherNode);
            this.Parent = pair;
            otherNode.Parent = pair;
            return pair;
        }
    }

    class Number : Node
    {
        public Number(int value)
        {
            Value = value;
        }

        public int Value { get; set; }

        public override Pair FindLeftMostNestedPair(int level) => null;
        public override Number FindLeftMostHighNumber() => Value >= 10 ? this : null;
        public override Number FindNumberLeftOf(Node source) => this;
        public override Number FindNumberRightOf(Node source) => this;
        public override Number FindLeftMostNumber() => this;
        public override Number FindRightMostNumber() => this;
        public override int Magnitude() => Value;

        public void Split()
        {
            int left = Value / 2;
            int right = Value - left;
            Pair pair = new Pair(new Number(left), new Number(right));
            pair.Parent = Parent;
            pair.Left.Parent = pair;
            pair.Right.Parent = pair;
            if (Parent.Left == this) Parent.Left = pair;
            if (Parent.Right == this) Parent.Right = pair;
        }
    }

    class Pair : Node
    {
        public Pair(Node left, Node right)
        {
            Left = left;
            Right = right;
        }

        public Node Left { get; set; }
        public Node Right { get; set; }

        public override Pair FindLeftMostNestedPair(int level)
        {
            if (level == 0) return this;
            Pair found = Left.FindLeftMostNestedPair(level - 1);
            if (found != null) return found;
            found = Right.FindLeftMostNestedPair(level - 1);
            return found;
        }

        public override Number FindLeftMostHighNumber()
        {
            Number found = Left.FindLeftMostHighNumber();
            if (found != null) return found;
            found = Right.FindLeftMostHighNumber();
            return found;
        }

        public override Number FindLeftMostNumber()
            => Left.FindLeftMostNumber();

        public override Number FindRightMostNumber()
            => Right.FindRightMostNumber();

        public override int Magnitude()
            => 3 * Left.Magnitude() + 2 * Right.Magnitude();

        public override Number FindNumberLeftOf(Node source)
        {
            if (source == Right) return Left.FindRightMostNumber();
            return Parent?.FindNumberLeftOf(this);
        }

        public override Number FindNumberRightOf(Node source)
        {
            if (source == Left) return Right.FindLeftMostNumber();
            return Parent?.FindNumberRightOf(this);
        }

        public void Explode()
        {
            Number leftNumber = Parent.FindNumberLeftOf(this);
            if (leftNumber != null) leftNumber.Value += ((Number)Left).Value;
            Number rightNumber = Parent.FindNumberRightOf(this);
            if (rightNumber != null) rightNumber.Value += ((Number)Right).Value;
            Number zero = new Number(0);
            zero.Parent = Parent;
            if (Parent.Left == this) Parent.Left = zero;
            if (Parent.Right == this) Parent.Right = zero;
        }
    }

    static Node Generator(string line)
    {
        char[] characters = line.ToCharArray();
        int scanner = 0;

        Node ReadThing()
        {
            char c = characters[scanner++];
            if (c is >= '0' and <= '9') return new Number(c - '0');
            if (c is '[')
            {
                Node left = ReadThing();
                c = characters[scanner++]; // this should be ','
                Node right = ReadThing();
                c = characters[scanner++]; // this should be ']'
                Pair pair = new Pair(left, right);
                left.Parent = pair;
                right.Parent = pair;
                return pair;
            }

            return null; // should not happen
        }

        return ReadThing();
    }

    static void Reduce(Node root)
    {
        bool reduced = false;
        do
        {
            Pair nestedPair = root.FindLeftMostNestedPair(4);
            if (nestedPair != null)
            {
                reduced = true;
                nestedPair.Explode();
                continue;
            }

            Number highNumber = root.FindLeftMostHighNumber();
            if (highNumber != null)
            {
                reduced = true;
                highNumber.Split();
                continue;
            }

            reduced = false;
        } while (reduced);
    }
}