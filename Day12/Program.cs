using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day12
{
    class Program
    {
        static void Main(string[] args)
        {
            int padLeft = 3;
            int padRight = 3;

            long generation = 0;
            List<string> plants = (new string('.', padLeft) + "#......##...#.#.###.#.##..##.#.....##....#.#.##.##.#..#.##........####.###.###.##..#....#...###.##" + new string('.', padRight)).Select(s => s.ToString()).ToList();
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
            Console.ReadKey();

            while (generation < 50000000000)
            {
                List<string> next = new List<string>(plants);
                Parallel.For(2, plants.Count - 2, (i) =>
                {
                    string pattern = plants[i - 2] + plants[i - 1] + plants[i] + plants[i + 1] + plants[i + 2];
                    string statechange = statechanges.ContainsKey(pattern) ? statechanges[pattern] : plants[i];

                    next[i] = statechange;
                });

                if (next[0] == "#" || next[1] == "#" || next[2] == "#")// || next[3] == "#" || next[4] == "#")
                {
                    next.Insert(0, ".");
                    padLeft++;
                }

                if (next[next.Count - 1] == "#" || next[next.Count - 2] == "#" || next[next.Count - 3] == "#")// || next[next.Count - 4] == "#" || next[next.Count - 5] == "#")
                {
                    next.Add(".");
                    padRight++;
                }

                plants = next;
                generation++;

                if (generation % 10000 == 0)
                //Console.WriteLine($"{string.Join("", plants)}");
                Console.WriteLine($"@{generation}, size: {plants.Count}, plants: {plants.Count(p => p == "#")}");
            }

            int result = 0;
            for (int i = 0; i < plants.Count; i++)
            {
                result += plants[i] == "#" ? (i - padLeft) : 0;
            }

            Console.WriteLine($"Result: {result}");
            Console.ReadKey();
        }
    }
}
