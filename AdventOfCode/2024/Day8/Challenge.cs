using System.Drawing;
using PointSet = System.Collections.Generic.HashSet<AdventOfCode._2024.Day8.Point>;

namespace AdventOfCode._2024.Day8;

using Map = Size;

public class Challenge : IAdventDay
{
    public static string Run(Context ctx)
    {
        var (map, antennas) = InitializeMap(ctx.GetInputIterator());

        PointSet antiNodesPart1 = [];
        PointSet antiNodesPart2 = [];

        foreach (var (antenna1, antenna2) in antennas
                     .GroupBy(x => x.Frequency)
                     .SelectMany(YieldPairs))
        {
            Part1(antenna1, antenna2, map, antiNodesPart1);
            Part2(antenna1, antenna2, map, antiNodesPart2);
        }
        
        return $"Part 1: {antiNodesPart1.Count}, Part 2: {antiNodesPart2.Count}";
    }

    private static void Part1(Point antenna1, Point antenna2, Map map, HashSet<Point> antinodes)
    {
        var v = antenna2 - antenna1;
        AddIfInBounds(antenna1 - v);
        AddIfInBounds(antenna2 + v);
        
        return;
        
        void AddIfInBounds(Point antiNode)
        {
            if (!antiNode.IsOutside(map))
                antinodes.Add(antiNode);
        }
    }
    
    private static void Part2(Point antenna1, Point antenna2, Map map, HashSet<Point> antinodes)
    {
        var v = antenna2 - antenna1;
        
        AddAntiNodes(antenna1, v);
        AddAntiNodes(antenna2, -1 * v);
        
        return;
        
        void AddAntiNodes(Point start, Vector vector)
        {
            antinodes.Add(start);
            while (true)
            {
                var antiNode = start + vector;
                if (antiNode.IsOutside(map))
                    break;

                antinodes.Add(antiNode);
                start = antiNode;
            }
        }
    }

    /// <summary>
    ///     For well known minimal problems like yielding combinations from a given set,
    ///     Chat GPT may provide you the correct code 
    /// </summary>
    private static IEnumerable<(Point antenna1, Point antenna2)> YieldPairs(IEnumerable<Antenna> frequencyGroup)
    {
        var antennas = frequencyGroup.ToList();
        for (var i = 0; i < antennas.Count; i++)
        for (var j = i + 1; j < antennas.Count; j++)
            yield return (antennas[i].Location, antennas[j].Location);
    }

    private static ChallengeContext InitializeMap(IEnumerable<string> lines)
    {
        List<Antenna> antennas = new();
        var yCoord = 0;
        var width = 0;
        foreach (var line in lines)
        {
            width = line.Length;
            for (var i = 0; i < line.Length; i++)
            {
                switch (line[i])
                {
                    case '.':
                        break;
                    case var freq:
                        antennas.Add(new Antenna(freq, new Point(i, yCoord)));
                        break;
                }
            }
            yCoord++;
        }
        return new ChallengeContext(new Map(width, yCoord), antennas);
    }
}

public record struct ChallengeContext(Map Grid, List<Antenna> Antennas);
public readonly record struct Antenna(char Frequency, Point Location);
public readonly record struct Vector(int X, int Y)
{
    public static Point operator +(Point a, Vector b) => new(a.X + b.X, a.Y + b.Y);
    public static Point operator -(Point a, Vector b) => new(a.X - b.X, a.Y - b.Y);
    public static Vector operator *(int scalar, Vector v) => new(scalar * v.X, scalar * v.Y);
}
public readonly record struct Point(int X, int Y)
{
    public static Vector operator -(Point a, Point b) => new(a.X - b.X, a.Y - b.Y);
    public bool IsOutside(Map map) => X < 0 || X >= map.Width || Y < 0 || Y >= map.Height;
}