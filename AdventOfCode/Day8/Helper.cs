namespace AdventOfCode.Day8;

public enum Direction
{
    LeftOrDown,
    RightOrUp
}

public enum Orientation
{
    Row,
    Column
}

public static class Helper
{
    public static IEnumerable<(bool reset, T tree, int row, int col)> Iterate<T>(this T[][] trees,
        Orientation orientation, Direction direction)
    {
        // Assuming it's a square
        var length = trees.Length;

        switch (direction)
        {
            case Direction.LeftOrDown:
                for (var a = 0; a < length; a++)
                for (var b = 0; b < length; b++)
                {
                    var (c, d) = orientation == Orientation.Row ? (a, b) : (b, a);
                    yield return new(b == length - 1, trees[c][d], c, d);
                }

                break;
            case Direction.RightOrUp:
                for (var a = length - 1; a >= 0; a--)
                for (var b = length - 1; b >= 0; b--)
                {
                    var (c, d) = orientation == Orientation.Row ? (a, b) : (b, a);
                    yield return new(b == 0, trees[c][d], c, d);
                }

                break;
        }
    }
}