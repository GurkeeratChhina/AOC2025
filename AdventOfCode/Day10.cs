namespace AdventOfCode;

using Google.OrTools.Init;
using Google.OrTools.LinearSolver;

public class Day10 : BaseDay
{
    private readonly string[] _input;
    int n;
    private int[][,] buttons;
    private int[][] finalTargets;
    private int[] numButtons;
    private int[] numBits;

    public Day10()
    {
        _input = File.ReadAllLines(InputFilePath);
        n = _input.Length;
        buttons = new int[n][,];
        finalTargets = new int[n][];
        numButtons = new int[n];
        numBits = new int[n];
    }

    public override ValueTask<string> Solve_1() => new($"{part1ManualMatrix()}");

    public override ValueTask<string> Solve_2() => new($"{part2Placeholder()}");

    // void parse() {
    //     for (int i = 0; i < n; i++) {
    //         string line = _input[i];
    //         int j = 1;

    //         // parse .# part
    //         int arrangement = 0;
    //         for (; j < line.Length; j++) {
    //             if (line[j] == ']') break;
    //             if (line[j] == '#') {
    //                 arrangement ++;
    //             }
    //         }
    //         startingArrangements[i][j] = 1;
    //         int bitlength = j-2 +'0'; // not actually bitlength, but used to invert a digit 'n' to represent the nth bit counting from the left instead of the right

    //         // parse numbers
    //         List<int> listOfButtons = new List<int>();
    //         int number = 0;
    //         j+= 3;
    //         for (; j < line.Length; j++) {
    //             if (line[j] == '{') break;
    //             else if (line[j] == ',') continue;
    //             else if (line[j] == ')') {
    //                 listOfButtons.Add(number);
    //                 number = 0;
    //                 j+=2;
    //             } else {
    //                 number += (1 << (bitlength - line[j]));
    //             }
    //         }
    //         buttons[i] = listOfButtons;
    //         j++;
    //         int num = 0;
    //         List<int> target = new List<int>();
    //         for (; j < line.Length; j++) {
    //             if (line[j] == '}') break;
    //             if (line[j] == ',') {
    //                 target.Add(num);
    //                 num = 0;
    //             } else {
    //                 num = num*10 + (line[j]-'0');
    //             }
    //         }
    //         targets[i] = target;
    //     }
    // }

    void parse2() {
        for (int i = 0; i < n; i++) {
            string[] line = _input[i].Split(new char[]{'{', '}', '(', ')', '[', ']', ' '}, StringSplitOptions.RemoveEmptyEntries);

            numBits[i] = line[0].Length;
            numButtons[i] = line.Length - 2;
            buttons[i] = new int[numBits[i], numButtons[i]+1];
            for (int j = 0; j < numButtons[i]; j++) {
                foreach (var digit in line[j+1]) {
                    if (digit == ',') continue;
                    buttons[i][(digit - '0'), j] = 1;
                }
            }
            for (int j = 0; j < numBits[i]; j++) {
                if (line[0][j] == '#') {
                    buttons[i][j, numButtons[i]] = 1;
                }
            }
            finalTargets[i] = new int[numBits[i]];
            var xs = line[^1].Split(',');
            for (int j = 0; j < numBits[i]; j ++) {
                finalTargets[i][j] = int.Parse(xs[j]);
            }

            // Console.WriteLine($"Row {i} buttons:");
            // Console.WriteLine("starting:");
            // for (int k = 0; k < numBits[i]; k++) {
            //     Console.WriteLine(startingArrangements[i][k]);
            // }
            // for(int k = 0; k < numButtons[i]; k++) {
            //     Console.WriteLine($"{k}-th button:");
            //     for (int r = 0; r < numBits[i]; r++) {
            //         Console.WriteLine(buttons[i][k,r]);
            //     }
            // }
        }
        
    }
    
    // public int Part1(){
    //     parse();
    //     int total = 0;
    //     for (int i = 0; i < n; i++) {
    //         // Console.WriteLine(i);
    //         var results = new Dictionary<int, int> {{startingArrangements[i],0}};
    //         foreach (var button in buttons[i]) {
    //             // Console.WriteLine("testing button number value " + button);
    //             var newResults = new Dictionary<int, int>(results);
    //             foreach (var (result, mincount) in results) {
    //                 int x = button ^ result;
    //                 if (results.ContainsKey(x)) {
    //                     newResults[x] = Math.Min(results[x], mincount+1);
    //                 } else {
    //                     newResults.Add(x,mincount+1);
    //                 }
    //             }
    //             results = newResults;
    //         }
    //         total += results[0];
    //     }
    //     return total;
    // }

    // public int Part2(){
    //     return 0;
    // }

    // int test1() {
    //     parse2();
    //     int total = 0;

    //     for (int j = 0; j < n; j ++) {
    //         var targets = startingArrangements[j];
    //         int numBitsRow = numBits[j];
    //         var buttonsRow = buttons[j];
    //         int numButtonsRow = numButtons[j];
    //         // Console.WriteLine(numButtons);

    //         Solver solver = Solver.CreateSolver("SCIP");

    //         Variable[] ais = new Variable[numButtonsRow]; //coefficients
    //         for (int i = 0; i < numButtonsRow; i++){
    //             ais[i] = solver.MakeIntVar(0, 1, $"a_{i}");
    //         }
    //         Variable[] zs = new Variable[numBitsRow]; //offsets to deal with %2
    //         for (int b = 0; b < numBitsRow; b++){
    //             zs[b] = solver.MakeIntVar(0, numButtonsRow, $"z_{b}");
    //         }

    //         for (int b = 0; b < numBitsRow; b++) { // equation for each bit
    //             Constraint constraint = solver.MakeConstraint(targets[b], targets[b], "constraint");
    //             constraint.SetCoefficient(zs[b], -2);
    //             for (int i = 0; i < numButtonsRow; i++) {
    //                 constraint.SetCoefficient(ais[i], buttonsRow[i,b]);
    //             }
    //         }

    //         Objective obj = solver.Objective();
    //         foreach (var ai in ais) {
    //             obj.SetCoefficient(ai, 1);
    //         }
    //         obj.SetMinimization();
    //         solver.Solve();

    //         // Console.WriteLine("Solution:");
    //         // Console.WriteLine(solver.Objective().Value());
    //         // for(int k = 0; k < numButtons[j]; k++) {
    //         //     Console.WriteLine($"a_{k} = " + ais[k].SolutionValue());
    //         // }
    //         // for (int k = 0; k < numBits[j]; k++) {
    //         //     Console.WriteLine($"z_{k} = " + zs[k].SolutionValue());
    //         // }
    //         total += (int)solver.Objective().Value();
    //     }
    //     return total;
    // }

    // int test2() {
    //     // parse2();
    //     int total = 0;

    //     for (int j = 0; j < n; j ++) {
    //         var targets = finalTargets[j];
    //         int numBitsRow = numBits[j];
    //         var buttonsRow = buttons[j];
    //         int numButtonsRow = numButtons[j];
    //         // Console.WriteLine(numButtons);

    //         Solver solver = Solver.CreateSolver("SCIP");

    //         Variable[] ais = new Variable[numButtonsRow]; //coefficients
    //         for (int i = 0; i < numButtonsRow; i++){
    //             ais[i] = solver.MakeIntVar(0, double.PositiveInfinity, $"a_{i}");
    //         }

    //         for (int b = 0; b < numBitsRow; b++) { // equation for each bit
    //             Constraint constraint = solver.MakeConstraint(targets[b], targets[b], "constraint");
    //             for (int i = 0; i < numButtonsRow; i++) {
    //                 constraint.SetCoefficient(ais[i], buttonsRow[i,b]);
    //             }
    //         }

    //         Objective obj = solver.Objective();
    //         foreach (var ai in ais) {
    //             obj.SetCoefficient(ai, 1);
    //         }
    //         obj.SetMinimization();
    //         solver.Solve();
    //         total += (int)solver.Objective().Value();
    //     }
    //     return total;
    // }

    // int test2(){
    //     int total = 0;

    //     int[] targets = [3,5,4,7];
    //     int numBits = targets.Length;
    //     int[,] buttons = {{0,0,0,1},{0,1,0,1},{0,0,1,0},{0,0,1,1},{1,0,1,0},{1,1,0,0}};
    //     int numButtons = buttons.Length/numBits;
    //     // Console.WriteLine(numButtons);

    //     Solver solver = Solver.CreateSolver("GLOP");

    //     Variable[] ais = new Variable[numButtons]; //coefficients
    //     for (int i = 0; i < numButtons; i++){
    //         ais[i] = solver.MakeIntVar(0, double.PositiveInfinity, $"a_{i}");
    //     }

    //     for (int b = 0; b < numBits; b++) { // equation for each bit
    //         Constraint constraint = solver.MakeConstraint(targets[b], targets[b], "constraint");
    //         for (int i = 0; i < numButtons; i++) {
    //             constraint.SetCoefficient(ais[i], buttons[i,b]);
    //         }
    //     }

    //     Objective obj = solver.Objective();
    //     foreach (var ai in ais) {
    //         obj.SetCoefficient(ai, 1);
    //     }
    //     obj.SetMinimization();
    //     solver.Solve();

    //     total += (int)solver.Objective().Value();
    //     return total;
    // }

    public int bitsum(int x) {
        int sum = 0;
        int temp = x;
        while (temp > 0) {
            sum += temp%2;
            temp = temp/2;
        }
        return sum;
    }

    // TODO: verify correctness, return pivot/free variables
    List<int> eliminationBool (int[,] matrix) {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);
        bool[] isPivot = new bool[cols-1];

        for (int i = 0; i < rows; i++) {
            int firstNonZero = -1;
            bool hasPivot = false;
            for (int j = 0; j < cols; j++) {
                if (matrix[i,j] != 0) {
                    hasPivot = true;
                    firstNonZero = j;
                    isPivot[j] = true;
                    break;
                }
            }
            if (hasPivot) {
                for (int k = 0; k < rows; k++) {
                    if (i == k) continue;
                    if (matrix[k, firstNonZero] != 0) {
                        for (int j = 0; j < cols; j++) {
                            matrix[k,j] = matrix[k,j] ^ matrix[i,j];
                        }
                    }
                }
            }
        }

        List<int> freeVariables = new List<int>();
        for (int j = 0; j < cols-1; j++) {
            if (!isPivot[j]) {
                freeVariables.Add(j);
            }
        }
        return freeVariables;
    }

    int bruteForceFreeVariablesBoolean(int[,] matrix, List<int> freeVariables) {
        // Console.Write("The free variables are:");
        // foreach (var word in freeVariables) {
        //     Console.Write(word + ", ");
        // }
        // Console.Write('\n');
        int n = freeVariables.Count;
        int smallest = 0;
        for (int i = 0; i < matrix.GetLength(0); i++) {
            smallest += matrix[i, matrix.GetLength(1)-1] ;
        }
        int k = 1; //subsets with k elements
        while(k < smallest) {
            int[] indicesOfSubset = new int[k];
            for (int i = 0; i < k; i++) indicesOfSubset[i] = i;

            while(k <= n) {
                //calculate number of button presses for the subset of free variables
                int sum = k;
                for (int i = 0; i < matrix.GetLength(0); i++) {
                    int digit = matrix[i, matrix.GetLength(1)-1];
                    for (int j = 0; j < k; j++) {
                        
                        digit = digit ^ matrix[i, freeVariables[indicesOfSubset[j]]];
                    }
                    sum += digit;
                }

                if (sum < smallest) {
                    smallest = sum;
                }
                if (k < smallest) break;

                //update subset
                int indexToChange = k - 1;
                for (; indexToChange >= 0 && indicesOfSubset[indexToChange] == n - k + indexToChange; indexToChange--) ;
                if (indexToChange < 0) break;

                indicesOfSubset[indexToChange]++;
                for (int j = indexToChange + 1; j < k; j++) {
                    indicesOfSubset[j] = indicesOfSubset[j-1]+1;
                }
            }            
            k++;
        }
        return smallest;
    }

    // TODO: Implement
    int[,] invertMatrix (int[,] matrix) {
        return matrix;
    }

    // TODO: Implement
    int[] multiplyMatrixVector (int[,] matrix, int[] b) {
        return b;
    }

    static void printMatrix(int[,] matrix) {
        if (matrix == null)
        {
            Console.WriteLine("(null)");
            return;
        }

        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write($"{matrix[i, j],6}");
            }
            Console.WriteLine();
        }
    }

    public int part1ManualMatrix() {
        parse2();
        int total = 0;
        for (int i = 0; i < n; i ++) {
            // printMatrix(buttons[i]);
            // Console.WriteLine("reduced to");
            List<int> freeVars = eliminationBool(buttons[i]);
            // printMatrix(buttons[i]);
            total += bruteForceFreeVariablesBoolean(buttons[i], freeVars);
        }
        return total;
    }

    public int part2Placeholder() {
        return 0;
    }
}
