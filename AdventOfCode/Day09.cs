namespace AdventOfCode;

public class Day09 : BaseDay
{
    private readonly string[] _input;
    private long[] xs;
    private long[] ys;

    public Day09()
    {
        _input = File.ReadAllLines(InputFilePath);
        xs = new long[_input.Length];
        ys = new long[_input.Length];
    }

    public override ValueTask<string> Solve_1() => new($"{Part1()}");

    public override ValueTask<string> Solve_2() => new($"{Part2()}");

    void parse() {
        long temp;
        for (int i = 0; i < _input.Length; i++) {
            string[] nums = _input[i].Split(',');
            // parse input into xs, ys

            temp = 0; 
            for (int j = 0; j < nums[0].Length; j++) {
                temp = temp*10 + nums[0][j] - '0';
            }
            xs[i] = temp;

            temp = 0;
            for (int j = 0; j < nums[1].Length; j++) {
                temp = temp*10 + nums[1][j] - '0';
            }
            ys[i] = temp;
        }
    }

    public long Part1(){
        parse();
        long max_size = 0;
        for (int i = 0; i < _input.Length - 1; i++) {
            for (int j = i+1; j < _input.Length; j++) {
                max_size = Math.Max(max_size, Math.Abs(xs[i] - xs[j] +1 )*Math.Abs(ys[i] - ys[j] + 1));
            }
        }

        return max_size;
    }

    public int Part2(){
        return 0;
    }
}
