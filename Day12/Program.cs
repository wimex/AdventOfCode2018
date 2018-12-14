using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Day12
{
    class Program
    {
        static void Main(string[] args)
        {
            long generation = 0;
            List<string> padding = new List<string> {".", ".", ".", ".", "."};
            /*List<string> plants = "#..#.#..##......###...###".Select(s => s.ToString()).ToList();
            Dictionary<string, string> statechanges = new Dictionary<string, string>
            {
                {"...##", "#"},
                {"..#..", "#"},
                {".#...", "#"},
                {".#.#.", "#"},
                {".#.##", "#"},
                {".##..", "#"},
                {".####", "#"},
                {"#.#.#", "#"},
                {"#.###", "#"},
                {"##.#.", "#"},
                {"##.##", "#"},
                {"###..", "#"},
                {"###.#", "#"},
                {"####.", "#"}
            };*/
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

            long iterations = 50000000000;
            long index = 0;
            long pindex = 0;
            long summary = 0;
            string history = null;
            while (generation < iterations)
            {
                List<string> padded = padding.Concat(plants).Concat(padding).ToList();
                List<string> next = new List<string>(padded);
                Parallel.For(2, padded.Count - 2, (i) =>
                {
                    string pattern = padded[i - 2] + padded[i - 1] + padded[i] + padded[i + 1] + padded[i + 2];
                    string statechange = statechanges.TryGetValue(pattern, out string nx) ? nx : ".";

                    next[i] = statechange;
                });

                int padLeft = next.IndexOf("#");
                int padRight = next.LastIndexOf("#");

                plants = next.Skip(padLeft).Take(padRight - padLeft + 1).ToList();
                pindex = index;
                index += padding.Count - padLeft;
                summary = GetPlantSummary(index, plants);
                generation++;

                bool stabilized = history == string.Join("", plants);
                if (stabilized)
                {
                    index = index + (iterations - generation) * (index - pindex);
                    summary = GetPlantSummary(index, plants);
                    break;
                }

                Console.WriteLine($"{string.Join("", plants)}");
                Console.WriteLine($"@{generation}, size: {plants.Count}, plants: {plants.Count(p => p == "#")}, index: {index}, summary: {summary}");
                history = string.Join("", plants);
            }
            
            Console.WriteLine($"Result: {summary}");
            Console.ReadKey();
        }

        static long GetPlantSummary(long index, List<string> plants)
        {
            long result = 0;
            for (int i = 0; i < plants.Count; i++)
            {
                result += plants[i] == "#" ? (i - index) : 0;
            }

            return result;
        }
    }
}
