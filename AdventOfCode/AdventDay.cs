namespace AdventOfCode;

public interface IAdventDay
{
    static abstract string Day { get; }
    static abstract string Run(Context ctx);
}

public class Context
{
    private readonly string _day;

    public Context(string day)
    {
        _day = day;
    }

    public IEnumerable<string> GetIterator(string filename)
    {
        var path = Path.Combine("..", "..", "..", _day, filename);
        using var f = File.OpenRead(path);
        using var sr = new StreamReader(f);
        while (sr.ReadLine() is { } line)
        {
            yield return line;
        }
    }
}