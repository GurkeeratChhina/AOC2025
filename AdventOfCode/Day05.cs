namespace AdventOfCode;

public class Day05 : BaseDay
{
    private readonly string[] _input;
    long[] lows;
    long[] highs;
    byte[] indices;
    long[] food;
    short[] foodIndices;

    public Day05()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new($"{Part1()}");

    public override ValueTask<string> Solve_2() => new($"{Part2()}");

    public void parseCustom() {
        List<long> tempLows = new List<long>();
        List<long> tempHighs = new List<long>();

        int split = 0;
        for (; split < _input.Length; split++) {
            string line = _input[split];
            if (line.Length == 0) {
                break;
            }
            string[] halves = line.Split("-");
            tempLows.Add(long.Parse(halves[0]));
            tempHighs.Add(long.Parse(halves[1]));
        }
        lows = tempLows.ToArray();
        highs = tempHighs.ToArray();

        indices = new byte[split];
        food = new long[_input.Length - split - 1];
        for (byte i = 0; i < split; i++) {
            indices[i] = i;
        }
        for (int i = split+1; i < _input.Length; i++) {
            food[i-split-1] = long.Parse(_input[i]);
        }
        foodIndices = new short[food.Length];
        for (short i = 0; i < foodIndices.Length; i++) {
            foodIndices[i] = i;
        }
    }

    public int Part1(){
        parseCustom();

        int count = 0;
        
        foodIndices.Sort(customComparison2);
        indices.Sort(customTupleComparison);

        short current_index = 0;
        foreach (var i in foodIndices) {
            while(current_index < indices.Length && highs[indices[current_index]] < food[i]) {
                current_index++;
            }
            if (current_index < indices.Length && food[i] >= lows[indices[current_index]]) {
                count++;
            }
        }

        return count;
    }

    public long Part2(){
        long count = 0;
        long prev_low = -1;
        long prev_high = -1;

        foreach (var index in indices) {
            if (lows[index] <= prev_high) {
                prev_high = Math.Max(prev_high, highs[index]);
            } else {
                count += prev_high - prev_low + 1;
                prev_low = lows[index];
                prev_high = highs[index];
            }
        }
        count += prev_high - prev_low + 1;
        return count-1;
    }

    public int customTupleComparison( byte a, byte b) {
        if (lows[a] == lows[b]) {
            return 0;
        }
        if (lows[a] < lows[b]) {
            return -1;
        } 
        return 1;
    }

    public int customComparison2(short a, short b) {
        if (food[a] == food[b]) {
            return 0;
        }
        if (food[a] < food[b]) {
            return -1;
        } 
        return 1;
    }
}
