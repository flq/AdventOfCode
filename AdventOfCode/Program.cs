using AdventOfCode;
using AdventOfCode.Day1;

static void Run<T>() where T : AdventDay
{
    Console.WriteLine("Day1:");
    var output = T.Run(new Context(T.Day));
    Console.WriteLine(output);
}

Console.WriteLine("Advent of Code!");

Run<ElveWithMostCalories>();