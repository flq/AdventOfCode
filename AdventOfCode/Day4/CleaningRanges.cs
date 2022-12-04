using System.Text.RegularExpressions;

namespace AdventOfCode.Day4;

public class CleaningRanges : IAdventDay
{
    public static string Day => "Day4";
    public static string Run(Context ctx)
    {
        var count = ctx.GetInputIterator()
            .Select(line => line.Split(","))
            .Select(rangeInputs =>
            {
                
                var range1 = rangeInputs[0].Split("-");
                var range2 = rangeInputs[1].Split("-");
                return (new CleaningRange(int.Parse(range1[0]), int.Parse(range1[1])),
                    new CleaningRange(int.Parse(range2[0]), int.Parse(range2[1])));
            })
            .Count(ranges => ranges.Item1.IsContainedIn(ranges.Item2) || ranges.Item2.IsContainedIn(ranges.Item1));
        
        return count.ToString();
    }

    record CleaningRange(int LowerBound, int UpperBound)
    {
        public bool IsContainedIn(CleaningRange range) 
            => LowerBound <= range.LowerBound && UpperBound >= range.UpperBound;
    }
}