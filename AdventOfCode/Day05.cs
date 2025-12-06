namespace AdventOfCode;

public class Day05 : BaseDay
{
    private readonly string[] _input;

    public Day05()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new($"{Part1()}");

    public override ValueTask<string> Solve_2() => new($"{Part2()}");

    public int Part1(){
        int count = 0;
        int split = 0;

        List<(long,long)> ranges = new List<(long,long)>();
        List<long> food = new List<long>();

        for (; split < _input.Length; split++) {
            string line = _input[split];
            if (line.Length == 0) {
                break;
            }
            string[] halves = line.Split("-");
            ranges.Add((long.Parse(halves[0]), long.Parse(halves[1])));
        }
        for (int i = split+1; i < _input.Length; i++) {
            food.Add(long.Parse(_input[i]));
        }

        ranges.Sort();
        food.Sort();

        foreach (var item in food) {

        }

        return count;
    }

    public long Part2(){
        long count = 0;
        long prev_low = -1;
        long prev_high = -1;

        List<(long low,long high)> ranges = new List<(long,long)>();

        for (int i = 0; i < _input.Length; i++) {
            string line = _input[i];
            if (line.Length == 0) {
                break;
            }
            string[] halves = line.Split("-");
            ranges.Add((long.Parse(halves[0]), long.Parse(halves[1])));
        }

        ranges.Sort();

        foreach (var range in ranges) {
            if (range.low <= prev_high) {
                prev_high = Math.Max(prev_high, range.high);
            } else {
                count += prev_high - prev_low + 1;
                prev_low = range.low;
                prev_high = range.high;
            }
        }
        count += prev_high - prev_low + 1;
        return count-1;
    }
}
