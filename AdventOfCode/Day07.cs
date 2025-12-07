namespace AdventOfCode;

public class Day07 : BaseDay
{
    private readonly string[] _input;

    public Day07()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new($"{Part1()}");

    public override ValueTask<string> Solve_2() => new($"{Part2()}");

    public int Part1(){
        int count = 0;
        bool[] streams = new bool[_input[0].Length];
        int min = 0;
        int max = 0;
        for (int i = 0; i < _input[0].Length; i++) {
            if (_input[0][i] == 'S') {
                streams[i] = true;
                min = i;
                max = i;
                break;
            }
        }

        for (int i = 2; i < _input.Length; i++) {
            // Console.WriteLine(i + " " + min + " " + max);
            if (_input[i][min] == '^') {
                count++;
                streams[min] = false;
                streams[min-1] = true;
                streams[min+1] = true;
                min--;
            }
            if (_input[i][max] == '^') {
                count++;
                streams[max] = false;
                streams[max-1] = true;
                streams[max+1] = true;
                max++;
            }

            for (int j = min+2; j < max-1; j++) {
                if (_input[i][j] == '^') {
                    if (streams[j]) {
                        count++;
                        streams[j] = false;
                        streams[j-1] = true;
                        streams[j+1] = true;
                    }
                    j++;
                }
            }

            i++;
        }

        return count-1;
    }

    public long Part2(){
        long[] streams = new long[_input[0].Length];
        int min = 0;
        int max = 0;
        for (int i = 0; i < _input[0].Length; i++) {
            if (_input[0][i] == 'S') {
                streams[i] = 1;
                min = i;
                max = i;
                break;
            }
        }

        for (int i = 2; i < _input.Length; i++) {
            if (_input[i][min] == '^') {
                streams[min-1] += streams[min];
                streams[min+1] += streams[min];
                streams[min] = 0;
                min--;
            }
            if (_input[i][max] == '^') {
                streams[max-1] += streams[max];
                streams[max+1] += streams[max];
                streams[max] = 0;
                max++;
            }

            for (int j = min+2; j < max-1; j++) {
                if (_input[i][j] == '^') {
                    if (streams[j] > 0) {
                        streams[j-1] += streams[j];
                        streams[j+1] += streams[j];
                        streams[j] = 0;
                    }
                    j++;
                }
            }
            i++;
            
        }
        long count = 0;

        for(int j = 0; j < streams.Length; j++) {
            count += streams[j];
        }
    
        return count;
    }
}
