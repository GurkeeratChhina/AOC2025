namespace AdventOfCode;

public class Day06 : BaseDay
{
    private readonly string[] _input;

    public Day06()
    {
        _input = File.ReadAllLines(InputFilePath);

    }

    public override ValueTask<string> Solve_1() => new($"{Part1Faster()}");

    public override ValueTask<string> Solve_2() => new($"{Part2Faster()}");

    public long Part1Refactored() {
        long total = 0;
        long subtotal;

        int numberRows = _input.Length -1;

        string operations = _input[^1].Replace(" ", "");
        List<string>[] nums = new List<string>[numberRows]; 

        for (int i = 0; i < numberRows; i++) {
            nums[i] = _input[i].Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();
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

    public long Part1Faster() {
        long total = 0;
        long subtotal;
        long num;
        int numberRows = _input.Length -1;
        string operations = _input[^1];

        for (int i = operations.Length -1; i >= 0; i--) {
            if (operations[i] == '+') {
                subtotal = 0;
                for (int j = 0; j < numberRows; j++) {
                    int k = i;
                    while (_input[j][k] == ' ') {
                        k++;
                    }
                    num =  _input[j][k] - '0';
                    while( _input[j][++k] != ' ') {
                        num = num*10 + _input[j][k] - '0';
                    }
                    subtotal += num;
                }
                total += subtotal;
                i--;
            } else if (operations[i] == '*') {
                subtotal = 1;
                for (int j = 0; j < numberRows; j++) {
                    int k = i;
                    while (_input[j][k] == ' ') {
                        k++;
                    }
                    num =  _input[j][k] - '0';
                    while( _input[j][++k] != ' ') {
                        num = num*10 + _input[j][k] - '0';
                    }
                    subtotal *= num;
                }
                total += subtotal;
                i--;
            }
            
        }

        return total;
    }

    public long Part2() {
        long total = 0;
        long subtotal;
        char prevOp = '+';
        int numberRows = _input.Length -1;
        string operations = _input[^1];
        List<long> nums = new List<long>();
        long column;
        char digit = ' ';

        for (int i = 0; i < operations.Length; i++) {
            if (operations[i] == '+' || operations[i] == '*') {
                // Console.WriteLine(prevOp);
                if (prevOp == '+') {
                    subtotal = 0;
                    foreach (var num in nums) {
                        subtotal += num;
                    }
                    nums.Clear();
                } else {
                    subtotal = 1;
                    foreach (var num in nums) {
                        subtotal *= num;
                    }
                    nums.Clear();
                }
                total += subtotal;
                prevOp = operations[i];
            }
            column = 0;
            for (int j = 0; j < numberRows; j++) {
                digit = _input[j][i];
                if (digit != ' ') {
                    column = column*10 + (digit - '0');
                }
            }
            if (column != 0) {
                nums.Add(column);
            }
        }

        if (prevOp == '+') {
            subtotal = 0;
            foreach (var num in nums) {
                subtotal += num;
            }
        } else {
            subtotal = 1;
            foreach (var num in nums) {
                subtotal *= num;
            }
        }
        total += subtotal;

        return total;
    }

    public long Part2Faster() {
        long total = 0;
        long subtotal = 0;
        char prevOp = '+';
        int numberRows = _input.Length -1;
        string operations = _input[^1];
        long column;
        char digit = ' ';

        for (int i = 0; i < operations.Length; i++) {
            if (operations[i] == '+' || operations[i] == '*') {
                total += subtotal;
                prevOp = operations[i];
                if (prevOp == '+') {
                    subtotal = 0;
                } else {
                    subtotal = 1;
                }
            }
            column = 0;
            for (int j = 0; j < numberRows; j++) {
                digit = _input[j][i];
                if (digit != ' ') {
                    column = column*10 + (digit - '0');
                }
            }
            if (column != 0) {
                if (prevOp == '+') {
                    subtotal += column;
                } else {
                    subtotal *= column;
                }
            }
        }
        total += subtotal;

        return total;
    }

}
