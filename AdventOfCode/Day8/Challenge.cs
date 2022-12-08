namespace AdventOfCode.Day8;

public abstract class Challenge : IAdventDay
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

public readonly record struct UniqueTree(short Height, bool Visible = false);