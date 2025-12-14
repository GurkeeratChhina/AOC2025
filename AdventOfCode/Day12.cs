namespace AdventOfCode;

public class Day12 : BaseDay
{
    private readonly string[] _input;
    int number_polyominos;
    int polyomino_widths;
    int number_rectangles;
    int[] polyomino_areas;
    int[] rectanglexs;
    int[] rectangleys;
    int[,] polyomino_counts;

    public Day12()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new($"{Part1()}");

    public override ValueTask<string> Solve_2() => new($"{Part2()}");

    void parse() {
        number_polyominos = 6;
        polyomino_widths = 3;
        polyomino_areas = new int[number_polyominos];

        number_rectangles = _input.Length - number_polyominos*(polyomino_widths+2);
        rectanglexs = new int[number_rectangles];
        rectangleys = new int[number_rectangles];
        polyomino_counts = new int[number_polyominos,number_rectangles];

        for (int i = 0; i < number_polyominos; i++) {
            int size = 0;
            for (int j = i*(polyomino_widths+2)+1; j< (i+1)*(polyomino_widths+2)-1; j++) {
                foreach (var character in _input[j]) {
                    if (character == '#') {
                        size++;
                    }
                }
            }
            polyomino_areas[i] = size;
        }

        //parse rectangles
        for (int i = 0 ; i < _input.Length - number_polyominos*(polyomino_widths+2); i++) {
            int row = i+ number_polyominos*(polyomino_widths+2);
            string[] line = _input[row].Split(new char[]{' ', ':', 'x'});
            rectanglexs[i] = int.Parse(line[0]);
            rectangleys[i] = int.Parse(line[1]);
            for (int j = 0; j <number_polyominos; j++) {
                polyomino_counts[j, i] = int.Parse(line[j+3]);
            }
        }
    }

    public int Part1(){
        
        int total = 0;
        for (int i = 0; i < number_rectangles; i++) {
            int rectangle_area = rectanglexs[i] * rectangleys[i];
            int total_polynomio_area = 0;
            for (int j = 0; j < number_polyominos; j++) {
                total_polynomio_area += polyomino_areas[j]*polyomino_counts[j,i];
            }
            if (rectangle_area < total_polynomio_area) continue; // check upper bound

            int reduced_area = (rectanglexs[i]/polyomino_widths)*(rectangleys[i]/polyomino_widths);
            int total_polynominos = 0;
            for (int j = 0; j < number_polyominos; j++) {
                total_polynominos += polyomino_counts[j,i];
            }
            if (reduced_area >= total_polynominos) { // check lower bound
                total++;
                continue;
            }

            Console.WriteLine("Error - unimplemented case. Please double check your bounds.");
        }
        return total;
    }

    public int Part2(){
        return 0;
    }
}
