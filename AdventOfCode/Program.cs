using AdventOfCode;
using AdventOfCode.Day1;
using AdventOfCode.Day2;

static void Run<T>() where T : IAdventDay
{
    Console.WriteLine($"{T.Day}:");
    var output = T.Run(new Context(T.Day));
    Console.WriteLine(output);
}

Console.WriteLine("Advent of Code!");

Run<RockPaperScissors>();