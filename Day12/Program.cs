using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Day12
{
    class Program
    {
        static void Main(string[] args)
        {
            long generation = 0;
            List<string> padding = new List<string> { ".", ".", ".", ".", "." };
            List<string> plants = "#......##...#.#.###.#.##..##.#.....##....#.#.##.##.#..#.##........####.###.###.##..#....#...###.##".Select(s => s.ToString()).ToList();
            Dictionary<string, string> statechanges = new Dictionary<string, string>
            {
                {".#.##", "."},
                {".####", "."},
                {"#..#.", "."},
                {"##.##", "#"},
                {"..##.", "#"},
                {"##...", "#"},
                {"..#..", "#"},
                {"#.##.", "."},
                {"##.#.", "."},
                {".###.", "#"},
                {".#.#.", "#"},
                {"#..##", "#"},
                {".##.#", "#"},
                {"#.###", "#"},
                {".##..", "#"},
                {"###.#", "."},
                {"#.#.#", "#"},
                {"#....", "."},
                {"#...#", "."},
                {".#...", "#"},
                {"##..#", "."},
                {"....#", "."},
                {".....", "."},
                {".#..#", "#"},
                {"#####", "."},
                {"#.#..", "."},
                {"..#.#", "#"},
                {"...##", "."},
                {"...#.", "#"},
                {"..###", "."},
                {"####.", "#"},
                {"###..", "#"},
            };
            
            while (generation < 20)
            {
                List<string> next = new List<string>(plants);
                Parallel.For(2, plants.Count - 2, (i) =>
                {
                    string pattern = plants[i - 2] + plants[i - 1] + plants[i] + plants[i + 1] + plants[i + 2];
                    string statechange = statechanges.TryGetValue(pattern, out string nx) ? nx : ".";

                    next[i] = statechange;
                });

                int padLeft = next.IndexOf("#");
                int padRight = next.LastIndexOf("#");

                plants = padding.Concat(next.Skip(padLeft - 1).Take(padRight - padLeft + 1).ToList()).Concat(padding).ToList();
                generation++;
                
                Console.WriteLine($"{string.Join("", plants)}");
                Console.WriteLine($"@{generation}, size: {plants.Count}, plants: {plants.Count(p => p == "#")}");
            }

            long result = 0;
            for (int i = 0; i < plants.Count; i++)
            {
              result += plants[i] == "#" ? (i - padding.Count ) : 0;
            }

            Console.WriteLine($"Result: {result}");
            Console.ReadKey();
        }
    }
}
