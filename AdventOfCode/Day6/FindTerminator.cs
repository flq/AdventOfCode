namespace AdventOfCode.Day6;

public class FindTerminator : IAdventDay
{
    public static string Day => "Day6";

    public static string Run(Context ctx)
    {
        var messageStream = ctx.GetInputAsMemory();
        var consumedBytes = 0;
        while (consumedBytes < messageStream.Length - 4)
        {
            var items = messageStream.Slice(consumedBytes, 4).Span;
            var allDifferent = items switch
            {
                [var one, var two, var three, var four]
                    when one != two && one != three && one != four &&
                         two != three && two != four &&
                         three != four => true,
                _ => false
            };
            if (allDifferent) return (consumedBytes + 4).ToString();
            consumedBytes++;
        }

        throw new ArgumentException("Did not find terminator");
    }
}