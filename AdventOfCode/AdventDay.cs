namespace AdventOfCode;

public interface AdventDay
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

    public string GetPath(string filename) => Path.Combine("..", "..", "..", _day, filename);
}