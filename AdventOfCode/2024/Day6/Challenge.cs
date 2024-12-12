namespace AdventOfCode._2024.Day6;

public class Challenge : IAdventDay
{
    public static string Run(Context ctx)
    {
        var context = InitializeGridAndObstaclesAndGuard(ctx);

        HashSet<Point> visitedPlaces = [context.GuardInitialPosition];
        var guard = new Guard(context.GuardInitialPosition);

        while (!guard.Exited)
            visitedPlaces.UnionWith(guard.Move(context));

        return visitedPlaces.Count.ToString();
    }
    private static ChallengeContext InitializeGridAndObstaclesAndGuard(Context ctx)
    {
        HashSet<Point> obstacles = new();
        var guard = Point.Empty;
        var yCoord = 0;
        var width = 0;
        foreach (var line in ctx.GetInputIterator())
        {
            width = line.Length;
            for (var i = 0; i < line.Length; i++)
            {
                switch (line[i])
                {
                    case '#':
                        obstacles.Add(new Point(i, yCoord));
                        break;
                    case '^':
                        guard = new Point(i, yCoord);
                        break;
                }
            }
            yCoord++;
        }
        return new ChallengeContext(new Size(width, yCoord), guard, obstacles);
    }
}

internal class Guard(Point position)
{
    private Orientation orientation = Orientation.Up;
    private Point position = position;
    
    public bool Exited { get; private set; }
    
    public IEnumerable<Point> Move(ChallengeContext context)
    {
        while (true)
        {
            var p = GetNext();
            if (context.Obstacles.Contains(p))
            {
                Turn();
                yield break;
            }
            
            if (context.Grid.IsOutside(p))
            {
                Exited = true;
                yield break;
            }
            position = p;
            yield return p;
        }
    }
    
    private Point GetNext() =>
        orientation switch
        {
            Orientation.Up => position with { Y = position.Y - 1 },
            Orientation.Down => position with { Y = position.Y + 1 },
            Orientation.Left => position with { X = position.X - 1 },
            Orientation.Right => position with { X = position.X + 1 },
            _ => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null)
        };
    private void Turn()
    {
        orientation = orientation switch
        {
            Orientation.Up => Orientation.Right,
            Orientation.Right => Orientation.Down,
            Orientation.Down => Orientation.Left,
            Orientation.Left => Orientation.Up,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}

internal record struct ChallengeContext(Size Grid, Point GuardInitialPosition, HashSet<Point> Obstacles);

internal enum Orientation
{
    Up,
    Down,
    Left,
    Right
}