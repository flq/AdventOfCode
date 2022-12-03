namespace AdventOfCode.Day3;

public abstract class SackPacking : IAdventDay
{
    public static string Day => "Day3";

    public static string Run(Context ctx) => ctx.GetInputIterator()
        .Select(GetPriorityOfDoubleItem)
        .Sum()
        .ToString();

    // Use ASCII code of char as numeric value and offset according to requirements
    public static int Priority(char character) => character - (char.IsUpper(character) ? 38 : 96);

    private static int GetPriorityOfDoubleItem(string sack)
    {
        var chars = sack.AsSpan();
        var firstCompartment = chars[..(chars.Length / 2)];
        var secondCompartment = chars[(chars.Length / 2)..];

        //A good little O(N^2) but it's December...
        for (var i = 0; i < firstCompartment.Length; i++)
        for (var j = 0; j < secondCompartment.Length; j++)
            if (firstCompartment[i] == secondCompartment[j])
                return Priority(firstCompartment[i]);

        throw new ArgumentException("No double item in this sack");
    }
}