using Dijkstra.NET.Graph;
using Dijkstra.NET.ShortestPath;

namespace AdventOfCode.Day12;

public abstract class Challenge : IAdventDay
{
    public static string Day => "Day12";

    public static string Run(Context ctx)
    {
        var positions = ctx.GetInputIterator()
            .Select((line, yIndex) => line.Select((c, xIndex) => new Position(c, xIndex, yIndex)).ToArray())
            .ToArray();
        
        var (graph, start, end) = BuildWalkGraph(positions);
        var result = graph.Dijkstra(start, end);
        var join = string.Join("->", result.GetPath().Select(id => graph[id].Item));
        Console.WriteLine(join);
        
        return $"Shortest Path has {result.GetPath().Count() - 1} steps";
    }

    private static (Graph<Position, string>, uint startId, uint endId) BuildWalkGraph(Position[][] positions)
    {
        bool IsReachableFrom(Position from, Position to) => to.Height - from.Height is 0 or 1;

        IEnumerable<Position> ViableNextSteps(Position position)
        {
            if (position.X > 0 && IsReachableFrom(position, positions[position.Y][position.X - 1]))
                yield return positions[position.Y][position.X - 1];
            if (position.X < positions[position.Y].Length - 1 &&
                IsReachableFrom(position, positions[position.Y][position.X + 1]))
                yield return positions[position.Y][position.X + 1];

            if (position.Y > 0 && IsReachableFrom(position, positions[position.Y - 1][position.X]))
                yield return positions[position.Y - 1][position.X];
            if (position.Y < positions.Length - 1 && IsReachableFrom(position, positions[position.Y + 1][position.X]))
                yield return positions[position.Y + 1][position.X];
        }
        
        var graph = new Graph<Position, string>();
        uint startId = 0;
        uint endId = 0;
        
        var visited = new Dictionary<Position, uint>();
        
        uint TryAdd(Position position)
        {
            if (visited.TryGetValue(position, out var id))
                return id;
            id = graph.AddNode(position);
            visited.Add(position, id);
            return id;
        }

        foreach (var position in positions.SelectMany(x => x))
        {
            var id = TryAdd(position);
            if (position.IsStart)
                startId = id;
            if (position.IsEnd)
                endId = id;
            foreach (var nextStep in ViableNextSteps(position))
            {
                var nextId = TryAdd(nextStep);
                graph.Connect(id, nextId, 1, string.Empty);
            }
        }

        return (graph, startId, endId);
    }

    internal readonly struct Position : IEquatable<Position>
    {
        private static readonly Position Zero = new Position('0', -1, -1);
        
        private readonly char height;
        public readonly int X;
        public readonly int Y;

        public Position(char height, int x, int y)
        {
            this.height = height;
            X = x;
            Y = y;
        }

        public bool IsStart => height == 'S';
        public bool IsEnd => height == 'E';
        public char Height => IsStart ? 'a' : IsEnd ? 'z' : height;
        
        public bool Equals(Position other) => X == other.X && Y == other.Y;

        public override bool Equals(object? obj) => obj is Position other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(X, Y);

        public override string ToString() => $"{X},{Y}:{Height}";
    };
}

