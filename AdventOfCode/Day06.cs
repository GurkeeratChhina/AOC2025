namespace AdventOfCode;

public class Day06 : BaseDay
{
    private readonly string[] _input;
    List<string> nums1;
    List<string> nums2;
    List<string> nums3;
    List<string> nums4;
    List<string> ops;

    public Day06()
    {
        _input = File.ReadAllLines(InputFilePath);

        string line1 = _input[0];
        string line2 = _input[1];
        string line3 = _input[2];
        string line4 = _input[3];
        string line5 = _input[4];

        nums1 = new List<string>();
        nums2 = new List<string>();
        nums3 = new List<string>();
        nums4 = new List<string>();
        ops = new List<string>();

        string string1 = "";
        string string2 = "";
        string string3 = "";
        string string4 = "";
        string string5 = "";
        for (int i = 0; i<line1.Length; i++) {
            if (line1[i] == ' ' && line2[i] == ' ' && line3[i] == ' ' && line4[i] == ' ' && line5[i] == ' ') {
                nums1.Add(string1);
                nums2.Add(string2);
                nums3.Add(string3);
                nums4.Add(string4);
                ops.Add(string5);

                string1 = "";
                string2 = "";
                string3 = "";
                string4 = "";
                string5 = "";
            } else {
                if (line1[i] == ' ') {
                    // string1 += '0';
                } else {
                    string1 += line1[i];
                }

                if (line2[i] == ' ') {
                    // string2 += '0';
                } else {
                    string2 += line2[i];
                }

                if (line3[i] == ' ') {
                    // string3 += '0';
                } else {
                    string3 += line3[i];
                }

                if (line4[i] == ' ') {
                    // string4 += '0';
                } else {
                    string4 += line4[i];
                }

                if (line5[i] == ' ') {
                } else {
                    string5 += line5[i];
                }
            }
            
        }
        nums1.Add(string1);
        nums2.Add(string2);
        nums3.Add(string3);
        nums4.Add(string4);
        ops.Add(string5);
    }

    public override ValueTask<string> Solve_1() => new($"{Part1()}");

    public override ValueTask<string> Solve_2() => new($"{Part2()}");

    public long Part1(){
        long total = 0;
        long num1;
        long num2;
        long num3;
        long num4;

        for (int i = 0; i < ops.Count; i++) {
            num1 = long.Parse(nums1[i].Trim('0'));
            num2 = long.Parse(nums2[i].Trim('0'));
            num3 = long.Parse(nums3[i].Trim('0'));
            num4 = long.Parse(nums4[i].Trim('0'));
            
            // Console.WriteLine(nums1[i] + " turns into " + num1);
            // Console.WriteLine(ops[i]);

            if (ops[i] == "+") {
                // Console.WriteLine("adding");
                total += num1 + num2 + num3 + num4;

            } else {
                total += num1 * num2 * num3 * num4;
            }
            // Console.WriteLine(total);
        }

        return total;
    }

    public long Part1Refactored() {
        long total = 0;
        long subtotal;
        List<long> nums = new List<long>();

        string operations = _input[^1].Replace(" ", ""); 

        for (int i = 0; i < operations.Length; i++) {
            nums.Add(long.Parse(nums1[i]));
            nums.Add(long.Parse(nums2[i]));
            nums.Add(long.Parse(nums3[i]));
            nums.Add(long.Parse(nums4[i]));
            
            if (operations[i] == '+') {
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
        }

        return total;
    }

    public int Part2(){
        return 0;
    }
}
