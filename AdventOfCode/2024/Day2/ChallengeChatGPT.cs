namespace AdventOfCode._2024.Day2;

public class ChallengeChatGpt : IAdventDay
{
    public static string Day => "Day2";

    public static string Run(Context ctx)
    {
        int safeReports = 0;
        
        foreach (var line in ctx.GetInputIterator())
        {
            var levels = Array.ConvertAll(line.Split(' '), int.Parse);

            if (IsSafeReport(levels))
                safeReports++;
        }
        return safeReports.ToString();
    }
    
    static bool IsSafeReport(int[] levels)
    {
        bool isIncreasing = true, isDecreasing = true;

        for (int i = 1; i < levels.Length; i++)
        {
            int diff = levels[i] - levels[i - 1];

            if (diff < 1 || diff > 3)
                return false;

            if (diff > 0)
                isDecreasing = false;
            if (diff < 0)
                isIncreasing = false;
        }

        return isIncreasing || isDecreasing;
    }
}