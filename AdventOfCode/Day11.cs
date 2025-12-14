namespace AdventOfCode;

public class Day11 : BaseDay
{
    private readonly string[] _input;

    public Day11()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new($"{Part1()}");

    public override ValueTask<string> Solve_2() => new($"{Part2()}");

    public int Part1(){
        return 0;
    }

    public int Part2(){
        return 0;
    }
}
