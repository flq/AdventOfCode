namespace AdventOfCode;

public static class Tools
{
    public static (string left, string right) GetPair(this string input, char separator) =>
        input.Split(separator) switch
        {
            [{ } left, { } right] => (left, right),
            _ => throw new ArgumentException("Bad input " + input)
        };

    public static (int left, int right) GetNumberPair(this string input, char separator)
    {
        var (left, right) = input.GetPair(separator);
        return (int.Parse(left), int.Parse(right));
    }

    public static (List<T> items, Size size) GridLike<T>(this IEnumerable<string> lines, Func<(char val, int x, int y), T> builder)
    {
        List<T> result = []; 
        var y = 0;
        var width = 0;
        foreach (var line in lines)
        {
            width = line.Length;
            for (var x = 0; x < line.Length; x++)
            {
                builder((line[x], x,  y));
            }
            y++;
        }
        
        return (result, new Size(width, y));
    }
}

public readonly record struct Point(int X, int Y)
{
    public IEnumerable<Point> YieldNeighbours()
    {
        yield return this with {X = X - 1};
        yield return this with {X = X + 1};
        yield return this with {Y = Y - 1};
        yield return this with {Y = Y + 1};
    }
    
    public static readonly Point Empty = new (0, 0);
}

public readonly record struct Size(int Width, int Height)
{
    public bool IsOutside(Point p) =>
        p.X < 0 || p.X >= Width || p.Y < 0 || p.Y >= Height;
    public bool IsInside(Point p) => !IsOutside(p);
};

