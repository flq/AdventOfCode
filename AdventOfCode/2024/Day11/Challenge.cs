using System.Numerics;

namespace AdventOfCode._2024.Day11;

public class Challenge : IAdventDay
{
    public static string Run(Context ctx)
    {
        Queue<Stone> stones = new(
        new string(ctx.GetInputAsMemoryChar().Span)
            .Split(" ")
            .Select(x => new Stone(x.EndsWith("\n") ? x[..^1] : x )));
        
        var loops = 75;
        while (loops > 0)
        {
            Console.WriteLine(loops);
            Queue<Stone> nextLine = new();
            while (stones.Count > 0)
            {
                var stone = stones.Dequeue();
                var nextStones = stone.ApplyRules();
                foreach (var nextStone in nextStones)
                {
                    nextLine.Enqueue(nextStone);
                }
            }
            stones = nextLine;
            loops--;
        }
        return $"Stones: {stones.Count}";
    }
}

public readonly record struct Stone(string Raw)
{
    public IEnumerable<Stone> ApplyRules()
    {
        switch (Raw)
        {
            case "0":
                yield return new Stone("1");
                break;
            case var _ when Raw.Length % 2 == 0:
                yield return new Stone(Raw[..(Raw.Length / 2)]);
                var secondHalf = Raw[(Raw.Length / 2)..].TrimStart('0');
                yield return new Stone(secondHalf.Length == 0 ? "0" : secondHalf);
                break;
            default:
                var value = BigInteger.Parse(Raw);
                yield return new Stone(BigInteger.Multiply(value, 2024).ToString());
                break;
        }
    }
}