namespace AdventOfCode.Day3;

public abstract class BadgeFinding : IAdventDay
{
    public static string Day => "Day3";

    public static string Run(Context ctx) => ctx.GetInputIterator()
        .Chunk(3)
        .Select(GetPriorityOfBadge)
        .Sum()
        .ToString();

    private static int GetPriorityOfBadge(string[] sacksOf3Elves)
    {
        //A good little O(N^3) but it's December...
        for (var i = 0; i < sacksOf3Elves[0].Length; i++)
        for (var j = 0; j < sacksOf3Elves[1].Length; j++)
        for (var k = 0; k < sacksOf3Elves[2].Length; k++)
            if (sacksOf3Elves[0][i] == sacksOf3Elves[1][j] && sacksOf3Elves[0][i] == sacksOf3Elves[2][k])
                return SackPacking.Priority(sacksOf3Elves[0][i]);


        throw new ArgumentException("No common item in the 3 sacks");
    }
}