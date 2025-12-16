namespace AdventOfCode;

public class Day11 : BaseDay
{
    private readonly string[] _input;
    Dictionary<int, HashSet<int>> outEdges;
    Dictionary<int, HashSet<int>> inEdges;

    public Day11()
    {
        _input = File.ReadAllLines(InputFilePath);
        outEdges = new Dictionary<int, HashSet<int>>();
        inEdges = new Dictionary<int, HashSet<int>>();
    }

    public override ValueTask<string> Solve_1() => new($"{Part1()}");

    public override ValueTask<string> Solve_2() => new($"{Part2()}");

    public int encode(ReadOnlySpan<Char> s) {
        return (s[0] - 'a') * 26 * 26 + (s[1] - 'a') * 26 + (s[2] - 'a');
    }

    public void parse() {
        foreach (var line in _input) {
            int start = encode(line.AsSpan(0,3));
            // Console.WriteLine(line + " " + start);
            var temp = new HashSet<int>();
            for (int i = 5; i < line.Length-2; i+=4) {
                int outEdge = encode(line.AsSpan(i,3));
                // Console.WriteLine(outEdge);
                temp.Add(outEdge);
                if (!inEdges.ContainsKey(outEdge)) {
                    inEdges[outEdge] = new HashSet<int>();
                }
                inEdges[outEdge].Add(start);
            }

            outEdges[start] = temp;
        }
    }

    public HashSet<int> reachable(string start, string end) {
        var q = new Queue<int>();
        var reachables = new HashSet<int>();
        q.Enqueue(encode(start.AsSpan()));
        while(q.Count > 0) {
            var curr = q.Dequeue();
            if (curr == encode(end.AsSpan())) {
                return reachables;
            }
            if (reachables.Contains(curr)) continue;
            reachables.Add(curr);
            foreach (var outedge in outEdges[curr]) {
                q.Enqueue(outedge);
            }
        }
        return reachables;
    }

    public int pathsBetween(string start, string end) {
        var values = new Dictionary<int, int>();
        values[encode(start.AsSpan())] = 1;

        var reachables = reachable(start, end);
        var visited = new HashSet<int>();

        var q = new Queue<int>();
        q.Enqueue(encode(start.AsSpan()));
        while(q.Count > 0) {
            var curr = q.Dequeue();
            visited.Add(curr);
            if (curr == encode(end.AsSpan())) {
                return values[curr];
            }
            foreach (var next in outEdges[curr]) {
                if (values.ContainsKey(next)) {
                    values[next] += values[curr];
                } else {
                    values[next] = values[curr];
                }
                bool allEdges = true;
                foreach (var edge in inEdges[next]) {
                    if (reachables.Contains(edge) && !visited.Contains(edge)) {
                        allEdges = false;
                        break;
                    }
                }
                if (allEdges) {
                    q.Enqueue(next);
                }
            }
        }
        return values[encode(end.AsSpan())];
    }

    public int Part1(){
        parse();
        return pathsBetween("you", "out");
    }

    public long Part2(){
        int arrangement1 = pathsBetween("fft", "dac");
        if (arrangement1 > 0) {
            return (long)pathsBetween("svr", "fft") * (long)arrangement1 * (long)pathsBetween("dac", "out");
        } else {
            return (long)pathsBetween("svr", "dac") * (long)pathsBetween("dac", "fft") * (long)pathsBetween("fft", "out");
        }

    }
}
