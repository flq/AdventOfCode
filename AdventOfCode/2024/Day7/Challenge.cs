using System.Collections;
using System.Globalization;

namespace AdventOfCode._2024.Day7;

public class Challenge : IAdventDay
{
    public static string Run(Context ctx)
    {
        var entries = Initialize(ctx.GetInputIterator());
        decimal sumOfResults = 0;
        foreach (var entry in entries)
        {
            var ops = new OperationsProvider(entry.OperatorCount);
            foreach (var operations in ops.PermutateOperators())
            {
                var inputs = new Queue<decimal>(entry.Inputs);
                var carryOver = inputs.Dequeue();
                
                while (inputs.Count > 0) {
                    carryOver = operations.Pop()(carryOver, inputs.Dequeue());
                }
                
                if (carryOver == entry.Result)
                {
                    sumOfResults += carryOver;
                    break;
                }
            }
        }
        
        return sumOfResults.ToString(CultureInfo.InvariantCulture);
    }
    private static List<Entry> Initialize(IEnumerable<string> inputFile)
    {
        var entries = (from line in inputFile 
            select line.Split(":") into resultAndInput 
            let input = resultAndInput[1].Split(" ", StringSplitOptions.RemoveEmptyEntries) 
            select new Entry(decimal.Parse(resultAndInput[0]), input.Select(decimal.Parse).ToArray()))
            .ToList();
        return entries;
    }
}

internal readonly record struct Entry(decimal Result, decimal[] Inputs)
{
    public int OperatorCount => Inputs.Length - 1;
}

internal class OperationsProvider(int operationsCount)
{
    public IEnumerable<Stack<Func<decimal, decimal, decimal>>> PermutateOperators()
    {
        // Inspection of the input shows that the number of operators stays way way below 2^32,
        // hence we can count up from 0 to (no of ops)^operationsCount in a single int (see docs on BitArray ctor)
        var operatorPermutationLimit = Math.Pow(2, operationsCount);
        for (var i = 0; i < operatorPermutationLimit; i++)
        {
            var v = new BitArray([i]);
            var stack = new Stack<Func<decimal, decimal, decimal>>();
            for (var o = 0; o < operationsCount; o++)
            {
                stack.Push(v[o] switch
                {
                    true => Add,
                    false => Multiply
                });
            }
            yield return stack;
        }
    }

    static decimal Add(decimal a, decimal b) => a + b;
    static decimal Multiply(decimal a, decimal b) => a * b;
}