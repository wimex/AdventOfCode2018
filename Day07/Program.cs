using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day07
{
    class Program
    {
        private class Step
        {
            public string Name { get; set; }
            public List<Step> Dependencies { get; set; } = new List<Step>();
        }

        private class Worker
        {
            public string Name { get; set; } = ".";
            public int Time { get; set; } = 0;
        }

        static void Main(string[] args)
        {
            List<Step> steps1 = new List<Step>();
            List<Step> steps2 = new List<Step>();
            List<string> letters = new List<string>();
            string[] lines = File.ReadAllLines("input.txt");
            Regex regex = new Regex("Step ([A-Z]) must be finished before step ([A-Z]) can begin.");

            foreach (string line in lines)
            {
                Match match = regex.Match(line);
                string pointA = match.Groups[1].Value;
                string pointB = match.Groups[2].Value;

                if (!letters.Contains(pointA))
                    letters.Add(pointA);
                if (!letters.Contains(pointB))
                    letters.Add(pointB);

                if (steps1.All(s => s.Name != pointA))
                    steps1.Add(new Step { Name = pointA });
                if (steps1.All(s => s.Name != pointB))
                    steps1.Add(new Step { Name = pointB });

                if (steps2.All(s => s.Name != pointA))
                    steps2.Add(new Step { Name = pointA });
                if (steps2.All(s => s.Name != pointB))
                    steps2.Add(new Step { Name = pointB });

                steps1.First(s => s.Name == pointB).Dependencies.Add(steps1.First(s => s.Name == pointA));
                steps2.First(s => s.Name == pointB).Dependencies.Add(steps2.First(s => s.Name == pointA));
            }

            letters = letters.OrderBy(l => l).ToList();

            while (steps1.Any())
            {
                Step current = steps1.OrderBy(s => s.Name).First(s => !s.Dependencies.Any());
                steps1.Remove(current);

                foreach (Step clean in steps1.Where(s => s.Dependencies.Contains(current)))
                {
                    clean.Dependencies.Remove(current);
                }

                Console.Write($"{current.Name}");
            }

            int seconds = 0;
            List<Worker> workers = new List<Worker> { new Worker(), new Worker(), new Worker(), new Worker(), new Worker() };

            Console.WriteLine();
            Console.WriteLine("-----     1     2     3     4     5");
            List<string> order = new List<string>();
            while (steps2.Any() || workers.Any(w => w.Time != 0))
            {
                List<Step> available = steps2.OrderBy(s => s.Name).Where(s => !s.Dependencies.Any()).ToList();
                string avprint = string.Join("", available.Select(a=>a.Name));
                Console.Write(seconds.ToString().PadLeft(4, ' ') + " ");

                for (int i = 0; i < workers.Count; i++)
                {
                    if (workers[i].Time == 0 && available.Any())
                    {
                        Step current = available.First();
                        workers[i].Time = 60 + letters.IndexOf(current.Name) + 1;
                        workers[i].Name = current.Name;
                        steps2.Remove(current);
                        available.Remove(current);
                    }

                    if (workers[i].Time != 0)
                        workers[i].Time--;

                    if (workers[i].Time == 0 && workers[i].Name!="." && !string.IsNullOrWhiteSpace(workers[i].Name) && !order.Contains(workers[i].Name))
                    {
                        foreach (Step clean in steps2.Where(s => s.Dependencies.Any(d => d.Name == workers[i].Name)))
                        {
                            clean.Dependencies.RemoveAll(d => d.Name == workers[i].Name);
                        }

                        order.Add(workers[i].Name);
                        workers[i].Name = ".";
                    }

                    Console.Write(workers[i].Time.ToString().PadLeft(5, ' ') + workers[i].Name);
                }

                Console.WriteLine(" " + string.Join("", order) + $"[{avprint}]");
                seconds++;
            }

            Console.WriteLine(string.Join("", order));
            Console.ReadKey();
        }
    }
}
