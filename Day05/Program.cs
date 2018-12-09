using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day05
{
    class Program
    {
        static string FullyReact(List<char> polymer)
        {
            int position = 0;

            while (true)
            {
                if (polymer.Count == position + 1)
                    break;

                char current = polymer[position];
                char next = polymer[position + 1];
                bool cu = char.IsUpper(current);
                bool nu = char.IsUpper(next);

                if (cu == nu)
                {
                    position++;
                    continue;
                }

                if (!string.Equals(current.ToString(), next.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    position++;
                    continue;
                }

                polymer.RemoveAt(position + 1);
                polymer.RemoveAt(position);

                position = Math.Max(position - 1, 0);
            }

            return string.Join("", polymer);
        }

        static void Main(string[] args)
        {
            List<char> molecule = File.ReadAllText("input.txt").ToList();
            List<char> unique = molecule.GroupBy(c => c.ToString().ToUpperInvariant()).Select(c => c.Key.ToCharArray()[0]).ToList();
            string question1 = FullyReact(molecule);

            string shortest = null;
            foreach (char current in unique)
            {
                List<char> test = molecule.Where(c => !string.Equals(c.ToString(), current.ToString(), StringComparison.InvariantCultureIgnoreCase)).ToList();
                string result = FullyReact(test);

                if (shortest == null || shortest.Length > result.Length)
                    shortest = result;
            }

            Console.WriteLine(question1);
            Console.WriteLine($"Number of units remaining: {question1.Length}");
            Console.WriteLine("========");
            Console.WriteLine(shortest);
            Console.WriteLine($"Shortest item: {shortest.Length}");
            Console.ReadKey();
        }
    }
}
