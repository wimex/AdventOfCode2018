using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Day15
{
    class Program
    {
        private class Target
        {
            public int X { get; set; }
            public int Y { get; set; }

            public bool Reachable { get; set; }
            public int Distance { get; set; }

            public Target(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        static void MarkRoute(string[,] route, int width, int height, int x, int y, int distance)
        {
            bool reachable = route[x, y] == ".";
            if (!int.TryParse(route[x, y], out int previous))
                previous = int.MaxValue;

            if (previous > distance)
                route[x, y] = distance.ToString();

            if (!reachable)
                return;

            if (x > 0)
                MarkRoute(route, width, height, x - 1, y, distance + 1);
            if (y > 0)
                MarkRoute(route, width, height, x, y - 1, distance + 1);
            if (x < width - 1)
                MarkRoute(route, width, height, x + 1, y, distance + 1);
            if (y < height - 1)
                MarkRoute(route, width, height, x, y + 1, distance + 1);
        }

        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("input.txt");

            int width = lines[0].Length;
            int height = lines.Length;
            string[,] map = new string[width, height];

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    map[i,j] = lines[j][i].ToString();
                }
            }

            while (true)
            {
                for (int j = 0; j < height; j++)
                {
                    for (int i = 0; i < width; i++)
                    {
                        if (map[i,j] != "G" && map[i, j] != "E" && map[i, j] != "C" && map[i, j] != "I")
                            continue;

                        string tile = map[i, j];
                        List<Target> targets = new List<Target>();
                        for (int y = 0; y < height; y++)
                        {
                            for (int x = 0; x < width; x++)
                            {
                                bool enemy = tile == "G" && map[x, y] == "E" || tile == "E" && map[x, y] == "G";
                                if (!enemy)
                                    continue;

                                if (x > 0 && map[x - 1, y] == ".")
                                    targets.Add(new Target(x - 1, y));
                                if (y > 0 && map[x, y - 1] == ".")
                                    targets.Add(new Target(x, y - 1));
                                if (x < width - 1 && map[x + 1, y] == ".")
                                    targets.Add(new Target(x + 1, y));
                                if (y < height - 1 && map[x, y + 1] == ".")
                                    targets.Add(new Target(x, y + 1));
                            }
                        }

                        string[,] shadow = (string[,])map.Clone();
                        shadow[i, j] = ".";

                        MarkRoute(shadow, width, height, i, j, 0);
                        foreach (Target target in targets)
                        {
                            if (!int.TryParse(shadow[target.X, target.Y], out int distance))
                                continue;

                            target.Reachable = true;
                            target.Distance = distance;
                        }

                        int closest = targets.Where(d => d.Distance > 0).Min(d => d.Distance);
                        List<Target> moves = targets.Where(t => t.Distance == closest).OrderBy(t => t.Y).ThenBy(t => t.X).ToList();

                       // Console.WriteLine($"{map[i, j]} is taking a move: {i.ToString().PadLeft(2, ' ')},{j.ToString().PadLeft(2, ' ')} => {move.X.ToString().PadLeft(2, ' ')},{move.Y.ToString().PadLeft(2, ' ')}");
                        
                        for (int y = 0; y < height; y++)
                        {
                            for (int x = 0; x < width; x++)
                            {
                                Console.Write($"{shadow[x, y]}");
                            }

                            Console.WriteLine();
                        }
                    }
                }
            }
        }
    }
}
