namespace AdventOfCode;

public class Day11 : BaseDay
{
    private readonly string[] _input;
    Dictionary<string, HashSet<string>> outEdges;
    Dictionary<string, HashSet<string>> inEdges;

    public Day11()
    {
        _input = File.ReadAllLines(InputFilePath);
        outEdges = new Dictionary<string, HashSet<string>>();
        inEdges = new Dictionary<string, HashSet<string>>();
    }

    public override ValueTask<string> Solve_1() => new($"{Part1()}");

    public override ValueTask<string> Solve_2() => new($"{Part2()}");

    public void parse() {
        foreach (var line in _input) {
            string[] ids = line.Split(new char[] {':', ' '});
            var temp = new HashSet<string>();
            for (int i = 2; i < ids.Length; i++) {
                temp.Add(ids[i]);
            }
            outEdges[ids[0]] = temp;
            for (int i = 2; i < ids.Length; i++) {
                if (inEdges.ContainsKey(ids[i])) {
                    inEdges[ids[i]].Add(ids[0]);
                } else {
                    inEdges[ids[i]] = new HashSet<string>(new string[]{ids[0]});
                }
            }
        }
        outEdges["out"] = new HashSet<string>();
    }

    public HashSet<string> reachable(string start) {
        var q = new Queue<string>();
        var reachables = new HashSet<string>();
        q.Enqueue(start);
        while(q.Count > 0) {
            var pop = q.Dequeue();
            if (reachables.Contains(pop)) continue;
            reachables.Add(pop);
            foreach (var outedge in outEdges[pop]) {
                q.Enqueue(outedge);
            }
        }
        return reachables;
    }

    public int pathsBetween(string start, string end) {
        var values = new Dictionary<string, int>();
        values[start] = 1;

        var reachables = reachable(start);
        var q = new Queue<string>();
        q.Enqueue(start);
        var visited = new HashSet<string>();
        
        while(q.Count > 0) {
            var curr = q.Dequeue();
            visited.Add(curr);
            if (curr == end) {
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
        return values[end];
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
