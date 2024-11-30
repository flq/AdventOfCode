namespace AdventOfCode;

public interface IAdventDay
{
    static abstract string Day { get; }
    static abstract string Run(Context ctx);
}

public class Context(string day)
{
    private const string Year = "2024";

    public IEnumerable<string> GetInputIterator(string fileName = "input.txt")
    {
        using var f = File.OpenRead(Path(fileName));
        using var sr = new StreamReader(f);
        while (sr.ReadLine() is { } line) yield return line;
    }

    public ReadOnlyMemory<byte> GetInputAsMemory(string fileName = "input.txt") => 
        new(File.ReadAllBytes(Path(fileName)));

    private string Path(string fileName) => 
        System.IO.Path.Combine("..", "..", "..", Year, day, fileName);
}