namespace AdventOfCode._2024.Day4;

public class Challenge : IAdventDay
{
    public static string Day => "Day4";

    public static string Run(Context ctx)
    {
        var m = ctx.GetInputAsSingleString();
        var lineLength = m.Span.IndexOf(Environment.NewLine);
        var checker = new Checker(lineLength, m);

        var currentIndex = 0;
        var sum = 0;
        while (currentIndex < m.Length)
        {
            var nextIndex = m[currentIndex..].Span.IndexOf("X");
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

    private int Check(int x, int m, int a, int s)
    {
        var span = grid.Span;

        foreach (var index in new Span<int>([x, m, a, s]))
        {
            // the x cannot be out of bounds
            if (index < 0 || index >= span.Length) return 0;
        }

        char[] candidate = [span[x], span[m], span[a], span[s]];

        return candidate switch
        {
            ['X', 'M', 'A', 'S'] => 1,
            _ => 0
        };
    }
}