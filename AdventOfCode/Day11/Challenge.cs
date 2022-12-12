using System.Text.RegularExpressions;

namespace AdventOfCode.Day11;

public class Challenge : IAdventDay
{
    public static string Day => "Day11";

    public static string Run(Context ctx)
    {
        var monkeys = ctx.GetInputIterator()
            .Chunk(7)
            .Select(definition => new Monkey(definition))
            .ToList();

        for (int round = 1; round <= 20; round++)
            foreach (var thrownItems in monkeys.Select(monkey => monkey.DoRound().ToList()))
            foreach (var (item, toMonkey) in thrownItems)
                monkeys[toMonkey].Give(item);


        var topMonkeys = monkeys.OrderByDescending(m => m.Count).Take(2).ToList();
        return topMonkeys[0].Count * topMonkeys[1].Count + "";
    }
}

internal partial class Monkey
{
    private readonly List<int> items;
    private readonly Func<int, int> operation;
    private readonly (int whenTrue, int whenFalse) targetMonkeys;
    private readonly int testDivisor;

    public int Count { get; private set; }

    public Monkey(IReadOnlyList<string> definition)
    {
        if (definition.Count < 6)
            throw new ArgumentException("Invalid monkey definition");

        // items with their worry level
        var matches = NumberFinder().Matches(definition[1]);
        items = matches.Select(m => m.Value).Select(int.Parse).ToList();
        // operation
        var opMatches = OperationFinder().Matches(definition[2]);
        operation = opMatches[0].Groups switch
        {
            [_, {Value: "+"}, {Value: "old"}] => x => x + x,
            [_, {Value: "*"}, {Value: "old"}] => x => x * x,
            [_, {Value: "+"}, {Value: var v}] when int.TryParse(v, out var number) => x => x + number,
            [_, {Value: "*"}, {Value: var v}] when int.TryParse(v, out var number) => x => x * number,
            _ => throw new Exception("Unknown operation")
        };
        // testdivisor
        var testMatches = NumberFinder().Matches(definition[3]);
        testDivisor = int.Parse(testMatches[0].Value);
        // target monkeys
        var whenTrue = NumberFinder().Matches(definition[4])[0].Value;
        var whenFalse = NumberFinder().Matches(definition[5])[0].Value;
        targetMonkeys = (whenTrue: int.Parse(whenTrue), whenFalse: int.Parse(whenFalse));
    }

    public IEnumerable<(int Item, int ToMonkey)> DoRound()
    {
        var thisRoundsList = items.ToList();
        foreach (var item in thisRoundsList)
        {
            var newWorryLevel = operation(item) / 3;

            yield return (newWorryLevel,
                newWorryLevel % testDivisor == 0 ? targetMonkeys.whenTrue : targetMonkeys.whenFalse);
            items.RemoveAt(0);
            Count++;
        }
    }

    public void Give(int item) => items.Add(item);

    [GeneratedRegex("(\\d+)", RegexOptions.Compiled)]
    private static partial Regex NumberFinder();

    [GeneratedRegex(@"(\*|\+) (old|\d+)", RegexOptions.Compiled)]
    private static partial Regex OperationFinder();
}