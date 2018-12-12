using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Day11
{
    class Program
    {
        static void Main(string[] args)
        {
            const int serialNumber = 8772;
            const int size = 300;

            int[,] grid = new int[size, size];

            StringBuilder output = new StringBuilder();
            for (int j = 0; j < size; j++)
            {
                for (int i = 0; i < size; i++)
                {
                    int step1 = i + 1 + 10;
                    int step2 = step1 * (j + 1);
                    int step3 = step2 + serialNumber;
                    int step4 = step3 * step1;
                    int step5 = step4 > 100 ? (step4 / 100) % 10 : 0;
                    int power = step5 - 5;

                    output.Append($"{power.ToString().PadLeft(3, ' ')} ");
                    grid[i, j] = power;
                }

                output.AppendLine();
            }

            File.WriteAllText("grid.txt", output.ToString());

            object lck = new object();
            int maxsquare = 0;
            int maxlevel = -1;
            int maxpointx = 0;
            int maxpointy = 0;

            Parallel.For(1, 301, (squaresize) =>
            {
                for (int j = 0; j < size; j++)
                {
                    for (int i = 0; i < size; i++)
                    {
                        if (i + squaresize > size || j + squaresize > size)
                            continue;

                        int sum = 0;
                        for (int l = j; l < j + squaresize; l++)
                        {
                            for (int k = i; k < i + squaresize; k++)
                            {
                                sum += grid[k, l];
                            }
                        }

                        if (sum > maxlevel)
                        {
                            lock(lck)
                            {
                                Console.WriteLine($"TT: {sum} @ {i + 1},{j + 1} ({squaresize})");

                                maxlevel = sum;
                                maxpointx = i;
                                maxpointy = j;
                                maxsquare = squaresize;
                            }
                        }
                    }
                }
            });

            Console.WriteLine($"FF: {maxlevel} @ {maxpointx + 1},{maxpointy + 1} ({maxsquare})");
            Console.ReadKey();
        }
    }
}
