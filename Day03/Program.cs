using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Day03
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("input.txt");
            List<Tuple<string, int, int, int, int>> claims = new List<Tuple<string, int, int, int, int>>();
            int[,] fabric = new int[1000, 1000];

            foreach (string line in lines)
            {
                string[] pieces = line.Split(' ');
                string[] coords = pieces[2].Replace(":", "").Split(',');
                string[] sizes = pieces[3].Split('x');

                int x = int.Parse(coords[0]);
                int y = int.Parse(coords[1]);
                int w = int.Parse(sizes[0]);
                int h = int.Parse(sizes[1]);

                int a = x;
                int b = y;

                while (b < y + h)
                {
                    while (a < x + w)
                    {
                        fabric[a,b]++;
                        a++;
                    }

                    a = x;
                    b++;
                }

                claims.Add(Tuple.Create(pieces[0], x, y, w, h));
            }

            int summary = 0;
            using (FileStream result = new FileStream("result.txt", FileMode.Create, FileAccess.Write))
            {
                for (int i = 0; i < 1000; i++)
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        result.Write(Encoding.UTF8.GetBytes(Convert.ToString(fabric[i, j])));
                        summary += fabric[i, j] > 1 ? 1 : 0;
                    }

                    result.Write(Encoding.UTF8.GetBytes(Environment.NewLine));
                }
            }

            Console.WriteLine($"Multiple claims: {summary}");

            foreach (Tuple<string, int, int, int, int> claim in claims)
            {
                int a = claim.Item2;
                int b = claim.Item3;
                bool invalid = false;

                while (!invalid && b < claim.Item3 + claim.Item5)
                {
                    while (!invalid && a < claim.Item2 + claim.Item4)
                    {
                        invalid = fabric[a, b] > 1;
                        a++;
                    }

                    a = claim.Item2;
                    b++;
                }

                if (invalid)
                    continue;

                Console.WriteLine($"Found single claim: {claim.Item1}");
                break;
            }

            Console.ReadKey();
        }
    }
}
