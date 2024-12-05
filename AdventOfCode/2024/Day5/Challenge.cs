namespace AdventOfCode._2024.Day5;

public class Challenge : IAdventDay
{
    public static string Run(Context ctx)
    {
        List<Rule> ruleSet = [];

        var sumOfMiddles = 0;
        foreach (var line in ctx.GetInputIterator())
        {
            if (line == string.Empty) continue;     
            if (line.Contains('|'))
            {
                var (a, b) = line.GetNumberPair('|');
                ruleSet.Add(new Rule(a, b));
                continue;
            }
            
            // rule application starts
            var input = new Input(line);
            
            if (input.Validate(ruleSet))
            {
                sumOfMiddles += input.Middle;
            }
        }
        
        return sumOfMiddles.ToString();
    }
}

public readonly struct Input(string line)
{
    readonly List<int> numbers = line.Split(",").Select(int.Parse).ToList();

    public int Middle => numbers[numbers.Count / 2];
    public bool Validate(IEnumerable<Rule> rules)
    {
        var ints = numbers;
        return rules.All(r => r.Satisfied(ints));
    }
}

public readonly record struct Rule(int Precedes, int Follows)
{
    public bool Satisfied(List<int> numbers)
    {
        var precedesIndex = numbers.IndexOf(Precedes);
        var followsIndex = numbers.IndexOf(Follows);
        if (precedesIndex == -1 || followsIndex == -1) return true; // rule does not apply and is satisfied 
        return precedesIndex >= 0 && followsIndex >= 0 && precedesIndex < followsIndex;
    }
} 