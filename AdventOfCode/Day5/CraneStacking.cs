using System.Text.RegularExpressions;

namespace AdventOfCode.Day5;

public partial class CraneStacking : IAdventDay
{
    private static readonly Stack<string>[] Columns =
    {
        Col("B V W T Q N H D"),
        Col("B W D"),
        Col("C J W Q S T"),
        Col("P T Z N R J F"),
        Col("T S M J V P G"),
        Col("N T F W B"),
        Col("N V H F Q D L B"),
        Col("R F P H"),
        Col("H P N L B M S Z"),
    };

    private static readonly Regex FindCommandData = CommandDataFinder();
    public static string Day => "Day5";

    public static string Run(Context ctx)
    {
        var craneCommands = ctx.GetInputIterator().Skip(10)
            .Select(ToCraneCommand)
            .ToList();

        foreach (var craneCommand in craneCommands)
            craneCommand.Apply(Columns);

        return string.Join("", Columns.Select(s => s.Peek()));
    }

    /// <summary>
    ///     When copied out the order is reversed, so we add them reversed to the stack
    /// </summary>
    private static Stack<string> Col(string items) => new(items.Split(" ").Reverse());

    private static CraneCommand ToCraneCommand(string arg) =>
        FindCommandData.Matches(arg) switch
        {
            [{Value: var howMany}, {Value: var from}, {Value: var to}] =>
                new CraneCommand(int.Parse(from), int.Parse(to), int.Parse(howMany)),
            _ => throw new ArgumentException($"line {arg} did not match expectation")
        };

    [GeneratedRegex("(\\d+)", RegexOptions.Compiled)]
    private static partial Regex CommandDataFinder();
}

internal record CraneCommand(int From, int To, int HowMany)
{
    public void Apply(Stack<string>[] columns)
    {
        var howMany = HowMany;
        while (howMany > 0)
        {
            columns[To - 1].Push(columns[From - 1].Pop());
            howMany--;
        }
    }
}