namespace AdventOfCode.Day1;

public abstract class ElveWithMostCalories : AdventDay
{
    public static string Day => "Day1";

    public static string Run(Context ctx)
    {
        IEnumerable<(string Name, int Calories)> Iterate()
        {
            using var f = File.OpenRead(ctx.GetPath("input.txt"));
            using var sr = new StreamReader(f);
            var currentElve = 1;
            var currentCalories = 0;
            while (sr.ReadLine() is { } line)
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

        var (name, calories) = Iterate().MaxBy(elve => elve.Calories);
        return $"Elve {name} has most calories with {calories}";
    }
}