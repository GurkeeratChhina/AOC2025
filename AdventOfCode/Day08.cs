namespace AdventOfCode;

public class Day08 : BaseDay
{
    private readonly string[] _input;
    private long[] pairwiseDistances;
    private int numPoints;
    private int[] xs;
    private int[] ys;
    private int[] zs;

    private int[] maxHeap;
    private int heapSize;
    private int ArbitraryCutoff = 10000;

    List<List<int>> listOfCliques;
    List<int> encountered;

    public Day08()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new($"{Part1Faster()}");

    public override ValueTask<string> Solve_2() => new($"{Part2Faster()}");

    public long distance (int x1, int y1, int z1, int x2, int y2, int z2) {
        return (long)(x1-x2)*(long)(x1-x2) + (long)(y1-y2)*(long)(y1-y2) + (long)(z1-z2)*(long)(z1-z2);
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

    public void process2() {
        numPoints = _input.Length;
        xs = new int[numPoints];
        ys = new int[numPoints];
        zs = new int[numPoints];
        pairwiseDistances = new long[numPoints*(numPoints-1)/2];
        maxHeap = new int[ArbitraryCutoff];
        heapSize = 0;

        //parse to x,y,z
        for (int i = 0; i < numPoints; i++) {
            string[] nums = _input[i].Split(',');
            xs[i] = int.Parse(nums[0]);
            ys[i] = int.Parse(nums[1]);
            zs[i] = int.Parse(nums[2]);
        }

        int k = 0; // flattened i and j
        for (int i = 0; i < numPoints-1; i++) {
            for (int j = i+1; j < numPoints; j++) {
                pairwiseDistances[k] = distance(xs[i], ys[i], zs[i], xs[j], ys[j], zs[j]);
                addToHeap(k);
                k++;
            }
        }

        maxHeap.Sort(customComparison);

        listOfCliques = new List<List<int>>();
        encountered = new List<int>();
    }

    public void replaceBiggestOfHeap(int s) {
        int current = 0;
        int leftIndex, rightIndex, swap;

        //bubble down
        while (true) {
            leftIndex = current*2 + 1;
            rightIndex = leftIndex+1;
            if (leftIndex >= ArbitraryCutoff) break;
            if (rightIndex >= ArbitraryCutoff) { // only left child exists
                long leftVal = pairwiseDistances[maxHeap[leftIndex]];
                if (s > leftVal) break;
                swap = leftIndex;
            } else { // both children exist
                long leftVal = pairwiseDistances[maxHeap[leftIndex]];
                long rightVal = pairwiseDistances[maxHeap[rightIndex]];
                if (s > leftVal && s > rightVal) break;
                if (leftVal > rightVal) {
                    swap = leftIndex;
                } else {
                    swap = rightIndex;
                }
            }

           maxHeap[current] = maxHeap[swap];
           current = swap;
        }
        maxHeap[current] = s;
    }

    public void insertEndOfHeap(int s) {
        int currentIndex = heapSize++;
        while(currentIndex > 0) {
            int parent = (currentIndex-1)/2;
            if (pairwiseDistances[maxHeap[parent]] > pairwiseDistances[s]) break;
            maxHeap[currentIndex] = maxHeap[parent];
            currentIndex = parent;
        }
        maxHeap[currentIndex] = s;
    }

    public void addToHeap(int s) {
        if (heapSize < ArbitraryCutoff) {
            insertEndOfHeap(s);
        } else {
            if (pairwiseDistances[s] < pairwiseDistances[maxHeap[0]]) {
                replaceBiggestOfHeap(s);
            }
        }
    }

    public void addEdge(int i, int j) {
        if (encountered.Contains(i) && encountered.Contains(j)) {
            for (int m = 0; m < listOfCliques.Count; m++) {
                //i and j in same clique
                if (listOfCliques[m].Contains(i) && listOfCliques[m].Contains(j)) {
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
    }

    public int Part1Faster() {
        process2();
        // Console.WriteLine(maxHeap[0]);

        for (int k2 = 1; k2 < 1001; k2 ++) {
            // recover i, j from k = pairwiseDistanceIndices[k2]
            int i = 0;
            int j = maxHeap[k2];
            while (j >= numPoints - i - 1) {
                j -= (numPoints - i - 1);
                i++;
            }
            j+= i+1;

            addEdge(i,j);
        }
        int biggest = 0;
        int secondBiggest = 0;
        int thirdBiggest = 0;

        foreach (var clique in listOfCliques) {
            int s = clique.Count;
            if (s > biggest) {
                thirdBiggest = secondBiggest;
                secondBiggest = biggest;
                biggest = s;

            } else if (s > secondBiggest) {
                thirdBiggest = secondBiggest;
                secondBiggest = s;

            } else if (s > thirdBiggest) {
                thirdBiggest = s;
            }
        }
        return biggest*secondBiggest*thirdBiggest;
    }

    public long Part2Faster(){
        int k2 = 1001;
        while(true) {
            int i = 0;
            int j = maxHeap[k2];
            while (j >= numPoints - i - 1) {
                j -= (numPoints - i - 1);
                i++;
            }
            j+= i+1;

            addEdge(i, j);
            if (listOfCliques[0].Count == numPoints) {
                return (long)xs[i]*(long)xs[j];
            }
            k2++;
        }
    }
}
