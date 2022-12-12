namespace AdventOfCode.Day12;

public class Challenge : IAdventDay
{
    private static Position Start;
    private static Position End;
    
    public static string Day => "Day12";

    public static string Run(Context ctx)
    {
        var positions = ctx.GetInputIterator("sample.txt")
            .Select(CreateMapFragment)
            .ToArray();

        BuildWalkGraph(Start, positions);

        var paths = FindPaths(Start, End).ToList();

        Console.WriteLine($"Number of paths found: {paths.Count}");
        return $"Shortest Path has {paths.Min(p => p.Steps)} steps";
    }

    // find shortest path in a graph
    private static IEnumerable<Walk> FindPaths(Position start, Position end, Walk? currentPath = null)
    {
        currentPath ??= new Walk(start);

        foreach (var next in start.NextSteps)
            if (next.Equals(end))
            {
                currentPath.Add(end);
                yield return currentPath;
            }
            else
            {
                var newPath = currentPath.Clone();
                if (newPath.Add(next))
                {
                    foreach (var path in FindPaths(next, end, newPath))
                    {
                        yield return path;
                    }
                }
            }
    }

    private static void BuildWalkGraph(Position start, Position[][] positions)
    {
        IEnumerable<Position> ViableNextSteps(Position position)
        {
            if (position.X > 0 && positions[position.Y][position.X - 1].IsReachableFrom(position))
                yield return positions[position.Y][position.X - 1];
            if (position.X < positions[position.Y].Length - 1 &&
                positions[position.Y][position.X + 1].IsReachableFrom(position))
                yield return positions[position.Y][position.X + 1];

            if (position.Y > 0 && positions[position.Y - 1][position.X].IsReachableFrom(position))
                yield return positions[position.Y - 1][position.X];
            if (position.Y < positions.Length - 1 && positions[position.Y + 1][position.X].IsReachableFrom(position))
                yield return positions[position.Y + 1][position.X];
        }

        foreach (var position in positions.SelectMany(x => x))
        {
            position.NextSteps = ViableNextSteps(position).ToArray();
        }
    }

    private static Position[] CreateMapFragment(string line, int yIndex)
        => line.Select((c, xIndex) => new Position(c, xIndex, yIndex)).ToArray();

    internal class Position : IEquatable<Position>
    {
        private readonly char height;
        public readonly int X;
        public readonly int Y;
        private HashSet<Position> blackList = new();

        public Position(char height, int x, int y)
        {
            this.height = height;
            X = x;
            Y = y;

            if (IsStart) Start = this;
            if (IsEnd) End = this;
        }

        public bool IsStart => height == 'S';
        public bool IsEnd => height == 'E';

        public Position[] NextSteps { get; set; }

        private char actualHeight => IsStart ? 'a' : IsEnd ? 'z' : height;

        public bool Equals(Position other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object? obj)
        {
            return obj is Position other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public bool IsReachableFrom(Position position)
        {
            if (blackList.Contains(position))
            {
                return false;
            }
            var reachableFrom = position.actualHeight == actualHeight || position.actualHeight == actualHeight - 1;
            if (reachableFrom)
            {
                // Make sure we never walk back
                blackList.Add(position);
            }
            return reachableFrom;
        }

        public override string ToString() => $"{X},{Y}:{actualHeight}";
    };

    internal class Walk
    {
        private readonly List<Position> positions = new();

        public Walk(Position start)
        {
            positions.Add(start);
        }

        private Walk(IEnumerable<Position> walkSoFar)
        {
            positions = new List<Position>(walkSoFar);
        }

        public int Steps => positions.Count -1; // 1 step less than positions

        public bool Add(Position position)
        {
            if (positions.Contains(position))
            {
                // running in cycles
                return false;
            }
            positions.Add(position);
            return true;
        }

        public Walk Clone() => new(positions);

        public override string ToString() => "Steps: " + Steps + " " + string.Join(" -> ", positions);
    }
}

