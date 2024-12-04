using AdventOfCode;

Console.WriteLine("Advent of Code!");

Run<AdventOfCode._2024.Day4.Challenge>();

return;

static void Run<T>() where T : IAdventDay
{
    Console.WriteLine($"{T.Day}:");
    var output = T.Run(new Context(T.Day));
    Console.WriteLine(output);
}