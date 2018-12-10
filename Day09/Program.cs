using System;
using System.Linq;

namespace Day09
{
    class Program
    {
        private class Marble
        {
            public int Value { get; set; } = 0;
            
            public Marble Left { get; set; } = null;
            public Marble Right { get; set; } = null;
        }

        static long Play(int players, int points)
        {
            long[] scores = new long[players];
            int target = points;

            int position = 1;
            Marble head = new Marble();
            Marble current = head;
            head.Left = head;
            head.Right = head;

            while (position <= target)
            {
                if (position % 23 == 0)
                {
                    Marble removed = current.Left.Left.Left.Left.Left.Left.Left;
                    removed.Left.Right = removed.Right;
                    removed.Right.Left = removed.Left;

                    scores[position % scores.Length] += removed.Value + position;
                    current = removed.Right;
                }
                else
                {
                    Marble left = current.Right;
                    Marble right = current.Right.Right;
                    Marble next = new Marble
                    {
                        Value = position,
                        Left = left,
                        Right = right
                    };

                    left.Right = next;
                    right.Left = next;
                    current = next;
                }

                /*Console.Write($"[{position % players.Length}] ");
                Marble print = head;
                for (int i = 0; i <= position; i++)
                {
                    Console.Write(print == current ? $"({print.Value})" : $"{print.Value}");
                    Console.Write(" ");

                    print = print.Right;
                }

                Console.WriteLine();*/
                position++;
            }

            return scores.Max();
        }

        static void Main(string[] args)
        {
            long game1 = Play(400, 71864);
            long game2 = Play(400, 7186400);

            Console.WriteLine($"Winner: {game1}, {game2}");
            Console.ReadKey();
        }
    }
}
