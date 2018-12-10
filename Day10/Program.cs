using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Day10
{
    class Program
    {
        private class Star
        {
            public int PositionX { get; set; }
            public int PositionY { get; set; }

            public int VelocityX { get; set; }
            public int VelocityY { get; set; }
        }

        static void Main(string[] args)
        {
            List<Star> stars = new List<Star>();
            string[] lines = File.ReadAllLines("input.txt");
            Regex regex = new Regex(@"position=<([0-9\- ]+), ([0-9\- ]+)> velocity=<([0-9\- ]+), ([0-9\- ]+)>");

            foreach (string line in lines)
            {
                Match match = regex.Match(line);
                int px = int.Parse(match.Groups[1].Value);
                int py = int.Parse(match.Groups[2].Value);
                int vx = int.Parse(match.Groups[3].Value);
                int vy = int.Parse(match.Groups[4].Value);

                stars.Add(new Star()
                {
                    PositionX = px,
                    PositionY = py,
                    VelocityX = vx,
                    VelocityY = vy
                });
            }
            
            int pwidth = int.MaxValue, pheight = int.MaxValue;
            int seconds = 0;
            int found = 0;

            while (found < 32)
            {
                foreach (Star star in stars)
                {
                    star.PositionX += star.VelocityX;
                    star.PositionY += star.VelocityY;
                }

                int xmin = stars.Min(s => s.PositionX);
                int xmax = stars.Max(s => s.PositionX);
                int ymin = stars.Min(s => s.PositionY);
                int ymax = stars.Max(s => s.PositionY);

                int width = Math.Abs(xmin) + Math.Abs(xmax) + 1;
                int height = Math.Abs(ymin) + Math.Abs(ymax) + 1;

                if (pwidth <= width && pheight <= height)
                {

                    Console.WriteLine($"Found an interesting map at {seconds} ({width}x{height})");
                    StringBuilder map = new StringBuilder();
                    for (int j = ymin; j <= ymax; j++)
                    {
                        for (int i = xmin; i <= xmax; i++)
                        {
                            bool star = stars.Any(s => s.PositionX == i && s.PositionY == j);
                            map.Append(star ? "*" : " ");
                        }

                        map.AppendLine();
                    }

                    File.WriteAllText($"map-{seconds.ToString().PadLeft(4, '0')}.txt", map.ToString());
                    found++;
                }
                else
                {
                    if (found > 0)
                        return;
                }

                pwidth = width;
                pheight = height;
                seconds++;
            }

            Console.ReadKey();
        }
    }
}
