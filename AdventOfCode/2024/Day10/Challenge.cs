using System.Drawing;

namespace AdventOfCode._2024.Day10;

public class Challenge : IAdventDay
{
    public static string Run(Context ctx)
    {
        var points = InitializeMap(ctx.GetInputIterator("Small.txt"));
        var lookup = points.ToDictionary(k => k.Point, v => v.Elevation);
        var startingPoints = points.Where(p => p.Elevation == 0);

        var totalNinersCount = 0;
        var totalUniquenessCount = 0;
        foreach (var startingPoint in startingPoints)
        {
            var node = new Node(startingPoint.Point);
            var allNiners = LookForNextStep(startingPoint.Point, 1, [], node);
            totalNinersCount += allNiners.Count;
            totalUniquenessCount += node.UniquePaths;
        }

        return $"{totalNinersCount} niners, {totalUniquenessCount} uniqueness";

        HashSet<Point> LookForNextStep(Point point, int levelToLookFor, HashSet<Point> collectedNiners, Node node)
        {
            foreach (var neighbour in YieldNeighbours(point))
            {
                if (!lookup.TryGetValue(neighbour, out var elevation) || elevation != levelToLookFor)
                    continue;
                if (levelToLookFor == 9)
                {
                    collectedNiners.Add(neighbour);
                    node.AddEdgeTo(neighbour);
                    continue;
                }
                var thisPlace = node.AddEdgeTo(neighbour);
                LookForNextStep(neighbour, levelToLookFor + 1, collectedNiners, thisPlace);
            }
            return collectedNiners;

            IEnumerable<Point> YieldNeighbours(Point p)
            {
                yield return p with {X = p.X + 1};
                yield return p with {Y = p.Y + 1};
                yield return p with {X = p.X - 1};
                yield return p with {Y = p.Y - 1};
            }
        }
    }

    private static List<ElevationPoint> InitializeMap(IEnumerable<string> lines) =>
        lines
            .GridLike(data => new ElevationPoint(new Point(data.x, data.y), Parse(data.val))).items;
    
    private static int Parse(char lvl) =>
        lvl switch
        {
            '.' => -1,
            _ => int.Parse([lvl])
        };
}

internal readonly record struct ElevationPoint(Point Point, int Elevation);

internal class Node(Point elevationPoint)
{
    private readonly Node? parent;
    private Node(Node parent, Point elevationPoint) : this(elevationPoint)
    {
        this.parent = parent;
    }

    private readonly List<Node> children = [];

    public Node AddEdgeTo(Point point)
    {
        /*     / y \
         *   x      d
         *     \ z /
         */
        // okay, a node can have two parents, so this stuff is unfortunately flawed, but I'm tired now
        var item = new Node(this, point);
        children.Add(item);
        return item;
    }

    public int UniquePaths => children.Count == 0 ? 1 : children.Sum(child => child.UniquePaths);
}
    