namespace AdventOfCode.Day10;

public class Challenge : IAdventDay
{
    static int NoOp(int value) => value;
    public static string Day => "Day10";
    public static string Run(Context ctx)
    {
        var measurementCycles = new HashSet<int>(new[] {20, 60, 100, 140, 180, 220});
        
        var register = 1;
        var storedValues = new List<(int Cycle, int Value)>();

        var input = ctx.GetInputIterator().GetEnumerator();
        var instructionStack = new Stack<Instruction>();
        for (var cycle = 1; cycle <= 220; cycle++)
        {
            if (measurementCycles.Contains(cycle))
            {
                storedValues.Add((cycle, register));
            }

            if (instructionStack.Count == 0 && input.MoveNext())
            {
                switch (input.Current.Split(" "))
                {
                    case ["noop"]:
                        instructionStack.Push(new Instruction(NoOp));
                        break;
                    case ["addx", var valStr] when int.TryParse(valStr, out var val):
                        instructionStack.Push(new Instruction(v => v + val));
                        instructionStack.Push(new Instruction(NoOp));
                        break;
                }
            }
            register = instructionStack.Pop().Operation(register);
        }
        var sum = storedValues.Select(((tuple) => tuple.Cycle * tuple.Value)).Sum();
        return sum.ToString();
    }

    private record struct Instruction(Func<int, int> Operation);

}