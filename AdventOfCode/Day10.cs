namespace AdventOfCode;

using Google.OrTools.Init;
using Google.OrTools.LinearSolver;

public class Day10 : BaseDay
{
    private readonly string[] _input;
    // int n;
    // private int[] startingArrangements;
    // private List<int>[] buttons;
    // private List<int>[] targets;

    public Day10()
    {
        _input = File.ReadAllLines(InputFilePath);
        // n = _input.Length;
        // startingArrangements = new int[n];
        // buttons = new List<int>[n];
        // targets = new List<int>[n];
    }

    public override ValueTask<string> Solve_1() => new($"{test2()}");

    public override ValueTask<string> Solve_2() => new($"{test2()}");

    // void parse() {
    //     for (int i = 0; i < n; i++) {
    //         string line = _input[i];
    //         int j = 1;

    //         // parse .# part
    //         int arrangement = 0;
    //         for (; j < line.Length; j++) {
    //             if (line[j] == ']') break;
    //             arrangement = arrangement << 1;
    //             if (line[j] == '#') {
    //                 arrangement ++;
    //             }
    //         }
    //         startingArrangements[i] = arrangement;
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



    int test2(){
        int total = 0;

        int[] targets = [0,1,1,0];
        int numBits = targets.Length;
        int[,] buttons = {{0,0,0,1},{0,1,0,1},{0,0,1,0},{0,0,1,1},{1,0,1,0},{1,1,0,0}};
        int numButtons = buttons.Length/numBits;
        Console.WriteLine(numButtons);

        Solver solver = Solver.CreateSolver("GLOP");

        Variable[] ais = new Variable[numButtons]; //coefficients
        for (int i = 0; i < numButtons; i++){
            ais[i] = solver.MakeIntVar(0, 1, $"a_{i}");
        }
        Variable[] zs = new Variable[numBits]; //offsets to deal with %2
        for (int b = 0; b < numBits; b++){
            zs[b] = solver.MakeIntVar(0, numButtons, $"z_{b}");
            
        }

        for (int b = 0; b < numBits; b++) { // equation for each bit
            Constraint constraint = solver.MakeConstraint(targets[b], targets[b], "constraint");
            constraint.SetCoefficient(zs[b], -2);
            for (int i = 0; i < numButtons; i++) {
                constraint.SetCoefficient(ais[i], buttons[i,b]);
            }
        }

        Objective obj = solver.Objective();
        foreach (var ai in ais) {
            obj.SetCoefficient(ai, 1);
        }
        obj.SetMinimization();
        solver.Solve();

        total += (int)solver.Objective().Value();
        return total;
    }
}
