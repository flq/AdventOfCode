using AdventOfCode;

Console.WriteLine("Advent of Code!");

Run<AdventOfCode._2024.Day4.Challenge>();

return;

static void Run<T>() where T : IAdventDay
{
    var day = GetDayPart();
    Console.WriteLine($"{day}:");
    var output = T.Run(new Context(day));
    Console.WriteLine(output);
    return;
    string GetDayPart() => typeof(T).Namespace!.Split(".")[2];
}