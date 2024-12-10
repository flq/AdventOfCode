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
        foreach (var startingPoint in startingPoints)
        {
            var allNiners = LookForNextStep(startingPoint.Point, 1, []);
            totalNinersCount += allNiners.Count;
        }
        
        return $"{totalNinersCount} niners";
        
        HashSet<Point> LookForNextStep(Point point, int levelToLookFor, HashSet<Point> collectedNiners)
        {
            foreach (var neighbour in YieldNeighbours(point))
            {
                if (!lookup.TryGetValue(neighbour, out var elevation) || elevation != levelToLookFor)
                    continue;
                if (levelToLookFor == 9)
                {
                    collectedNiners.Add(neighbour);
                    continue;
                }
                LookForNextStep(neighbour, levelToLookFor + 1, collectedNiners);
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
            .Select((line, y) => (line, y))
            .Select(lineAndY => lineAndY.line.Select((lvl, x) => (elevation: Parse(lvl), x, lineAndY.y)))
            .SelectMany(x => x)
            .Select(data => new ElevationPoint(new Point(data.x, data.y), data.elevation))
            .ToList();
    private static int Parse(char lvl) =>
        lvl switch
        {
            '.' => -1,
            _ => int.Parse([lvl]),
        };
}

internal readonly record struct ElevationPoint(Point Point, int Elevation);