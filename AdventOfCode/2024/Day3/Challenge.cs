using System.Text.RegularExpressions;

namespace AdventOfCode._2024.Day3;

public partial class Challenge : IAdventDay
{
    private const int DoInstructionLength = 4;
    private const int DontInstructionLength = 7;
    static Regex MulRegex = CreateMulRegex();
    
    public static string Day => "Day3";

    public static string Run(Context ctx)
    {
        var input = ctx.GetInputAsSingleString();

        var isCalculating = true;
        var result = 0;
        foreach (var match in MulRegex.EnumerateMatches(input.Span))
        {
            isCalculating = match.Length switch
            {
                DoInstructionLength => true,
                DontInstructionLength => false,
                _ => isCalculating
            };
            
            // minimum size of a mul is mul(1,1) = 8
            if (isCalculating && match.Length > 7)
            {
                result += PerformCalculation(match, input);
            }
        }
        
        return result.ToString();
    }

    private static int PerformCalculation(ValueMatch match, ReadOnlyMemory<char> input)
    {
        var mul = input.Slice(match.Index, match.Length);
        // mul(\d{1,3},\d{1,3})"
        //     ^^^^^^^ ^^^^^^^
        var comma = mul.Span.IndexOf(',');
        var firstDigit = int.Parse(mul[4..comma].Span);
        var secondDigit = int.Parse(mul[(comma + 1)..^1].Span);
        return firstDigit * secondDigit;
    }

    [GeneratedRegex(@"mul\((\d{1,3}),(\d{1,3})\)|do\(\)|don't\(\)", RegexOptions.Compiled)]
    private static partial Regex CreateMulRegex();
}