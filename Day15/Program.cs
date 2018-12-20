using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Day15
{
    class Program
    {
        private class Tile : ICloneable
        {
            public int X { get; set; }
            public int Y { get; set; }

            public string Entity { get; set; }
            public int Distance { get; set; } = int.MaxValue;

            public bool Targetable { get; set; }
            public bool Moveable { get; set; }

            public bool Connected => this.Distance != int.MaxValue;
            public bool Reachable => this.Entity == ".";

            public bool IsUnit => this.Entity == "G" || this.Entity == "E";
            public bool IsWall => this.Entity == "#";

            public object Clone()
            {
                return new Tile
                {
                    X = this.X,
                    Y = this.Y,
                    Entity = this.Entity
                };
            }
        }

        static void MarkRoutes(Tile[,] map, int x, int y, int distance)
        {
            Tile tile = map[x, y];
            if (tile.IsUnit)
            {
                tile.Targetable = true;
                return;
            }

            if (!tile.Reachable)
                return;

            if (distance >= tile.Distance)
                return;
            else
                tile.Distance = distance;

            int width = map.GetLength(0);
            int height = map.GetLength(1);

            if (x > 0)
                MarkRoutes(map, x - 1, y, distance + 1);
            if (y > 0)
                MarkRoutes(map, x, y - 1, distance + 1);
            if (x < width - 1)
                MarkRoutes(map, x + 1, y, distance + 1);
            if (y < height - 1)
                MarkRoutes(map, x, y + 1, distance + 1);
        }

        static Tuple<Tile, int> BacktrackRoute(Tile[,] map, int fromX, int fromY, int toX, int toY, int length)
        {
            int width = map.GetLength(0);
            int height = map.GetLength(1);
            int x = fromX;
            int y = fromY;

            if (x == toX && y == toY)
                return Tuple.Create(map[x, y], length);

            List<Tuple<Tile, int>> distances = new List<Tuple<Tile, int>>();

            if (y > 0 && map[x, y - 1].Distance < map[x, y].Distance)
                distances.Add(BacktrackRoute(map, x, y - 1, toX, toY, length + 1));

            if (x > 0 && map[x - 1, y].Distance < map[x, y].Distance)
                distances.Add(BacktrackRoute(map, x - 1, y, toX, toY, length + 1));

            if (x < width - 1 && map[x + 1, y].Distance < map[x, y].Distance)
                distances.Add(BacktrackRoute(map, x + 1, y, toX, toY, length + 1));

            if (y < height - 1 && map[x, y + 1].Distance < map[x, y].Distance)
                distances.Add(BacktrackRoute(map, x, y + 1, toX, toY, length + 1));

            return distances.Where(d => d.Item1.X != toX && d.Item1.Y != d.Item1.Y).OrderByDescending(d => d.Item2).ThenBy(d => d.Item1.Y).ThenBy(d => d.Item1.X).FirstOrDefault();
        }

        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("input.txt");

            int width = lines[0].Length;
            int height = lines.Length;
            Tile[,] map = new Tile[width, height];

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    map[i, j] = new Tile
                    {
                        X = i,
                        Y = j,
                        Entity = lines[j][i].ToString()
                    };
                }
            }

            while (true)
            {
                for (int j = 0; j < height; j++)
                {
                    for (int i = 0; i < width; i++)
                    {
                        map[i, j].Moveable = map[i,j].IsUnit;
                    }
                }

                for (int j = 0; j < height; j++)
                {
                    for (int i = 0; i < width; i++)
                    {
                        if (map[i,j].Entity != "G" && map[i, j].Entity != "E" && map[i, j].Entity != "C" && map[i, j].Entity != "I")
                            continue;
                        if (!map[i, j].Moveable)
                            continue;

                        Tile tile = map[i, j];
                        tile.Moveable = false;

                        Tile[,] shadow = new Tile[width, height];
                        for (int y = 0; y < height; y++)
                        {
                            for (int x = 0; x < width; x++)
                            {
                                shadow[x, y] = (Tile) map[x, y].Clone();
                            }
                        }

                        shadow[i, j].Entity = ".";

                        MarkRoutes(shadow, i, j, 0);

                        List<Tile> targets = shadow.Cast<Tile>().Where(t => t.Targetable).Where(t => t.Entity != tile.Entity).OrderBy(t => t.Y).ThenBy(t => t.X).ToList();
                        if (!targets.Any())
                            continue;

                        Tile target = targets.First();
                        Tuple<Tile, int> next = BacktrackRoute(shadow, target.X, target.Y, i, j, 0);
                        //for (int y = 0; y < height; y++)
                        //{
                        //    for (int x = 0; x < width; x++)
                        //    {
                        //        if (shadow[x, y].Selected)
                        //            Console.Write($"X");
                        //        else
                        //            Console.Write($"{shadow[x, y].Entity}");
                        //    }

                        //    Console.WriteLine();
                        //}

                        //int nextx = -1, nexty = -1;
                        //if (j > 0 && shadow[i, j - 1].Selected && shadow[i, j - 1].Distance == 1)
                        //{
                        //    nextx = i;
                        //    nexty = j - 1;
                        //}
                        //else if (i > 0 && shadow[i - 1, j].Selected && shadow[i - 1, j].Distance == 1)
                        //{
                        //    nextx = i - 1;
                        //    nexty = j;
                        //}
                        //else if (i < width - 1 && shadow[i + 1, j].Selected && shadow[i + 1, j].Distance == 1)
                        //{
                        //    nextx = i + 1;
                        //    nexty = j;
                        //}
                        //else if (j < height - 1 && shadow[i, j + 1].Selected && shadow[i, j + 1].Distance == 1)
                        //{
                        //    nextx = i;
                        //    nexty = j + 1;
                        //}

                        //if (nextx < 0 || nexty < 0)
                        //    throw new Exception();

                        Console.WriteLine($"{map[i, j].Entity} is taking a move: {i.ToString().PadLeft(2, ' ')},{j.ToString().PadLeft(2, ' ')} => {next.Item1.X.ToString().PadLeft(2, ' ')},{next.Item1.Y.ToString().PadLeft(2, ' ')}");
                        map[next.Item1.X, next.Item1.Y].Entity = map[i, j].Entity;
                        map[i, j].Entity = ".";

                        for (int y = 0; y < height; y++)
                        {
                            for (int x = 0; x < width; x++)
                            {
                                if (map[x, y].Connected)
                                    Console.Write($"{map[x, y].Distance}");
                                else
                                    Console.Write($"{map[x, y].Entity}");
                            }

                            Console.WriteLine();
                        }


                        /*List<Target> targets = new List<Target>();
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

                        Waypoint wp = FindEnemies(shadow, i, j, 0);*/

                        /*
                        FindEnemies(shadow, width, height, i, j, 0);
                        foreach (Target target in targets)
                        {
                            if (!int.TryParse(shadow[target.X, target.Y], out int distance))
                                continue;

                            target.Reachable = true;
                            target.Distance = distance;
                        }

                        int closest = targets.Where(d => d.Distance > 0).Min(d => d.Distance);
                        Target moveto = targets.Where(t => t.Distance == closest).OrderBy(t => t.Y).ThenBy(t => t.X).First();

                        */
                    }
                }
            }
        }
    }
}
