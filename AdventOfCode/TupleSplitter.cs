namespace AdventOfCode;

public static partial class TupleSplitter
{
    public static (string left, string right) GetPair(this string input, char separator) =>
        input.Split(separator) switch
        {
            [{ } left, { } right] => (left, right),
            _ => throw new ArgumentException("Bad input " + input)
        };

    public static (int left, int right) GetNumberPair(this string input, char separator)
    {
        var (left, right) = input.GetPair(separator);
        return (int.Parse(left), int.Parse(right));
    }
}

