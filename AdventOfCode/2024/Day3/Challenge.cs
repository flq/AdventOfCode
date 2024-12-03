using System.Text.RegularExpressions;

namespace AdventOfCode._2024.Day3;

public partial class Challenge : IAdventDay
{
    static Regex MulRegex = CreateMulRegex();
    
    public static string Day => "Day3";

    public static string Run(Context ctx)
    {
        var input = ctx.GetInputAsSingleString();

        var result = 0;
        foreach (var match in MulRegex.EnumerateMatches(input.Span))
        {
            result += PerformCalculation(match, input);
        }
        return result.ToString();
    }

    private static int PerformCalculation(ValueMatch match, ReadOnlyMemory<char> input)
    {
        var mul = input.Slice(match.Index, match.Length);
        var comma = mul.Span.IndexOf(',');
        // skip "mul(," up to comma 
        var firstDigit = int.Parse(mul[4..comma].Span);
        var secondDigit = int.Parse(mul[(comma + 1)..^1].Span);
        return firstDigit * secondDigit;
    }

    [GeneratedRegex(@"mul\((\d{1,3}),(\d{1,3})\)", RegexOptions.Compiled)]
    private static partial Regex CreateMulRegex();
}