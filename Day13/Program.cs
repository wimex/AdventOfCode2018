using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Resolvers;

namespace Day13
{
    class Program
    {
        private class Tile
        {
            public int X { get; set; }
            public int Y { get; set; }

            public string Value { get; set; } = " ";

            public List<Cart> Carts { get; } = new List<Cart>();

            public Tile Left { get; set; }
            public Tile Right { get; set; }
            public Tile Up { get; set; }
            public Tile Down { get; set; }
        }
        
        private class Cart
        {
            public Direction Direction { get; set; }
            public Turn Turn { get; set; }
            public bool Moved { get; set; }
            public bool Crashed { get; set; }
        }

        private enum Direction
        {
            Up,
            Right,
            Down,
            Left
        }

        private enum Turn
        {
            Left,
            Straight,
            Right
        }

        static Tile GetTileByDirection(Tile source, Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return source.Up;
                case Direction.Right:
                    return source.Right;
                case Direction.Down:
                    return source.Down;
                case Direction.Left:
                    return source.Left;
                default:
                    throw new Exception();
            }
        }

        static Tuple<Direction, Tile> GetTurnByDirection(Tile source, Direction direction)
        {
            switch (source.Value)
            {
                case "/":
                    switch (direction)
                    {
                        case Direction.Up:
                            return Tuple.Create(Direction.Right, source.Right);
                        case Direction.Right:
                            return Tuple.Create(Direction.Up, source.Up);
                        case Direction.Down:
                            return Tuple.Create(Direction.Left, source.Left);
                        case Direction.Left:
                            return Tuple.Create(Direction.Down, source.Down);
                        default:
                            throw new Exception();
                    }
                case "\\":
                    switch (direction)
                    {
                        case Direction.Up:
                            return Tuple.Create(Direction.Left, source.Left);
                        case Direction.Right:
                            return Tuple.Create(Direction.Down, source.Down);
                        case Direction.Down:
                            return Tuple.Create(Direction.Right, source.Right);
                        case Direction.Left:
                            return Tuple.Create(Direction.Up, source.Up);
                        default:
                            throw new Exception();
                    }
                case "+":
                    switch (direction)
                    {
                        case Direction.Up:
                            return Tuple.Create(Direction.Up, source.Up);
                        case Direction.Right:
                            return Tuple.Create(Direction.Right, source.Right);
                        case Direction.Down:
                            return Tuple.Create(Direction.Down, source.Down);
                        case Direction.Left:
                            return Tuple.Create(Direction.Left, source.Left);
                        default:
                            throw new Exception();
                    }
                default:
                    throw new Exception();
            }
        }

        static Tuple<Direction, Tile> TurnToDirection(Tile source, Direction direction, Turn turn)
        {
            switch (turn)
            {
                case Turn.Left:
                    if (direction == Direction.Up)
                    {
                        return GetTurnByDirection(source, Direction.Left);
                    }
                    else
                    {
                        return GetTurnByDirection(source, direction - 1);
                    }
                case Turn.Right:
                    if (direction == Direction.Left)
                    {
                        return GetTurnByDirection(source, Direction.Up);
                    }
                    else
                    {
                        return GetTurnByDirection(source, direction + 1);
                    }
                case Turn.Straight:
                    return GetTurnByDirection(source, direction);
                default:
                    throw new Exception();
            }
        }

        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("input.txt");
            Tile[,] map = new Tile[lines[0].Length, lines.Length];
            int width = lines[0].Length;
            int height = lines.Length;

            for (int j = 0; j < width; j++)
            {
                for (int i = 0; i < height; i++)
                {
                    map[i, j] = new Tile();
                }
            }

            for (int j = 0; j < width; j++)
            {
                for (int i = 0; i < height; i++)
                {
                    if (lines[j][i] == ' ')
                        continue;

                    Tile current = map[i, j];
                    current.X = i;
                    current.Y = j;
                    current.Value = lines[j][i].ToString();

                    switch (current.Value)
                    {
                        case ">":
                            current.Value = "-";
                            current.Carts.Add(new Cart {Direction = Direction.Right});
                            goto horizontal;
                        case "<":
                            current.Value = "-";
                            current.Carts.Add(new Cart {Direction = Direction.Left});
                            goto horizontal;
                        case "^":
                            current.Value = "|";
                            current.Carts.Add(new Cart {Direction = Direction.Up});
                            goto vertical;
                        case "v":
                            current.Value = "|";
                            current.Carts.Add(new Cart {Direction = Direction.Down});
                            goto vertical;
                        case "|":
                            vertical:
                            current.Up = map[i, j - 1];
                            current.Down = map[i, j + 1];
                            break;
                        case "-":
                            horizontal:
                            current.Left = map[i - 1, j];
                            current.Right = map[i + 1, j];
                            break;
                        case "+":
                            current.Left = map[i - 1, j];
                            current.Right = map[i + 1, j];
                            current.Up = map[i, j - 1];
                            current.Down = map[i, j + 1];
                            break;
                        case "/":
                        case "\\":
                            if (i > 0 && (lines[j][i - 1] == '-' || lines[j][i - 1] == '>' || lines[j][i - 1] == '<' || lines[j][i - 1] == '+'))
                                current.Left = map[i - 1, j];
                            if (i < width-1 && (lines[j][i + 1] == '-' || lines[j][i + 1] == '>' || lines[j][i + 1] == '<' || lines[j][i + 1] == '+'))
                                current.Right = map[i + 1, j];
                            if (j > 0 && (lines[j - 1][i] == '|' || lines[j - 1][i] == 'v' || lines[j - 1][i] == '^' || lines[j - 1][i] == '+'))
                                current.Up = map[i, j - 1];
                            if (j < height - 1 && (lines[j + 1][i] == '|' || lines[j + 1][i] == 'v' || lines[j + 1][i] == '^' || lines[j + 1][i] == '+'))
                                current.Down = map[i, j + 1];
                            break;
                        default:
                            throw new Exception();
                    }
                }
            }

            for (int frame = 0; frame < Int32.MaxValue; frame++)
            {
                List<Tile> remaining = new List<Tile>();
                for (int j = 0; j < width; j++)
                {
                    for (int i = 0; i < height; i++)
                    {
                        bool crashed = map[i, j].Carts.Count > 1;
                        foreach (Cart cart in map[i, j].Carts)
                        {
                            cart.Moved = false;
                            cart.Crashed = crashed;
                        }

                        if (map[i, j].Carts.Any() && map[i, j].Carts.All(c => !c.Crashed))
                            remaining.Add(map[i, j]);
                    }
                }

                if (remaining.Count == 1)
                {
                    Console.WriteLine($"Only one cart is remaining: {remaining.First().X},{remaining.First().Y}");
                    goto finish;
                }

                for (int j = 0; j < width; j++)
                {
                    for (int i = 0; i < height; i++)
                    {
                        Tile tile = map[i, j];
                        Cart cart = tile.Carts.FirstOrDefault(c => !c.Moved && !c.Crashed);
                        if (cart == null)
                            continue;

                        cart.Moved = true;
                        //for (int k = 0; k < tile.Carts.Count; k++)
                        {

                            switch (tile.Value)
                            {
                                case "-":
                                case "|":
                                    Tile next = GetTileByDirection(tile, cart.Direction);
                                    if (next.Carts.Any())
                                    {
                                        Console.WriteLine($"Carts crashing at {next.X},{next.Y}");
                                        tile.Carts.Remove(cart);
                                        next.Carts.Clear();
                                    }
                                    else
                                    {
                                        tile.Carts.Remove(cart);
                                        next.Carts.Add(cart);
                                    }
                                    break;
                                case "/":
                                case "\\":
                                    Tuple<Direction, Tile> turn = GetTurnByDirection(tile, cart.Direction);
                                    if (turn.Item2.Carts.Any())
                                    {
                                        Console.WriteLine($"Carts crashing at {turn.Item2.X},{turn.Item2.Y}");
                                        tile.Carts.Remove(cart);
                                        turn.Item2.Carts.Clear();
                                    }
                                    else
                                    {
                                        tile.Carts.Remove(cart);
                                        turn.Item2.Carts.Add(cart);
                                        cart.Direction = turn.Item1;
                                    }

                                    break;
                                case "+":
                                    Tuple<Direction, Tile> cross = TurnToDirection(tile, cart.Direction, cart.Turn);
                                    if (cross.Item2.Carts.Any())
                                    {
                                        Console.WriteLine($"Carts crashing at {cross.Item2.X},{cross.Item2.Y}");
                                        tile.Carts.Remove(cart);
                                        cross.Item2.Carts.Clear();
                                    }
                                    else
                                    {
                                        tile.Carts.Remove(cart);
                                        cross.Item2.Carts.Add(cart);
                                        cart.Direction = cross.Item1;
                                        if (cart.Turn == Turn.Right)
                                        {
                                            cart.Turn = Turn.Left;
                                        }
                                        else
                                        {
                                            cart.Turn = cart.Turn + 1;
                                        }
                                    }

                                    break;
                            }
                        }
                    }
                }
            }
            
            finish:
            Console.ReadKey();
        }
    }
}
