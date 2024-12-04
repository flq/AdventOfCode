namespace AdventOfCode._2024.Day4;

public class Challenge : IAdventDay
{
    public static string Day => "Day4";

    public static string Run(Context ctx)
    {
        var m = ctx.GetInputAsSingleString();
        var lineLength = m.Span.IndexOf(Environment.NewLine);
        Console.WriteLine("lineLength: " + lineLength);
        var checker = new Checker(lineLength, m);

        var currentIndex = 0;
        var sum = 0;
        while (currentIndex < m.Length)
        {
            var nextIndex = m.Slice(currentIndex).Span.IndexOf("X");
            if (nextIndex == -1) break;
            sum += checker.NumberOfXMases(currentIndex + nextIndex);
            currentIndex += nextIndex + 1;
        }

        return sum.ToString();
    }
}

public class Checker(int lineLength, ReadOnlyMemory<char> grid)
{
    private readonly int λ = lineLength + 1; // +1 from the new-line

    public int NumberOfXMases(int x)
    {
        var λPlusOne = λ + 1;
        var λMinusOne = λ - 1;
        return
            Check(x, x + 1, x + 2, x + 3) + // Horizontal ->
            Check(x, x - 1, x - 2, x - 3) + // Horizontal <-
            Check(x, x - λ, x - 2 * λ, x - 3 * λ) + // Vertical up
            Check(x, x + λ, x + 2 * λ, x + 3 * λ) + // Vertical down
            Check(x, x - λPlusOne, x - 2 * λPlusOne, x - 3 * λPlusOne) + // Diag up left  
            Check(x, x - λMinusOne, x - 2 * λMinusOne, x - 3 * λMinusOne) + // Diag up right  
            Check(x, x + λMinusOne, x + 2 * λMinusOne, x + 3 * λMinusOne) + // Diag down left  
            Check(x, x + λPlusOne, x + 2 * λPlusOne, x + 3 * λPlusOne); // Diag down right  
    }

    private int Check(int xx, int mm, int aa, int ss)
    {
        var span = grid.Span;
        if (xx < 0 || xx >= span.Length) return 0;
        if (mm < 0 || mm >= span.Length) return 0;
        if (aa < 0 || aa >= span.Length) return 0;
        if (ss < 0 || ss >= span.Length) return 0;

        var (x, m, a, s) = (span[xx], span[mm], span[aa], span[ss]);

        return x == 'X' && m == 'M' && a == 'A' && s == 'S' ? 1 : 0;
    }
}