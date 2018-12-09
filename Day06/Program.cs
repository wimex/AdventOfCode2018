using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;

namespace Day06
{
    class Program
    {
        private class Point
        {
            public char L { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public bool I { get; set; }
            public int S { get; set; }
        }

        static void Main(string[] args)
        {
            const string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            string[] lines = File.ReadAllLines("input.txt");
            int size = 500;
            int[,] grid = new int[size, size];
            int[,] distances1 = new int[size, size];
            int[,] distances2 = new int[size, size];
            List<Point> points = new List<Point>();

            int position = -1;
            foreach (string line in lines)
            {
                string[] pieces = line.Split(", ", StringSplitOptions.RemoveEmptyEntries);
                int x = int.Parse(pieces[0]);
                int y = int.Parse(pieces[1]);

                points.Add(new Point {X = x, Y = y, I = false, L = alphabet[Math.Abs(position)]});

                grid[x, y] = position;
                distances1[x, y] = -1;
                position--;
            }

            foreach (Point point in points)
            {
                int x0 = 0, y0 = 0;
                int letter = grid[point.X, point.Y];

                while (y0 < size)
                {
                    while (x0 < size)
                    {
                        int distance = Math.Abs(point.X - x0) + Math.Abs(point.Y - y0);
                        if (distance > 0 && (distances1[x0, y0] == 0 || distance < distances1[x0, y0]))
                        {
                            if (distances1[x0, y0] < 0)
                                throw new Exception();

                            grid[x0, y0] = letter;
                            distances1[x0, y0] = distance;
                            point.I = point.I || x0 == 0 || y0 == 0 || x0 == size - 1 || y0 == size - 1;
                        }

                        x0++;
                    }

                    x0 = 0;
                    y0++;
                }
            }

            StringBuilder output = new StringBuilder();
            int i = 0, j = 0;

            while (j < size)
            {
                while (i < size)
                {
                    if (grid[i, j] == 0)
                        output.Append(".");
                    else if (grid[i, j] > 0)
                        output.Append(grid[i, j]);
                    else if (grid[i, j] < 0)
                        output.Append(alphabet[Math.Abs(grid[i, j])]);

                    i++;
                }

                output.Append(Environment.NewLine);
                i = 0;
                j++;
            }

            string map = output.ToString();
            foreach (Point point in points)
            {
                point.S = map.Count(c => c == point.L);
            }

            foreach (Point point in points.OrderByDescending(p=>p.S))
            {
                Console.WriteLine($"Label: {point.L}, Size: {point.S}, Infinite: {point.I}");
            }

            int x1 = 0, y1 = 0;
            int count = 0;
            while (y1 < size)
            {
                while (x1 < size)
                {
                    distances2[x1, y1] = points.Select(p => Math.Abs(p.X - x1) + Math.Abs(p.Y - y1)).Sum();
                    if (distances2[x1, y1] < 10000)
                        count++;

                    x1++;
                }

                x1 = 0;
                y1++;
            }

            Console.WriteLine($"Size of region: {count}");

            File.WriteAllText("grid.txt", map);
            Console.ReadKey();
        }
    }
}