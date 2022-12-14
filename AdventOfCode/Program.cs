using AdventOfCode;

static void Run<T>() where T : IAdventDay
{
    Console.WriteLine($"{T.Day}:");
    var output = T.Run(new Context(T.Day));
    Console.WriteLine(output);
}

Console.WriteLine("Advent of Code!");

// Run<AdventOfCode.Day1.ElveWithMostCalories>();
// Run<AdventOfCode.Day2.RockPaperScissors>();
// Run<AdventOfCode.Day3.SackPacking>();
// Run<AdventOfCode.Day3.BadgeFinding>();
// Run<AdventOfCode.Day4.CleaningRanges>();
// Run<AdventOfCode.Day4.CleaningRangesOverlaps>();
// Run<AdventOfCode.Day5.CraneStacking>();
// Run<AdventOfCode.Day6.FindTerminator>();
// Run<AdventOfCode.Day7.Challenge>();
// Run<AdventOfCode.Day8.Challenge>();
// Run<AdventOfCode.Day9.Challenge>();
// Run<AdventOfCode.Day10.Challenge>();
// Run<AdventOfCode.Day11.Challenge>();
Run<AdventOfCode.Day12.Challenge>();