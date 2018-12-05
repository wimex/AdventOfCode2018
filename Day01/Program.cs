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

            while (true) {
                foreach (string line in lines)
                {
                    int number = int.Parse(line.Substring(1));
                    if (line[0] == '+')
                        current += number;
                    else
                        current -= number;

                    if (history.Contains(current))
                        goto found;
                    else
                        history.Add(current);
                }
            }

        found:
            Console.WriteLine(string.Join(',', history) + "," + current);
            Console.ReadKey();
        }
    }
}
