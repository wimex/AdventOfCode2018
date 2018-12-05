using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day2
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("input.txt");
            int dbl = 0;
            int tpl = 0;

            foreach (string line in lines)
            {
                char[] letters = line.GroupBy(x => x).Select(x => x.Key).ToArray();
                bool isDouble = false;
                bool isTriple = false;

                foreach (char letter in letters)
                {
                    int count = line.Count(chr => chr == letter);
                    if (count == 2)
                        isDouble = true;
                    if (count == 3)
                        isTriple = true;
                }

                if (isDouble)
                    dbl++;
                if (isTriple)
                    tpl++;
            }

            Console.WriteLine($"Count: {dbl}+{tpl}={dbl * tpl}");

            foreach (string line1 in lines)
            {
                foreach (string line2 in lines.Where(l => l.Count() == line1.Count()))
                {
                    int difference = line1.Select((chr, idx) => chr == line2[idx] ? 0 : 1).Sum();
                    if (difference == 1)
                    {
                        Console.WriteLine($"IDs: {line1},{line2}");
                        Console.ReadKey();
                        return;
                    }
                }
            }
        }
    }
}
