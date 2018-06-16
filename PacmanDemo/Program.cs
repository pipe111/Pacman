﻿using PacMan;
using PacMan.Interfaces;
using PacMan.Foods;
using System;
using PacMan.Players;

namespace PacmanDemo
{
    class Program
    {
        const int SIZE = 16;
        static void Main(string[] args)
        {
            var size = new Size(32, 16);
            var game = new Game(@"C:\Users\fedyu\source\repos\pacman\PacmanDemo\map.txt", size);
            bool lost = true;
            while (true)
            {
                if (lost==false)
                {
                    game.pacman.Lives--;
                    Console.Clear();
                    if (game.pacman.Lives != 0)
                    {

                        string liveorlives = game.pacman.Lives == 1 ? "live" : "lives";
                        Console.WriteLine($"You lost,you have more {game.pacman.Lives} {liveorlives}");
                        Console.WriteLine("Press the spacebar to continue the game");
                        while (true)
                        {
                            ConsoleKeyInfo space = Console.ReadKey(true);
                            if (space.Key == ConsoleKey.Spacebar)
                                break;
                        }
                        game.Start();
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("You lost");
                        break;
                    }
                }
                string LiveorLives= game.pacman.Lives == 1 ? "Live" : "Lives";
                Console.Clear();
                ShowMap(game.map);
                Console.WriteLine($"{LiveorLives} {game.pacman.Lives} ");
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.LeftArrow)
                {
                    lost=game.Move(Direction.Left);
                }
                if (key.Key == ConsoleKey.RightArrow)
                {
                    lost = game.Move(Direction.Right);
                }
                if (key.Key == ConsoleKey.UpArrow)
                {
                    lost = game.Move(Direction.Up);
                }
                if (key.Key == ConsoleKey.DownArrow)
                {
                    lost = game.Move(Direction.Down);
                }
            }
            Console.ReadLine();
        }

        public static void ShowMap(IMap map)
        {
            int[,] array = map.map;
            for(int y=0; y<SIZE; y++)
            {
                for(int x=0; x<2*SIZE;x++)
                {
                    switch(array[x,y])
                    {
                        case 0:
                            Console.Write(Empty.GetCharElement());
                            break;
                        case 1:
                            Console.Write(Wall.GetCharElement());
                            break;
                        case 2:
                            Console.Write(LittleGoal.GetCharElement());
                            break;
                        case 5:
                            Console.Write(Pacman.GetCharElement());
                            break;
                        case 7:
                            Console.Write(Clyde.GetCharElement());
                            break;
                        default:
                            Console.Write(' ');
                            break;
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
