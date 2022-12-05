#pragma warning disable CS8524
namespace AdventOfCode.Day2;

public abstract class RockPaperScissors : IAdventDay
{
    public static string Day => "Day2";

    public static string Run(Context ctx)
    {
        (Result, int) Winner(Move opponentMove, Move myMove) =>
            (opponentMove, myMove) switch
            {
                (Move.Rock, Move.Scissors) or (Move.Paper, Move.Rock) or (Move.Scissors, Move.Paper)
                    => (Result.Loss, PointsFromMove(myMove)),
                (Move.Scissors, Move.Rock) or (Move.Rock, Move.Paper) or (Move.Paper, Move.Scissors)
                    => (Result.Win, PointsFromMove(myMove)),
                _ => (Result.Draw, PointsFromMove(myMove))
            };

        var pointsStep1 = ctx.GetInputIterator()
            .Select(line => line.Split(" "))
            .Select(result => Winner(Translate(result[0]), Translate(result[1])))
            .Select(PointsCalculator)
            .Sum();

        var pointsStep2 = (from line in ctx.GetInputIterator()
                let pieces = line.Split(" ")
                let opponentMove = Translate(pieces[0])
                let result =
                    PointsCalculator(Winner(opponentMove, ProperMoveForDesiredOutcome(opponentMove, pieces[1])))
                select result)
            .Sum();

        return $"{pointsStep1.ToString()},{pointsStep2.ToString()}";
    }

    private static Move ProperMoveForDesiredOutcome(Move opponentMove, string desiredOutcome)
    {
        return (opponentMove, desiredOutcome) switch
        {
            (_, "Y") => opponentMove, // Draw
            (_, "Z") => opponentMove switch // Win
            {
                Move.Paper => Move.Scissors,
                Move.Scissors => Move.Rock,
                Move.Rock => Move.Paper
            },
            (_, "X") => opponentMove switch // Lose
            {
                Move.Paper => Move.Rock,
                Move.Scissors => Move.Paper,
                Move.Rock => Move.Scissors
            },
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static int PointsCalculator((Result, int) roundResult) =>
        roundResult switch
        {
            (Result.Win, var pointsFromRound) => 6 + pointsFromRound,
            (Result.Loss, var pointsFromRound) => pointsFromRound,
            (Result.Draw, var pointsFromRound) => 3 + pointsFromRound,
            _ => throw new ArgumentOutOfRangeException()
        };

    private static Move Translate(string input) => input switch
    {
        "A" or "X" => Move.Rock,
        "B" or "Y" => Move.Paper,
        "C" or "Z" => Move.Scissors,
        _ => throw new ArgumentOutOfRangeException(nameof(input), input, null)
    };

    private static int PointsFromMove(Move move) => move switch
    {
        Move.Rock => 1,
        Move.Paper => 2,
        Move.Scissors => 3,
        _ => throw new ArgumentOutOfRangeException(nameof(move), move, null)
    };

    private enum Move
    {
        Rock,
        Paper,
        Scissors
    }

    private enum Result
    {
        Loss,
        Win,
        Draw
    }
}