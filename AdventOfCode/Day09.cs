namespace AdventOfCode;

public class Day09 : BaseDay
{
    private readonly string[] _input;
    private int n;
    private int[] xs;
    private int[] ys;

    public Day09()
    {
        _input = File.ReadAllLines(InputFilePath);
        n = _input.Length;
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

    long area(int x1, int y1, int x2, int y2) {
        return (long)(Math.Abs(x1 - x2) + 1) * (long)(Math.Abs(y1 - y2) + 1);
    }

    public long Part1(){
        parse();
        long max_size = 0;
        for (int i = n/16; i < 3*n/16; i++) {
            for (int j = 9*n/16; j < 11*n/16; j++ ) {
                max_size = Math.Max(max_size, area(xs[i], ys[i], xs[j], ys[j]));
            }
        }
        for (int i = 5*n/16; i < 7*n/16; i++) {
            for (int j = 13*n/16; j < 15*n/16; j++ ) {
                max_size = Math.Max(max_size, area(xs[i], ys[i], xs[j], ys[j]));
            }
        }
        return max_size;
    }

    public long Part2(){
        int dx;

        int slitIndex = 2;
        for (; slitIndex < n; slitIndex+=2) { // find index of slit
            dx = xs[slitIndex] - xs[slitIndex-2];
            if (dx > xs[0]/2) {
                // Console.WriteLine("horizontal slit " + slitIndex + " has coords " + xs[slitIndex] + " " + ys[slitIndex]);
                break;
            }
        }

        int aboveIndex = 0;
        for (; aboveIndex < n; aboveIndex+=2) {
            if (xs[aboveIndex] < xs[slitIndex]) break;
        }

        long aboveRect = 0;
        for (int i = slitIndex-2; i > 0; i--) {
            if (ys[i] > ys[aboveIndex]) break;
            if (xs[i] < xs[i+2]) continue;
            aboveRect = Math.Max(aboveRect, area(xs[i], ys[i], xs[slitIndex], ys[slitIndex]));
        }

        int belowIndex = n-2;
        for (; belowIndex > 0; belowIndex-=2) {
            if (xs[belowIndex] < xs[slitIndex]) break;
        }

        long belowRect = 0;
        for (int i = slitIndex+3; i < n; i++) {
            if (ys[i] < ys[belowIndex]) break;
            if (xs[i] < xs[i-2]) continue;
            belowRect = Math.Max(belowRect, area(xs[i], ys[i], xs[slitIndex+1], ys[slitIndex+1]));
        }

        if (aboveRect > belowRect) {
            return aboveRect;
        }
        return belowRect;
    }
}
