namespace AdventOfCode;

public class Day02 : BaseDay
{
    private readonly string _input;

    public Day02()
    {
        _input = File.ReadAllText(InputFilePath).Trim();
    }

    public override ValueTask<string> Solve_1() => new($"{Part1()}");

    public override ValueTask<string> Solve_2() => new($"{Part2()}");

    public long Part1(){
        long total = 0;
        foreach (var range in _input.Split(',')) {
            var nums = range.Split('-');
            long lower;
            long upper;
            int n;
            int m;

            // Console.WriteLine(nums[0] + " " + nums[1]);
            if (nums[0].Length %2 == 1) {
                lower = (long)Math.Pow(10, nums[0].Length);
                n = nums[0].Length +1;
            } else {
                lower = long.Parse(nums[0]);
                n = nums[0].Length;
            }
            if (nums[1].Length %2 == 1) {
                upper = (long)Math.Pow(10, nums[1].Length - 1) -1;
                m = nums[1].Length - 1;
            } else {
                upper = long.Parse(nums[1]);
                m = nums[1].Length;
            }
            // Console.WriteLine(lower + " " + upper);
            long lowerFirst = lower/(long)Math.Pow(10, n/2);
            long upperFirst = upper/(long)Math.Pow(10,m/2); 
            long lowerSecond = lower%(long)Math.Pow(10, n/2);
            long upperSecond = upper%(long)Math.Pow(10,m/2); 
            // Console.WriteLine(upperFirst - lowerFirst);
            
            if (lowerFirst == upperFirst) {
                if (lowerFirst >= lowerSecond && lowerFirst <= upperSecond) {
                    var toRepeat = lowerFirst.ToString();
                    var repeated = toRepeat+toRepeat;
                    total += long.Parse(repeated);
                }
            } else if (lowerFirst < upperFirst) {
                if (lowerFirst >= lowerSecond) {
                    var toRepeat = lowerFirst.ToString();
                    var repeated = toRepeat+toRepeat;
                    total += long.Parse(repeated);
                }
                if (upperFirst <= upperSecond) {
                    var toRepeat = upperFirst.ToString();
                    var repeated = toRepeat+toRepeat;
                    total += long.Parse(repeated);
                }
            }
            for (long i = lowerFirst + 1; i <= upperFirst - 1; i++) {
                var toRepeat = i.ToString();
                var repeated = toRepeat+toRepeat;
                total += long.Parse(repeated);
                // Console.WriteLine("increasing total");
            }
            
        }

        return total;
    }

    public long Part2(){
        long total = 0;
        foreach (var range in _input.Split(',')) {
            var nums = range.Split('-');
            long lower = long.Parse(nums[0]);
            long upper = long.Parse(nums[1]);
            int n = nums[0].Length;
            int m = nums[1].Length;
            if (m > 10) {
                Console.WriteLine("Error - values larger than code expects");
            }
            // Console.WriteLine(lower + " " + upper);
            if (n==m) {
                for (int i = 1; i < n; i++) {
                    if (n%i == 0) { // i is a proper factor of n
                        if (i ==2 && n == 8) { // factor of 2 counted with 4
                            continue;
                        }
                        if (i == 1 && (n==4 || n == 8 || n==9)) { //factor of 1 counted among larger factors
                            continue;
                        }
                        long lowerStart = long.Parse(nums[0].Substring(0,i));
                        long upperStart = long.Parse(nums[1].Substring(0,i)); 
                        if (i == 1 && (n ==6 || n==10)) { //factor of 1 being double counted and needs to be subtracted
                            string toRepeat = lowerStart.ToString();
                            string repeated = string.Concat(Enumerable.Repeat(toRepeat, n/i));
                            long repeatedNum = long.Parse(repeated);
                            if (repeatedNum >= lower && repeatedNum <= upper) {
                                total -= repeatedNum;
                                // Console.WriteLine("double counted, subtracting a" + repeatedNum);
                            }
                            if (lowerStart != upperStart) {
                                toRepeat = upperStart.ToString();
                                repeated = string.Concat(Enumerable.Repeat(toRepeat, n/i));
                                repeatedNum = long.Parse(repeated);
                                if (repeatedNum <= upper) {
                                    total -= repeatedNum;
                                    // Console.WriteLine("double counted, subtracting b" + repeatedNum);
                                }
                            }
                            for (long j = lowerStart+1; j <= upperStart -1 ; j++) {
                                toRepeat = j.ToString();
                                repeated = string.Concat(Enumerable.Repeat(toRepeat, n/i));
                                repeatedNum = long.Parse(repeated);
                                total -= repeatedNum;
                                // Console.WriteLine("double counted, subtracting c" + repeatedNum);
                            }
                        } else { //add number
                            string toRepeat = lowerStart.ToString();
                            string repeated = string.Concat(Enumerable.Repeat(toRepeat, n/i));
                            long repeatedNum = long.Parse(repeated);
                            if (repeatedNum >= lower && repeatedNum <= upper) {
                                total += repeatedNum;
                                // Console.WriteLine("first counted, adding a" + repeatedNum);
                            }
                            if (lowerStart != upperStart) {
                                toRepeat = upperStart.ToString();
                                repeated = string.Concat(Enumerable.Repeat(toRepeat, n/i));
                                repeatedNum = long.Parse(repeated);
                                if (repeatedNum <= upper) {
                                    total += repeatedNum;
                                    // Console.WriteLine("first counted, adding b" + repeatedNum);
                                }
                            }
                            for (long j = lowerStart+1; j <= upperStart -1 ; j++) {
                                toRepeat = j.ToString();
                                repeated = string.Concat(Enumerable.Repeat(toRepeat, n/i));
                                repeatedNum = long.Parse(repeated);
                                total += repeatedNum;
                                // Console.WriteLine("first counted, adding c" + repeatedNum);
                            }
                        }
                    }
                }
            } else if (m-n == 1) {
                long mid1 = (long)Math.Pow(10, n) -1;
                long mid2 = (long)Math.Pow(10, n);
                //do the rest of this case as the n=m case but after splitting up range

                //first half of range, replace upper with mid1
                // Console.WriteLine("different lengths, splitting" + lower + " " + mid1);
                for (int i = 1; i < n; i++) {
                    if (n%i == 0) { // i is a proper factor of n
                        if (i ==2 && n == 8) { // factor of 2 counted with 4
                            continue;
                        }
                        if (i == 1 && (n==4 || n == 8 || n==9)) { //factor of 1 counted among larger factors
                            continue;
                        }
                        long lowerStart = long.Parse(nums[0].Substring(0,i));
                        long upperStart = long.Parse(mid1.ToString().Substring(0,i)); 
                        if (i == 1 && (n ==6 || n==10)) { //factor of 1 being double counted and needs to be subtracted
                            string toRepeat = lowerStart.ToString();
                            string repeated = string.Concat(Enumerable.Repeat(toRepeat, n/i));
                            long repeatedNum = long.Parse(repeated);
                            if (repeatedNum >= lower && repeatedNum <= mid1) {
                                total -= repeatedNum;
                                // Console.WriteLine("double counted, subtracting a" + repeatedNum);
                            }
                            if (lowerStart != upperStart) {
                                toRepeat = upperStart.ToString();
                                repeated = string.Concat(Enumerable.Repeat(toRepeat, n/i));
                                repeatedNum = long.Parse(repeated);
                                if (repeatedNum <= mid1) {
                                    total -= repeatedNum;
                                    // Console.WriteLine("double counted, subtracting b" + repeatedNum);
                                }
                            }
                            for (long j = lowerStart+1; j <= upperStart -1 ; j++) {
                                toRepeat = j.ToString();
                                repeated = string.Concat(Enumerable.Repeat(toRepeat, n/i));
                                repeatedNum = long.Parse(repeated);
                                total -= repeatedNum;
                                // Console.WriteLine("double counted, subtracting c" + repeatedNum);
                            }
                        } else { //add number
                            string toRepeat = lowerStart.ToString();
                            string repeated = string.Concat(Enumerable.Repeat(toRepeat, n/i));
                            long repeatedNum = long.Parse(repeated);
                            if (repeatedNum >= lower && repeatedNum <= mid1) {
                                total += repeatedNum;
                                // Console.WriteLine("first counted, adding a" + repeatedNum);
                            }
                            if (lowerStart != upperStart) {
                                toRepeat = upperStart.ToString();
                                repeated = string.Concat(Enumerable.Repeat(toRepeat, n/i));
                                repeatedNum = long.Parse(repeated);
                                if (repeatedNum <= mid1) {
                                    total += repeatedNum;
                                    // Console.WriteLine("first counted, adding b" + repeatedNum);
                                }
                            }
                            for (long j = lowerStart+1; j <= upperStart -1 ; j++) {
                                toRepeat = j.ToString();
                                repeated = string.Concat(Enumerable.Repeat(toRepeat, n/i));
                                repeatedNum = long.Parse(repeated);
                                total += repeatedNum;
                                // Console.WriteLine("first counted, adding c" + repeatedNum);
                            }
                        }
                    }
                }

                //second half of range, replace lower with mid2
                // Console.WriteLine("different lengths, splitting" + mid2 + " " + upper);
                for (int i = 1; i < m; i++) {
                    if (m%i == 0) { // i is a proper factor of n
                        if (i ==2 && m == 8) { // factor of 2 counted with 4
                            continue;
                        }
                        if (i == 1 && (m==4 || m == 8 || m==9)) { //factor of 1 counted among larger factors
                            continue;
                        }
                        long lowerStart = long.Parse(mid2.ToString().Substring(0,i));
                        long upperStart = long.Parse(nums[1].Substring(0,i)); 
                        if (i == 1 && (m ==6 || m==10)) { //factor of 1 being double counted and needs to be subtracted
                            string toRepeat = lowerStart.ToString();
                            string repeated = string.Concat(Enumerable.Repeat(toRepeat, m/i));
                            long repeatedNum = long.Parse(repeated);
                            if (repeatedNum >= mid2 && repeatedNum <= upper) {
                                total -= repeatedNum;
                                // Console.WriteLine("double counted, subtracting a" + repeatedNum);
                            }
                            if (lowerStart != upperStart) {
                                toRepeat = upperStart.ToString();
                                repeated = string.Concat(Enumerable.Repeat(toRepeat, m/i));
                                repeatedNum = long.Parse(repeated);
                                if (repeatedNum <= upper) {
                                    total -= repeatedNum;
                                    // Console.WriteLine("double counted, subtracting b" + repeatedNum);
                                }
                            }
                            for (long j = lowerStart+1; j <= upperStart -1 ; j++) {
                                toRepeat = j.ToString();
                                repeated = string.Concat(Enumerable.Repeat(toRepeat, m/i));
                                repeatedNum = long.Parse(repeated);
                                total -= repeatedNum;
                                // Console.WriteLine("double counted, subtracting c" + repeatedNum);
                            }
                        } else { //add number
                            string toRepeat = lowerStart.ToString();
                            string repeated = string.Concat(Enumerable.Repeat(toRepeat, m/i));
                            long repeatedNum = long.Parse(repeated);
                            if (repeatedNum >= mid2 && repeatedNum <= upper) {
                                total += repeatedNum;
                                // Console.WriteLine("first counted, adding a" + repeatedNum);
                            }
                            if (lowerStart != upperStart) {
                                toRepeat = upperStart.ToString();
                                repeated = string.Concat(Enumerable.Repeat(toRepeat, m/i));
                                repeatedNum = long.Parse(repeated);
                                if (repeatedNum <= upper) {
                                    total += repeatedNum;
                                    // Console.WriteLine("first counted, adding b" + repeatedNum);
                                }
                            }
                            for (long j = lowerStart+1; j <= upperStart -1 ; j++) {
                                toRepeat = j.ToString();
                                repeated = string.Concat(Enumerable.Repeat(toRepeat, m/i));
                                repeatedNum = long.Parse(repeated);
                                total += repeatedNum;
                                // Console.WriteLine("first counted, adding c" + repeatedNum);
                            }
                        }
                    }
                }
                
            } else {
                Console.WriteLine("ERROR, UNEXPECTED CASE");
            }
        }
        return total;
    }
}
