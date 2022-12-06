namespace AdventOfCode.Day6;

public abstract class FindTerminator : IAdventDay
{
    public static string Day => "Day6";

    public static string Run(Context ctx)
    {
        const int terminatorLength = 14;
        static bool CheckIfAllDifferent(ReadOnlySpan<byte> bytes)
        {
            for (var i = 0; i < bytes.Length; i++)
            for (var j = i + 1; j < bytes.Length; j++)
                if (bytes[i] == bytes[j])
                    return false;
            return true;
        }
        
        var messageStream = ctx.GetInputAsMemory();
        
        var consumedBytes = 0;
        while (consumedBytes < messageStream.Length - terminatorLength)
        {
            var items = messageStream.Slice(consumedBytes, terminatorLength).Span;
            if (CheckIfAllDifferent(items)) return (consumedBytes + terminatorLength).ToString();
            consumedBytes++;
        }

        throw new ArgumentException("Did not find terminator");
    }
}