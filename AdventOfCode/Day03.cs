namespace AdventOfCode;

public class Day03 : BaseDay
{
    private readonly string[] _input;

    public Day03()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new($"{Part1()}");

    public override ValueTask<string> Solve_2() => new($"{Part2Attempt5()}");

    public int Part1(){
        int sum = 0;
        foreach (var line in _input) {
            char biggest = '0';
            char secondBiggest = '0';
            for (int i = 0; i < line.Length-1; i++) {
                if (line[i] > biggest) {
                    biggest = line[i];
                    secondBiggest = '0';
                } else if (line[i] > secondBiggest) {
                    secondBiggest = line[i];
                }
            }
            if (line[^1] > secondBiggest) {
                    secondBiggest = line[^1];
                }
            // Console.WriteLine((biggest - '0') * 10 + (secondBiggest - '0'));
            sum += (biggest - '0') * 10 + (secondBiggest - '0');
        }
        return sum;
    }

    public long Part2(){
        long sum = 0;
        int numDigits = 12;
        foreach (var line in _input) {
            char[] bestSoFar = new char[numDigits];
            for (int i = 0; i < numDigits; i ++) {
                bestSoFar[i] = '0';
            }
            for (int i = 0; i < line.Length-numDigits; i++) {
                for (int j = 0; j < numDigits; j++) {
                    if (line[i] > bestSoFar[j]) {
                        bestSoFar[j] = line[i];
                        for (int k = j+1; k < numDigits; k++) {
                            bestSoFar[k] = '0';
                        }
                        break;
                    }
                }
            }
            for (int i = numDigits; i > 0; i--) {
                for (int j = numDigits - i; j < numDigits; j ++) {
                    if (line[^i] > bestSoFar[j]) {
                        bestSoFar[j] = line[^i];
                        for (int k = j+1; k < numDigits; k++) {
                            bestSoFar[k] = '0';
                        }
                        break;
                    }
                }
            }
            // Console.WriteLine(new string(bestSoFar));
            sum += long.Parse(new string(bestSoFar));
        }
        return sum;
    }

    public long Part2WithoutStrings() {
        long sum = 0;
        int numDigits = 12;
        foreach (var line in _input) {
            char[] bestSoFar = new char[numDigits];
            for (int i = 0; i < line.Length; i++) {
                for (int j = 0; j < numDigits; j++) {
                    if (line.Length-i < numDigits -j) {
                        continue;
                    }
                    if (line[i] > bestSoFar[j]) {
                        bestSoFar[j] = line[i];
                        for (int k = j+1; k < numDigits; k++) {
                            bestSoFar[k] = '0';
                        }
                        break;
                    }
                }
            }
            long number = 0;
            for (int i = 0; i < numDigits; i++) {
                number = number * 10 + (bestSoFar[i] - '0');
            }
            sum += number;
        }
        return sum;
    }

    public long Part2WithoutChars() {
        long sum = 0;
        int numDigits = 12;
        
        long[] pow10 = new long[numDigits + 1];
        pow10[0] = 1;
        for (int i = 1; i <= numDigits; i++) {
            pow10[i] = pow10[i-1] * 10;
        }

        foreach (var line in _input) {
            long bestSoFar = 0;
            for (int i = 0; i < line.Length; i++) {
                int digit = line[i] - '0';
                for (int j = 0; j < numDigits; j++) {
                    if (line.Length-i < numDigits -j) {
                        continue;
                    }
                    if (digit > (bestSoFar/ pow10[numDigits-j-1])%10) {
                        bestSoFar = ((bestSoFar/ pow10[numDigits-j])*10 + digit)*pow10[numDigits-j-1];
                        break;
                    }
                }
            }
            sum += bestSoFar;
        }
        return sum;
    }

    public long Part2Attempt4() {
        long sum = 0;
        int numDigits = 12;
        Span<int> bestSoFar = stackalloc int[numDigits];
        foreach (var line in _input) {
            bestSoFar[0] = 0;
            for (int i = 0; i < line.Length; i++) {
                int digit = line[i] - '0';
                for (int j = 0; j < numDigits; j++) {
                    if (line.Length-i < numDigits -j) {
                        continue;
                    }
                    if (digit > bestSoFar[j]) {
                        bestSoFar[j] = digit;
                        for (int k = j+1; k < numDigits; k++) {
                            bestSoFar[k] = 0;
                        }
                        break;
                    }
                }
            }
            long number = 0;
            for (int i = 0; i < numDigits; i++) {
                number = number * 10 + bestSoFar[i];
            }
            sum += number;
        }
        return sum;
    }

    public long Part2Attempt5() {
        long sum = 0;
        int numDigits = 12;
        Span<char> bestSoFar = stackalloc char[numDigits];
        foreach (var line in _input) {
            bestSoFar[0] = '0';
            for (int i = 0; i < line.Length; i++) {
                for (int j = 0; j < numDigits; j++) {
                    if (line.Length-i < numDigits -j) {
                        continue;
                    }
                    if (line[i] > bestSoFar[j]) {
                        bestSoFar[j] = line[i];
                        for (int k = j+1; k < numDigits; k++) {
                            bestSoFar[k] = '0';
                        }
                        break;
                    }
                }
            }
            long number = 0;
            for (int i = 0; i < numDigits; i++) {
                number = number * 10 + (bestSoFar[i] - '0');
            }
            sum += number;
        }
        return sum;
    }

}
