using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Day08
{
    class Program
    {
        private class Node
        {
            public List<int> Metadata { get; set; } = new List<int>();
            public List<Node> Children { get; set; } = new List<Node>();

            public int Summary
            {
                get
                {
                    if (!this.Children.Any())
                        return this.Metadata.Sum(m => m);

                    int summary = 0;
                    foreach (int metadata in this.Metadata)
                    {
                        if (metadata == 0)
                            continue;
                        if (metadata > this.Children.Count)
                            continue;

                        summary += this.Children[metadata - 1].Summary;
                    }

                    return summary;
                }
            }
        }

        static int GetNodeSum(Queue<int> input)
        {
            if (input.Count < 2)
                return 0;

            int summary = 0;
            int children = input.Dequeue();
            int metadata = input.Dequeue();

            while (children > 0)
            {
                summary += GetNodeSum(input);
                --children;
            }

            while (metadata > 0)
            {
                summary += input.Dequeue();
                --metadata;
            }

            return summary;
        }

        static Node GetNode(Queue<int> input)
        {
            if (input.Count < 2)
                return new Node();

            Node result = new Node();
            int children = input.Dequeue();
            int metadata = input.Dequeue();

            while (children > 0)
            {
                result.Children.Add(GetNode(input));
                --children;
            }

            while (metadata > 0)
            {
                result.Metadata.Add(input.Dequeue());
                --metadata;
            }

            return result;
        }

        static void Main(string[] args)
        {
            Queue<int> input = new Queue<int>(File.ReadAllText("input.txt").Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse));
            Queue<int> input1 = new Queue<int>(input);
            Queue<int> input2 = new Queue<int>(input);

            int checksum = GetNodeSum(input1);
            Node root = GetNode(input2);

            Console.WriteLine($"Summary 1: {checksum}");
            Console.WriteLine($"Summary 2: {root.Summary}");
            Console.ReadKey();
        }
    }
}
