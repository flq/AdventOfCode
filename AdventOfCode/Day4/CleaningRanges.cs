namespace AdventOfCode.Day4;

public class CleaningRanges : IAdventDay
{
    public static string Day => "Day4";

    public static string Run(Context ctx)
    {
        var count = GetRanges(ctx)
            .Count(ranges => ranges[1].IsAnyContainedInTheOther(ranges[0]));

        return count.ToString();
    }

    /// <summary>
    ///     Gives the ranges contained in a single line. The ranges are already sorted
    /// </summary>
    public static IEnumerable<List<CleaningRange>> GetRanges(Context ctx)
    {
        return ctx.GetInputIterator()
            .Select(line => line.GetPair(','))
            .Select(pair =>
            {
                var (lower1, upper1) = pair.left.GetNumberPair('-');
                var (lower2, upper2) = pair.right.GetNumberPair('-');
                return lower1 <= lower2
                        ? new List<CleaningRange> {new(lower1, upper1), new(lower2, upper2)}
                        : new List<CleaningRange> {new(lower2, upper2), new(lower1, upper1)}
                    ;
            });
    }

    public record CleaningRange(int LowerBound, int UpperBound)
    {
        public bool IsAnyContainedInTheOther(CleaningRange range)
            => (LowerBound <= range.LowerBound && UpperBound >= range.UpperBound) ||
               (range.LowerBound <= LowerBound && range.UpperBound >= UpperBound);

        public bool OverlapsWith(CleaningRange range)
            => LowerBound <= range.LowerBound && UpperBound >= range.LowerBound;
    }
}