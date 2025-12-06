namespace AdventOfCode;

public class Day06 : BaseDay
{
    private readonly string[] _input;

    public Day06()
    {
        _input = File.ReadAllLines(InputFilePath);

    }

    public override ValueTask<string> Solve_1() => new($"{Part1Refactored()}");

    public override ValueTask<string> Solve_2() => new($"{Part2()}");

    public long Part1Refactored() {
        long total = 0;
        long subtotal;

        int numberRows = _input.Length -1;

        string operations = _input[^1].Replace(" ", "");
        List<string>[] nums = new List<string>[numberRows]; 

        for (int i = 0; i < numberRows; i++) {
            nums[i] = _input[i].Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
            // Console.WriteLine("{0}", nums[i]);
        }

        for (int i = 0; i < operations.Length; i++) {         
            if (operations[i] == '+') {
                subtotal = 0;
                for (int j = 0; j < numberRows; j++) {
                    subtotal += long.Parse(nums[j][i]);
                }
            } else {
                subtotal = 1;
                for (int j = 0; j < numberRows; j++) {
                    subtotal *= long.Parse(nums[j][i]);
                }
            }
            total += subtotal;
        }

        return total;
    }

    public long Part1RefactoredSlower() {
        long total = 0;
        long subtotal;
        char prevOp = '+';
        int numberRows = _input.Length -1;
        string operations = _input[^1];
        string[] nums = new string[numberRows];
        for (int j = 0; j < numberRows; j++) {
            nums[j] = "0";
        }

        for (int i = 0; i < operations.Length; i++) {
            if (operations[i] == '+' || operations[i] == '*') {
                // Console.WriteLine(prevOp);
                if (prevOp == '+') {
                    subtotal = 0;
                    for (int j = 0; j < numberRows; j++) {
                        subtotal += long.Parse(nums[j]);
                        nums[j] = "";
                    }
                } else {
                    subtotal = 1;
                    for (int j = 0; j < numberRows; j++) {
                        subtotal *= long.Parse(nums[j]);
                        nums[j] = "";
                    }
                }
                total += subtotal;
                prevOp = operations[i];
            }

            for (int j = 0; j < numberRows; j++) {
                nums[j] += _input[j][i];
            }
        }

        if (prevOp == '+') {
            subtotal = 0;
            for (int j = 0; j < numberRows; j++) {
                subtotal += long.Parse(nums[j]);
                nums[j] = "";
            }
        } else {
            subtotal = 1;
            for (int j = 0; j < numberRows; j++) {
                subtotal *= long.Parse(nums[j]);
                nums[j] = "";
            }
        }
        total += subtotal;

        return total;
    }

    public int Part2(){
        return 0;
    }
}
