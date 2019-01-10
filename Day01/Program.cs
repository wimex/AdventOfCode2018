using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("input.txt");
            List<int> history = new List<int>();
            int current = 0;
            bool print = false;

            while (true) {
                foreach (string line in lines)
                {
                    int number = int.Parse(line);
                    current += number;

                    if (history.Contains(current))
                    {
                        Console.WriteLine($"Stable: {current}");
                        Console.ReadKey();
                        return;
                    }

                    history.Add(current);
                }

                if (!print)
                {
                    Console.WriteLine($"Summary: {current}");
                    print = true;
                }
            }
        }
    }
}
