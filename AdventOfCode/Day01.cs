namespace AdventOfCode;

public class Day01 : BaseDay
{
    private readonly string[] _input;

    public Day01()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new($"{Part1()}");

    public override ValueTask<string> Solve_2() => new($"{Part2()}");


    public int Part1() {
        int curr_value = 50;
        int modulus = 100;
        int count = 0;
        foreach (var line in _input)
        {
            char direction = line[0];
            int amount = int.Parse(line.Substring(1));
            if (direction == 'L') {
                curr_value = (curr_value - amount) % modulus ;
            } else {
                curr_value = (curr_value + amount) % modulus;
            }
            if (curr_value == 0) {
                count++;
            }
        }
        return count;
    }

    public int Part2(){
        int curr_value = 50;
        int prev_value = 0;
        int divisor = 100;
        int count = 0;
        foreach (var line in _input)
        {
            prev_value = curr_value;
            char direction = line[0];
            int amount = int.Parse(line.Substring(1));
            if (direction == 'L') {

                curr_value -= amount;
                
                if (prev_value == 0) {
                    count -= curr_value/divisor;
                } else if (curr_value <= 0) {
                    count -= curr_value/divisor -1;
                }
                curr_value = (curr_value % divisor);
                if (curr_value < 0) {
                    curr_value += 100;
                }
            } else {
                curr_value += amount;
                count += curr_value/divisor;
                curr_value = curr_value % divisor;
            }
        }
        return count;
    }
}
