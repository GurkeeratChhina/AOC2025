namespace AdventOfCode;

public class Day09 : BaseDay
{
    private readonly string[] _input;
    private int[] xs;
    private int[] ys;

    public Day09()
    {
        _input = File.ReadAllLines(InputFilePath);
        xs = new int[_input.Length];
        ys = new int[_input.Length];
    }

    public override ValueTask<string> Solve_1() => new($"{Part1()}");

    public override ValueTask<string> Solve_2() => new($"{Part2()}");

    void parse() {
        // parse input into xs, ys
        for (int i = 0; i < _input.Length; i++) {
            string[] nums = _input[i].Split(',');
            xs[i] = int.Parse(nums[0]);
            ys[i] = int.Parse(nums[1]);
        }
    }

    public long Part1(){
        parse();
        long max_size = 0;
        for (int i = 0; i < _input.Length - 1; i++) {
            for (int j = i+1; j < _input.Length; j++) {
                max_size = Math.Max(max_size, (long)Math.Abs(xs[i] - xs[j] +1 )*(long)Math.Abs(ys[i] - ys[j] + 1));
            }
        }

        return max_size;
    }

    public int Part2(){
        int min_x;
        int max_x;
        int min_y;
        int max_y;
        int delta;

        return 0;
    }
}
