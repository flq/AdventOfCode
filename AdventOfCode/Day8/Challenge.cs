namespace AdventOfCode.Day8;

public class Challenge : IAdventDay
{
    public static string Day => "Day8";

    public static string Run(Context ctx)
    {
        var uniqueTrees = ctx.GetInputIterator().Select(ToTreeLine).ToArray();
        
        Console.WriteLine($"Rows: {uniqueTrees.Length}, Columns: {uniqueTrees[0].Length}");
        
        void MarkTreesAsVisible(IEnumerable<(bool reset, UniqueTree tree, int row, int col)> linesOfTrees)
        {
            var referenceHeight = -1;
            foreach (var (reset, tree, row, col) in linesOfTrees)
            {
                if (tree.Height > referenceHeight)
                {
                    uniqueTrees[row][col] = tree with {Visible = true};
                    referenceHeight = tree.Height;
                }

                if (reset)
                {
                    referenceHeight = -1;
                }
            }
        }
        
        MarkTreesAsVisible(uniqueTrees.Iterate(Orientation.Row, Direction.LeftOrDown));
        MarkTreesAsVisible(uniqueTrees.Iterate(Orientation.Column, Direction.LeftOrDown));
        MarkTreesAsVisible(uniqueTrees.Iterate(Orientation.Row, Direction.RightOrUp));
        MarkTreesAsVisible(uniqueTrees.Iterate(Orientation.Column, Direction.RightOrUp));

        // How many trees are still visible?
        var visibleTrees = (from treeLine in uniqueTrees
            select treeLine.Count(t => t.Visible)).Sum();
        
        return visibleTrees.ToString();
    }

    private static UniqueTree[] ToTreeLine(string line) =>
        line.Select(heightStr => new UniqueTree(short.Parse(heightStr.ToString())))
            .ToArray();
}

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

public readonly record struct UniqueTree(short Height, bool Visible = false);

public static class Helper
{
    public static IEnumerable<(bool reset, UniqueTree tree, int row, int col)> Iterate(this UniqueTree[][] trees, Orientation orientation, Direction direction)
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
                for (var a = trees.Length - 1; a >= 0; a--)
                for (var b = trees.Length - 1; b >= 0; b--)
                {
                    var (c, d) = orientation == Orientation.Row ? (a, b) : (b, a);
                    yield return new(b == 0, trees[c][d], c, d);
                }
                break;
        }
    }
}