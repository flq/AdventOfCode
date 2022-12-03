namespace AdventOfCode.Day1;

public abstract class ElveWithMostCalories : IAdventDay
{
    public static string Day => "Day1";

    public static string Run(Context ctx)
    {
        IEnumerable<(string Name, int Calories)> Iterate()
        {
            var currentElve = 1;
            var currentCalories = 0;
            foreach (var line in ctx.GetInputIterator())
            {
                if (line is "")
                {
                    yield return (currentElve.ToString(), currentCalories);
                    currentElve++;
                    currentCalories = 0;
                }
                else if (int.TryParse(line, out var calories))
                {
                    currentCalories += calories;
                }
            }
        }

        return string.Join(", ", Iterate().OrderByDescending(elve => elve.Calories).Take(3).Select(e => e.Calories).Sum());
    }
}