using static System.Math;

namespace AdventOfCode.Day9;

public class Challenge : IAdventDay
{
    public static string Day => "Day9";

    public static string Run(Context ctx)
    {
        var listOfCommands = ctx.GetInputIterator().Select(CreateCommand).ToList();
        var uniquePositions = EnumerateAllPositions(listOfCommands).Distinct().Count();
        return "Unique positions: " + uniquePositions;
    }

    private static IEnumerable<Position> EnumerateAllPositions(IEnumerable<Command> commands)
    {
        var headPosition = new Position(0, 0);
        var tailPosition = new Position(0, 0);
        foreach (var direction in commands.SelectMany(c => c.SingleSteps()))
        {
            var previousHeadPosition = headPosition;
            headPosition = direction switch
            {
                Direction.R => headPosition.MoveRight(),
                Direction.L => headPosition.MoveLeft(),
                Direction.U => headPosition.MoveUp(),
                Direction.D => headPosition.MoveDown(),
                _ => headPosition
            };
            tailPosition = tailPosition.MoveTowards(previousHeadPosition, headPosition);
            yield return tailPosition;
        }
    }

    private static Command CreateCommand(string line) =>
        line.Split(" ") switch
        {
            [var directionStr, var distanceStr] when Enum.TryParse<Direction>(directionStr, out var direction) &&
                                                     int.TryParse(distanceStr, out var distance) => new Command(
                direction, distance),
            _ => throw new ArgumentException("Invalid command with line " + line)
        };
}

internal readonly record struct Position(int X, int Y)
{
    public Position MoveRight() => this with {X = X + 1};
    public Position MoveLeft() => this with {X = X - 1};
    public Position MoveUp() => this with {Y = Y + 1};
    public Position MoveDown() => this with {Y = Y - 1};

    public Position MoveTowards(Position previousHeadPosition, Position headPosition)
    {
        if (this == headPosition)
            return this;

        var (hX, hY) = headPosition;
        if (hX == X)
        {
            if (Abs(Y - hY) > 1)
                return hY - Y > 0 ? MoveUp() : MoveDown();
        }
        else if (hY == Y)
        {
            if (Abs(X - hX) > 1)
                return hX - X > 0 ? MoveRight() : MoveLeft();
        }
        else if ((Abs(Y - hY) == 2 && Abs(X - hX) == 1) || (Abs(X - hX) == 2 && Abs(Y - hY) == 1))
        {
            return previousHeadPosition;
        }

        return this;
    }
}

internal readonly record struct Command(Direction Direction, int Distance)
{
    public IEnumerable<Direction> SingleSteps()
    {
        for (var _ = 0; _ < Distance; _++)
            yield return Direction;
    }
}

internal enum Direction
{
    U,
    D,
    L,
    R
}
