namespace AdventOfCode;

public class Day04 : BaseDay
{
    private readonly string[] _input;

    public Day04()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new($"{Part1()}");

    public override ValueTask<string> Solve_2() => new($"{Part2Faster5()}");

    public int Part1(){
        int height = _input.Length;
        int width = _input[0].Length;
        int count = 0;
        int surrounding;
        // Console.WriteLine(height + " " + width);
        for(int i = 0; i < height; i++) {
            // Console.WriteLine("row " + i + " is equal to " + _input[i]);
            for (int j = 0; j < width; j++) {
                // Console.WriteLine("looking at column " + j);
                if (_input[i][j] == '@') {
                    // Console.WriteLine("found an @");
                    surrounding = 0;
                    for (int dx = -1; dx <= 1; dx++) {
                        for (int dy = -1; dy <= 1; dy++) {
                            if (i + dy >= 0 && i + dy < height && j + dx >= 0 && j + dx < width && _input[i+dy][j+dx] == '@') {
                                surrounding++;
                            }
                        }
                    }
                    if (surrounding <5) {
                        count++;
                    }
                    // Console.WriteLine("evaluated column " + j + " new count is " + count);
                }
            }
        }
        return count;
    }

    public int Part2(){
        int height = _input.Length;
        int width = _input[0].Length;
        int total = 0;
        int count = 0;
        int surrounding;
        int[,] grid = new int[height, width]; //we will use int grid where 0 = `.`, 1 = `X`, and 2 = `@`.

        //populate grid with @'s from _input
        for (int i = 0; i < height; i++) {
            for (int j = 0; j < width; j++) {
                if (_input[i][j] == '@') {
                    grid[i,j] = 2;
                }
            }
        }
        
        //as long as we get a non-zero answer, repeat part1, marking 2's that can be removed as 1
        do {
            count = 0;
            for(int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    if (grid[i,j] == 2) {
                        surrounding = 0;
                        for (int dx = -1; dx <= 1; dx++) {
                            for (int dy = -1; dy <= 1; dy++) {
                                if (i + dy >= 0 && i + dy < height && j + dx >= 0 && j + dx < width && (grid[i+dy,j+dx] == 2 || grid[i+dy,j+dx] == 1)) {
                                    surrounding++;
                                }
                            }
                        }
                        if (surrounding <5) {
                            count++;
                            grid[i,j] = 1;
                        }
                    }
                }
            }
            total += count;
            
            //remove any 1's
            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    if (grid[i,j] == 1) {
                        grid[i,j] = 0;
                    }
                }
            }
        } while (count > 0);
        
        return total;
    }

    public int Part2Faster(){
        int height = _input.Length;
        int width = _input[0].Length;
        int total = 0;
        int count = 0;
        int surrounding;
        int[,] grid = new int[height, width]; //we will use int grid where 0 = `.`, 1 = `@`

        //populate grid with @'s from _input
        for (int i = 0; i < height; i++) {
            for (int j = 0; j < width; j++) {
                if (_input[i][j] == '@') {
                    grid[i,j] = 1;
                }
            }
        }
        
        //as long as we get a non-zero answer, repeat part1
        do {
            count = 0;
            for(int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    if (grid[i,j] > 0) {
                        surrounding = 0;
                        for (int dx = -1; dx <= 1; dx++) {
                            for (int dy = -1; dy <= 1; dy++) {
                                if (i + dy >= 0 && i + dy < height && j + dx >= 0 && j + dx < width && grid[i+dy,j+dx] > 0) {
                                    surrounding++;
                                }
                            }
                        }
                        if (surrounding <5) {
                            count++;
                            grid[i,j] = 0;
                        }
                    }
                }
            }
            total += count;
        } while (count > 0);
        
        return total;
    }

    public int Part2Faster2(){
        int height = _input.Length;
        int width = _input[0].Length;
        int count = 0;
        int surrounding;
        int[,] grid = new int[height, width];
        (int,int)[] dirs ={(-1,-1), (0,-1), (1,-1), (-1, 0), (1, 0), (-1, 1), (0, 1), (1, 1)};
        int nx;
        int ny;
        Queue<(int, int)> Q = new();

        void addNeighboursToQueue(int y, int x) {
            foreach (var (dx, dy) in dirs) {
                nx = x + dx;
                ny = y + dy;
                if (ny >= 0 && ny < height && nx >= 0 && nx < width && grid[ny,nx] >0) {
                    Q.Enqueue((ny,nx));
                }
            }
        }

        //populate grid, replacing @ with the number of neighbouring @'s
        for (int i = 0; i < height; i++) {
            for (int j = 0; j < width; j++) {
                if (_input[i][j] == '@') {
                    surrounding = 0;
                    foreach (var (dx, dy) in dirs) {
                        nx = j + dx;
                        ny = i + dy;
                        if (ny >= 0 && ny < height && nx >= 0 && nx < width && _input[ny][nx] == '@') {
                            surrounding++;
                        }
                    }
                    grid[i,j] = surrounding;
                }
            }
        }

        for(int i = 0; i < height; i++) {
            for (int j = 0; j < width; j++) {
                if (grid[i,j] > 0 && grid[i,j] < 4) {
                    count++;
                    grid[i,j] = 0;
                    addNeighboursToQueue(i,j);
                }
            }
        }

        while(Q.Count > 0) {
            var (y,x) = Q.Dequeue();
            if (grid[y,x] == 0) {
                continue;
            }
            grid[y,x]--;
            if (grid[y,x] < 4) {
                count++;
                grid[y,x] = 0;
                addNeighboursToQueue(y,x);
            }
        }
        
        return count;
    }

    public int Part2Faster3() {
        int height = _input.Length;
        int width = _input[0].Length;
        int size = height * width;
        int deaths = 0;
        
        byte[] grid = new byte[size];
        var q = new Stack<int>();

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                if (_input[y][x] == '@') {
                    byte count = 0;
                    for (int dx = -1; dx <= 1; dx++) {
                        for (int dy = -1; dy <= 1; dy++) {
                            int nx = x + dx;
                            int ny = y + dy;
                            
                            if ((uint)nx < (uint)width && (uint)ny < (uint)height) {
                                if (_input[ny][nx] == '@') {
                                    count++;
                                }
                            }
                        }   
                    }
                    grid[y*width+x] = count;
                    if (count < 5) {
                        q.Push(y*width+x);
                    }                     
                }
            }
        }

        while (q.Count > 0) {
            int i = q.Pop();
            if (grid[i] == 0) continue;
            grid[i] = 0;
            deaths++;

            int y = i / width;
            int x = i - y * width; 

            for (int dx = -1; dx <= 1; dx++) {
                for (int dy = -1; dy <= 1; dy++) {
                    int nx = x + dx;
                    int ny = y + dy;
                    if ((uint)nx < (uint)width && (uint)ny < (uint)height) {
                        int newi = ny * width + nx;
                        if (grid[newi] == 0) continue;
                        if (--grid[newi] < 5) {
                            q.Push(newi);
                        } 
                    }
                }
            }
        }

        return deaths;
    }

    public int Part2Faster4() {
        int height = _input.Length;
        int width = _input[0].Length;
        int size = (height + 2) * (width + 2);
        int deaths = 0;
        
        byte[] grid = new byte[size];
        var q = new Stack<int>();

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                if (_input[y][x] == '@') {
                    byte count = 0;
                    for (int dx = -1; dx <= 1; dx++) {
                        for (int dy = -1; dy <= 1; dy++) {
                            int nx = x + dx;
                            int ny = y + dy;
                            
                            if ((uint)nx < (uint)width && (uint)ny < (uint)height) {
                                if (_input[ny][nx] == '@') {
                                    count++;
                                }
                            }
                        }   
                    }
                    if (count < 5) {
                        grid[(y+1)*(width+2)+(x+1)] = 0;
                        deaths++;
                        q.Push((y+1)*(width+2)+(x+1));
                    } else {
                        grid[(y+1)*(width+2)+(x+1)] = count;
                    }             
                }
            }
        }

        int[] dirs = {-width-3, -width-2, -width-1, -1, 1, width+1, width+2, width+3};

        while (q.Count > 0) {
            int posToKill = q.Pop();

            for (int i = 0; i < 8; i++) {
                int nextPos = posToKill + dirs[i];
                if (grid[nextPos] == 0) continue;
                if (--grid[nextPos] < 5) {
                    grid[nextPos] = 0;
                    deaths++;
                    q.Push(nextPos);
                }
            }
        }

        return deaths;
    }

    public int Part2Faster5() {
        int height = _input.Length;
        int width = _input[0].Length;
        int size = (height + 2) * (width + 2);
        int deaths = 0;
        
        Span<byte> grid = stackalloc byte[size];
        Span<int> q = stackalloc int[size];
        int head = 0;
        int tail = 0;

        Span<int> dirs = stackalloc[] {-width-3, -width-2, -width-1, -1, 1, width+1, width+2, width+3};

        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                if (_input[y][x] == '@') {
                    byte count = 0;
                    for (int dx = -1; dx <= 1; dx++) {
                        for (int dy = -1; dy <= 1; dy++) {
                            int nx = x + dx;
                            int ny = y + dy;
                            if (nx >= 0 && nx < width && ny >= 0 && ny < height && _input[ny][nx] == '@') {
                                count++;
                            }
                        }   
                    }
                    if (count < 5) {
                        grid[(y+1)*(width+2)+(x+1)] = 0;
                        deaths++;
                        q[tail++] = (y+1)*(width+2)+(x+1);
                    } else {
                        grid[(y+1)*(width+2)+(x+1)] = count;
                    }             
                }
            }
        }

        while (head < tail) {
            int posToKill = q[head++];
            for (int i = 0; i < 8; i++) {
                int nextPos = posToKill + dirs[i];
                if (grid[nextPos] == 0) continue;
                if (--grid[nextPos] < 5) {
                    grid[nextPos] = 0;
                    deaths++;
                    q[tail++] = nextPos;  
                }
            }
        }

        return deaths;
    }
}
