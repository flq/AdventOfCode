namespace AdventOfCode;

public static class TupleSplitter
{
    public static (string left, string right) GetPair(this string input, char separator)
    {
        var output = input.Split(separator);
        return (output[0], output[1]);
    }
    public static (int left, int right) GetNumberPair(this string input, char separator)
    {
        var output = input.Split(separator);
        return (int.Parse(output[0]), int.Parse(output[1]));
    }
    
}