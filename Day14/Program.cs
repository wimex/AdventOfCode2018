using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.Versioning;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Day14
{
    class Program
    {
        static void Main(string[] args)
        {
            const long target = 82540; //or replace with the question for part 1
            const int print = 10;

            string needle = target.ToString(CultureInfo.InvariantCulture);
            int tlength = needle.Length;
            int limit = int.MaxValue - tlength - 10;

            List<int> elves = new List<int>
            {
                0,
                1
            };
            List<int> recipes = new List<int>
            {
                3,
                7
            };

            bool part1 = false;
            bool part2 = false;
            string sequence = null;
            long step = 0;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (!part2 || !part1)
            {
                /*Console.Write($"{frame.ToString().PadLeft(3, ' ')} ");
                foreach (Recipe recipe in recipes)
                {
                    bool around = recipe.Elves.Any();
                    Console.Write($"{(around ? "[" : " ")}{recipe.Score}{(around ? "]" : " ")} ");
                }

                Console.WriteLine();*/

                if (!part1 && recipes.Count >= limit + print + 1)
                {
                    Console.WriteLine($"Found a wise elf: {string.Join("", recipes.Skip(limit).Take(print))}");
                    part1 = true;
                }

                int sum = recipes[elves[0]] + recipes[elves[1]];
                if (sum > 99)
                    throw new Exception();

                if (sum >= 10)
                {
                    int score = (int) (sum / 10);
                    recipes.Add(score);
                    sequence += score;
                }

                if (sum >= 0)
                {
                    int score = (int) (sum % 10);
                    recipes.Add(score);
                    sequence += score;
                }

                int next1 = 1 + recipes[elves[0]];
                elves[0] += next1;
                elves[0] = (int)(elves[0] % recipes.Count);

                sequence = sequence != null && sequence.Length > tlength ? sequence.Substring(sequence.Length - tlength) : sequence;
                if (!part2 && sequence == needle)
                {
                    Console.WriteLine($"Found the required sequence at {recipes.Count - tlength}");
                    limit = recipes.Count - tlength;
                    part2 = true;
                }

                int next2 = 1 + recipes[elves[1]];
                elves[1] += next2;
                elves[1] = (int)(elves[1] % recipes.Count);

                sequence = sequence != null && sequence.Length > tlength ? sequence.Substring(sequence.Length - tlength) : sequence;
                if (!part2 && sequence == needle)
                {
                    Console.WriteLine($"Found the required sequence at {recipes.Count - tlength}");
                    limit = recipes.Count - tlength;
                    part2 = true;
                }
                step++;

                if (step % 10000000 == 0)
                {
                    Console.WriteLine($"Checking recipies at {step} - {sequence}");
                    stopwatch.Stop();

                    Console.WriteLine($"Last iteration took {stopwatch.ElapsedMilliseconds} ms");
                    stopwatch.Restart();
                }
            }

            Console.ReadKey();
        }
    }
}
