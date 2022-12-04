namespace AdventOfCode.Day4;

public class CleaningRangesOverlaps : IAdventDay
{
    public static string Day => "Day4";

    public static string Run(Context ctx)
    {
        var count = CleaningRanges.GetRanges(ctx)
            .Count(ranges => ranges[0].OverlapsWith(ranges[1]));
        
        return count.ToString();
    }
}