namespace AdventOfCode._2024.Day5;

public class Challenge : IAdventDay
{
    public static string Run(Context ctx)
    {
        List<Rule> ruleSet = [];

        var sumOfMiddlesValid = 0;
        var sumOfMiddlesFixed = 0;
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
            if (input.IsValid(ruleSet))
            {
                sumOfMiddlesValid += input.Middle;
            }
            else
            {
                input.CorrectWith(ruleSet);
                sumOfMiddlesFixed += input.Middle;
            }
        }
        
        return $"valids: {sumOfMiddlesValid}, fixed: {sumOfMiddlesFixed}";
    }
}

public readonly struct Input(string line)
{
    private readonly List<int> pages = line.Split(",").Select(int.Parse).ToList();

    public int Middle => pages[pages.Count / 2];
    
    public bool IsValid(IEnumerable<Rule> rules)
    {
        var localPages = pages;
        return rules.All(r => r.SatisfiedBy(localPages));
    }
    
    public void CorrectWith(List<Rule> ruleSet)
    {
        var localPages = pages;
        fixNext:
        var unsatisfied = ruleSet.FirstOrDefault(r => !r.SatisfiedBy(localPages));
        if (unsatisfied != default)
        {
            unsatisfied.FixToSatisfy(localPages);
            goto fixNext;
        }
    }
}

public readonly record struct Rule(int Precedes, int Follows)
{
    public bool SatisfiedBy(List<int> numbers)
    {
        var (precedesIndex, followsIndex) = FindMatches(numbers);
        if (precedesIndex == -1 || followsIndex == -1) return true; // rule does not apply and hence satisfied 
        return precedesIndex >= 0 && followsIndex >= 0 && precedesIndex < followsIndex;
    }
    public void FixToSatisfy(List<int> pages)
    {
        var (precedesIdx, followsIdx) = FindMatches(pages);
        (pages[precedesIdx], pages[followsIdx]) = (pages[followsIdx], pages[precedesIdx]);
    }
    
    private (int precedes, int follows) FindMatches(List<int> numbers)
    {
        var precedesIndex = numbers.IndexOf(Precedes);
        var followsIndex = numbers.IndexOf(Follows);
        return (precedesIndex, followsIndex);
    }
} 