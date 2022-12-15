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
        // var join = string.Join("->", result.GetPath().Select(id => graph[id].Item));
        // Console.WriteLine(join);
        
        return $"Shortest Path has {result.Distance} steps";
    }

    private static (Graph<Position, string>, uint startId, uint endId) BuildWalkGraph(Position[][] positions)
    {
        IEnumerable<Position> YieldViablePositions(Position position, IReadOnlySet<Position> avoid)
        {
            if (position.X > 0 && !avoid.Contains(positions[position.Y][position.X - 1])) 
                yield return positions[position.Y][position.X - 1];
            if (position.X < positions[position.Y].Length - 1 && !avoid.Contains(positions[position.Y][position.X + 1])) 
                yield return positions[position.Y][position.X + 1];
            if (position.Y > 0 && !avoid.Contains(positions[position.Y - 1][position.X])) 
                yield return positions[position.Y - 1][position.X];
            if (position.Y < positions.Length - 1 && !avoid.Contains(positions[position.Y + 1][position.X])) 
                yield return positions[position.Y + 1][position.X];
        }

        IEnumerable< (Position position, int distance)> AllPathsToNextHeight(Position position, HashSet<Position> avoid, int distance = 1)
        {
            foreach (var next in YieldViablePositions(position, avoid))
            {
                if (next.Height == position.Height)
                {
                    avoid.Add(position);
                    foreach (var destination in AllPathsToNextHeight(next, avoid, distance + 1))
                    {
                        yield return destination;
                    }
                }
                if (next.Height - position.Height == 1 || (next.Height == position.Height && next.IsEnd))
                {
                    avoid.Remove(position);
                    yield return (next, distance);
                }
            }
        }
        
        var graph = new Graph<Position, string>();
        var visited = new Dictionary<Position, uint>();
        
        uint TryAdd(Position position)
        {
            if (visited.TryGetValue(position, out var id))
                return id;
            id = graph.AddNode(position);
            visited.Add(position, id);
            return id;
        }
        
        var start = positions.SelectMany(x => x).Single(x => x.IsStart);
        var end = positions.SelectMany(x => x).Single(x => x.IsEnd);
        uint startId = TryAdd(start);
        uint endId = TryAdd(end);
        

        var currentHeight = new Stack<Position>(); 
        currentHeight.Push(start);
        var bestPathChooser = new Dictionary<(Position, Position), int>();
        var visitedHeights = new HashSet<char>();

        while (currentHeight.Count > 0 && currentHeight.Peek().Height <= 'z')
        {
            var pos = currentHeight.Pop();
            if (!visitedHeights.Contains(pos.Height))
            {
                visitedHeights.Add(pos.Height);
                Console.WriteLine($"First time on height {pos.Height}");
            }
            
            foreach (var (nextPosition, distance) in AllPathsToNextHeight(pos, new HashSet<Position>()))
            {
                var key = (pos, nextPosition);
                if (bestPathChooser.ContainsKey(key) && bestPathChooser[key] < distance)
                    continue;
                currentHeight.Push(nextPosition);
                    bestPathChooser[key] = distance;
            }
        }
        foreach (var ((from, to), distance) in bestPathChooser)
        {
            var fromId = TryAdd(from);
            var toId = TryAdd(to);
            graph.Connect(fromId, toId, distance, "");
        }
        
        return (graph, startId, endId);
    }

    internal readonly struct Position : IEquatable<Position>
    {
        public static bool operator ==(Position left, Position right) => left.Equals(right);

        public static bool operator !=(Position left, Position right) => !left.Equals(right);

        public static readonly Position None = new Position('0', -1, -1);
        
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

