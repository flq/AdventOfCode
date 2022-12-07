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

    private string Path(string fileName) => System.IO.Path.Combine("..", "..", "..", _day, fileName);

    public IEnumerable<string> GetInputIterator(string fileName = "input.txt")
    {
        using var f = File.OpenRead(Path(fileName));
        using var sr = new StreamReader(f);
        while (sr.ReadLine() is { } line) yield return line;
    }

    public ReadOnlyMemory<byte> GetInputAsMemory(string fileName = "input.txt")
    {
        var file = File.ReadAllBytes(Path(fileName));
        return new ReadOnlyMemory<byte>(file);
    }
}