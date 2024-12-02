namespace AdventOfCode._2024.Day2;

public class Challenge : IAdventDay
{
    public static string Day => "Day2";

    public static string Run(Context ctx)
    {
        return ctx.GetInputIterator()
            .Select(line => new Report(line.Split(" ").Select(int.Parse)))
            .Count(r => r.IsSafe)
            .ToString();
    }

    private readonly struct Report(params IEnumerable<int> levels)
    {
        public bool IsSafe => levels.GenerateWithOmissions().Any(CheckIfLevelsAreSafe);

        private static bool CheckIfLevelsAreSafe(IEnumerable<int> levelsToCheck)
        {
            int? previousValue = null;
            bool? increasing = null;
            foreach (var level in levelsToCheck)
            {
                if (!previousValue.HasValue)
                {
                    previousValue = level;
                    continue;
                }

                increasing ??= level > previousValue;
                    
                var delta = level - previousValue.Value;
                    
                var isSafe = Math.Abs(delta) is >= 1 and <= 3 && 
                             ((increasing.Value && delta > 0) || (!increasing.Value && delta < 0));
                    
                if (!isSafe) return false;
                previousValue = level;
            }

            return true;
        }
    }
}

file static class Helper
{
    /// <summary>
    ///     This helper was provided by ChatGPT with the following prompt:
    ///     For an enumerable of items I want a method that produces an enumeration over multiple versions of those
    ///     items where in each iteration one of the items is missing. As an example, for 
    ///     [1,2,3] I want back:
    ///     [1, 2, 3],
    ///     [2, 3],
    ///     [1, 3],
    ///     [1, 2]
    ///     note the original list is also part. The order in which the different lists are produced is irrelevant
    ///     The only change made was the rider warning with regard to the captured variable. However
    ///     in this particular case it isn't strictly necessary.
    /// </summary>
    public static IEnumerable<IEnumerable<T>> GenerateWithOmissions<T>(this IEnumerable<T> source)
    {
        var list = source.ToList();
        yield return list; // Include the original list

        for (var i = 0; i < list.Count; i++)
        {
            var capture = i;
            yield return list.Where((_, index) => index != capture);
        }
    }
}