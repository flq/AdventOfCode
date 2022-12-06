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

    private string Path => System.IO.Path.Combine("..", "..", "..", _day, "input.txt");

    public IEnumerable<string> GetInputIterator()
    {
        using var f = File.OpenRead(Path);
        using var sr = new StreamReader(f);
        while (sr.ReadLine() is { } line) yield return line;
    }

    public ReadOnlyMemory<byte> GetInputAsMemory()
    {
        var file = File.ReadAllBytes(Path);
        return new ReadOnlyMemory<byte>(file);
    }
}