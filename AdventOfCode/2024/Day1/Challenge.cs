using System.Numerics;

namespace AdventOfCode._2024.Day1;

public class Challenge : IAdventDay
{
    public static string Day => "Day1";

    public static string Run(Context ctx)
    {
        List<int> left = [];
        List<int> right = [];

        foreach (var line in ctx.GetInputIterator())
        {
            left.Add(int.Parse(line[..5]));
            right.Add(int.Parse(line[8..]));
        }

        /*
          Part 1:
          left.Sort();
          right.Sort();
          var result = left.Zip(right, (l, r) => Abs(l - r)).Sum();
        */

        BigInteger b = 0;
        foreach (var l in left)
        {
            var appearances = right.Count(r => l == r);
            b += l * appearances;
        }
        
        return b.ToString();
    }
}