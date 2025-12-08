namespace AdventOfCode;

public class Day08 : BaseDay
{
    private readonly string[] _input;
    private long[] pairwiseDistances;
    private int[] pairwiseDistanceIndices;
    private int numPoints;
    private long[] xs;
    private long[] ys;
    private long[] zs;

    public Day08()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new($"{Part1()}");

    public override ValueTask<string> Solve_2() => new($"{Part2()}");

    public long distance (long x1, long y1, long z1, long x2, long y2, long z2) {
        return (x1-x2)*(x1-x2) + (y1-y2)*(y1-y2) + (z1-z2)*(z1-z2);
    }

    public void process() {
        numPoints = _input.Length;
        xs = new long[numPoints];
        ys = new long[numPoints];
        zs = new long[numPoints];
        pairwiseDistances = new long[numPoints*(numPoints-1)/2];
        pairwiseDistanceIndices = new int[numPoints*(numPoints-1)/2];
        for (int i = 0; i < pairwiseDistanceIndices.Length; i++) {
            pairwiseDistanceIndices[i] = i;
        }

        long temp;
        for (int i = 0; i < numPoints; i++) {
            string[] nums = _input[i].Split(',');
            temp = 0;

            // parse input into xs, ys, zs
            for (int j = 0; j < nums[0].Length; j++) {
                temp = temp*10 + nums[0][j] - '0';
            }
            xs[i] = temp;

            temp = 0;
            for (int j = 0; j < nums[1].Length; j++) {
                temp = temp*10 + nums[1][j] - '0';
            }
            ys[i] = temp;

            temp = 0;
            for (int j = 0; j < nums[2].Length; j++) {
                temp = temp*10 + nums[2][j] - '0';
            }
            zs[i] = temp;
        }

        int k = 0; // TODO: relate k to i and j.
        for (int i = 0; i < numPoints-1; i++) {
            for (int j = i+1; j < numPoints; j++) {
                pairwiseDistances[k] = distance(xs[i], ys[i], zs[i], xs[j], ys[j], zs[j]);
                k++;
            }
        }

        pairwiseDistanceIndices.Sort(customComparison);
    }

    int customComparison(int a, int b) {
        if (pairwiseDistances[a] == pairwiseDistances[b]) {
            return 0;
        }
        if (pairwiseDistances[a] < pairwiseDistances[b]) {
            return -1;
        }
        return 1;
    }

    public int Part1(){
        process();

        List<List<int>> listOfCliques = new List<List<int>>();
        List<int> encountered = new List<int>();

        // Console.WriteLine("starting to find edges");
        for (int k2 = 0; k2 < 1000; k2 ++) {
            // recover i, j from k = pairwiseDistanceIndices[k2]
            int i = 0;
            int j = pairwiseDistanceIndices[k2];
            while (j >= numPoints - i - 1) {
                j -= (numPoints - i - 1);
                i++;
            }
            j+= i+1;

            // Console.WriteLine("The " + k2 + " edge is " + i + " and " + j);

            if (encountered.Contains(i) && encountered.Contains(j)) {
                // Console.WriteLine("Found both indices " + i + " and " + j);
                for (int m = 0; m < listOfCliques.Count; m++) {
                    //i and j in same clique
                    if (listOfCliques[m].Contains(i) && listOfCliques[m].Contains(j)) {
                        // Console.WriteLine("THIS HAPPENED");
                        break;
                    }
                    //i and j in seperate cliques, merge and remove
                    else if (listOfCliques[m].Contains(i)) {
                        for (int n = m+1; n < listOfCliques.Count; n++) {
                            if (listOfCliques[n].Contains(j)) {
                                listOfCliques[m].AddRange(listOfCliques[n]);
                                listOfCliques.RemoveAt(n);
                                break;
                            }
                        }
                        break;
                    } else if (listOfCliques[m].Contains(j)) {
                        for (int n = m+1; n < listOfCliques.Count; n++) {
                            if (listOfCliques[n].Contains(i)) {
                                listOfCliques[m].AddRange(listOfCliques[n]);
                                listOfCliques.RemoveAt(n);
                                break;
                            }
                        }
                        break;
                    }
                }
            // one index part of clique, add other
            } else if (encountered.Contains(i)) {
                foreach (var clique in listOfCliques) {
                    if (clique.Contains(i)) {
                        clique.Add(j);
                        break;
                    }
                }
                encountered.Add(j);
            } else if (encountered.Contains(j)) {
                foreach (var clique in listOfCliques) {
                    if (clique.Contains(j)) {
                        clique.Add(i);
                        break;
                    }
                }
                encountered.Add(i);
            // neither index part of clique
            } else {
                List<int> newClique = new List<int> {i, j};
                listOfCliques.Add(newClique);
                encountered.Add(i);
                encountered.Add(j);
            }
            // Console.WriteLine("this many nontrivial cliques:" + listOfCliques.Count);
            // foreach (var clique in listOfCliques) {
            //     Console.WriteLine("clique contains " + clique.Count + " nodes");
            // }
            
        }
        
        listOfCliques.Sort((a,b) => b.Count.CompareTo(a.Count)); // dont do this to find the biggest 3.
        return listOfCliques[0].Count*listOfCliques[1].Count*listOfCliques[2].Count;
    }

    public long Part2(){
        List<List<int>> listOfCliques = new List<List<int>>();
        List<int> encountered = new List<int>();

        int k2 = 0;
        while(true) {
            // recover i, j from k = pairwiseDistanceIndices[k2]
            int i = 0;
            int j = pairwiseDistanceIndices[k2];
            while (j >= numPoints - i - 1) {
                j -= (numPoints - i - 1);
                i++;
            }
            j+= i+1;

            // Console.WriteLine("The " + k2 + " edge is " + i + " and " + j);

            if (encountered.Contains(i) && encountered.Contains(j)) {
                // Console.WriteLine("Found both indices " + i + " and " + j);
                for (int m = 0; m < listOfCliques.Count; m++) {
                    //i and j in same clique
                    if (listOfCliques[m].Contains(i) && listOfCliques[m].Contains(j)) {
                        // Console.WriteLine("THIS HAPPENED");
                        break;
                    }
                    //i and j in seperate cliques, merge and remove
                    else if (listOfCliques[m].Contains(i)) {
                        for (int n = m+1; n < listOfCliques.Count; n++) {
                            if (listOfCliques[n].Contains(j)) {
                                listOfCliques[m].AddRange(listOfCliques[n]);
                                listOfCliques.RemoveAt(n);
                                break;
                            }
                        }
                        break;
                    } else if (listOfCliques[m].Contains(j)) {
                        for (int n = m+1; n < listOfCliques.Count; n++) {
                            if (listOfCliques[n].Contains(i)) {
                                listOfCliques[m].AddRange(listOfCliques[n]);
                                listOfCliques.RemoveAt(n);
                                break;
                            }
                        }
                        break;
                    }
                }
            // one index part of clique, add other
            } else if (encountered.Contains(i)) {
                foreach (var clique in listOfCliques) {
                    if (clique.Contains(i)) {
                        clique.Add(j);
                        break;
                    }
                }
                encountered.Add(j);
            } else if (encountered.Contains(j)) {
                foreach (var clique in listOfCliques) {
                    if (clique.Contains(j)) {
                        clique.Add(i);
                        break;
                    }
                }
                encountered.Add(i);
            // neither index part of clique
            } else {
                List<int> newClique = new List<int> {i, j};
                listOfCliques.Add(newClique);
                encountered.Add(i);
                encountered.Add(j);
            }
            if (listOfCliques[0].Count == numPoints) {
                // finished, do smth with i and j
                return xs[i]*xs[j];
            }
            k2++;
        }
    }
}
