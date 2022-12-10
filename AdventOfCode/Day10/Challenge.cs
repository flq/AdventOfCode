using System.Text;

namespace AdventOfCode.Day10;

public class Challenge : IAdventDay
{
    static int NoOp(int value) => value;
    public static string Day => "Day10";
    public static string Run(Context ctx)
    {
        var register = 1;
        
        var input = ctx.GetInputIterator().GetEnumerator();
        var instructionStack = new Stack<Instruction>();
        var currentCRTLine = new StringBuilder(40);
        for (var cycle = 1; cycle <= 240; cycle++)
        {
            currentCRTLine.Append(Math.Abs(currentCRTLine.Length - register) <= 1 ? '#' : '.');
            if (currentCRTLine.Length == 40)
            {
                Console.WriteLine(currentCRTLine);
                currentCRTLine.Clear();
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
        
        return "";
    }

    private record struct Instruction(Func<int, int> Operation);

}