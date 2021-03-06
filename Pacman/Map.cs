﻿using Newtonsoft.Json;
using PacMan.Foods;
using PacMan.Interfaces;
using PacMan.Players;
using System;
using System.IO;

namespace PacMan
{
    public class Map : IMap, ICloneable
    {
        public ICoord[,] map { get; set; }
        public int Widht { get; set; }
        public int Height { get; set; }
        public string Name { get; private set; }

        internal Pacman Pacman { get; private set; }
        internal Inky Inky { get; private set; }
        internal Pinky Pinky { get; private set; }
        internal Blinky Blinky { get; private set; }
        internal Clyde Clyde { get; private set; }

        public Map(string path, string name)
        {
            Name = name;
            map = LoadMap(path);
        }

        public object Clone()
        {
            Map board = (Map)MemberwiseClone();
            board.map = (ICoord[,])map.Clone();
            return board;
        }

        public ICoord this[IPosition index]
        {
            get => map[index.X, index.Y];
            set => map[index.X, index.Y] = value;
        }

        public string[,] GetArrayID()
        {
            string[,] array = new string[Widht, Height];
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Widht; x++)
                {
                    array[x, y] = map[x, y].GetId();
                }
            }
            return array;
        }

        public bool OnMap(IPosition position)
        {
            return position.X >= 0 && position.X < Widht &&
                position.Y >= 0 && position.Y < Height;
        }

        private ICoord[,] LoadMap(string path)
        {
            StreamReader FileWithMap = new StreamReader(path);
            string all = FileWithMap.ReadToEnd();
            FileWithMap.Close();
            var array = JsonConvert.DeserializeObject<string[,]>(all);

            Widht = array.GetLength(1);
            Height = array.GetLength(0);

            ICoord[,] maze = new ICoord[Widht, Height];
            
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Widht; x++)
                {
                    switch (array[y, x])
                    {
                        case "emtry":
                            maze[x, y] = new Empty(new Position(x, y));
                            break;
                        case "wall":
                            maze[x, y] = new Wall(new Position(x, y));
                            break;
                        case "littlegoal":
                            maze[x, y] = new LittleGoal(new Position(x, y));
                            break;
                        case "energaizer":
                            maze[x, y] = new Energizer(new Position(x, y));
                            break;
                        case "cherry":
                            maze[x, y] = new Cherry(new Position(x, y), this);
                            break;
                        case "pacmannone":
                            Pacman = new Pacman(new Position(x, y), this);
                            maze[x, y] = Pacman;
                            break;
                        case "clyde":
                            Clyde = new Clyde(new Position(x, y), this);
                            maze[x, y] = Clyde;
                            break;
                        case "blinky":
                            Blinky = new Blinky(new Position(x, y), this);
                            maze[x, y] = Blinky;
                            break;
                        case "inky":
                            Inky = new Inky(new Position(x, y), this);
                            maze[x, y] = Inky;
                            break;
                        case "pinky":
                            Pinky = new Pinky(new Position(x, y), this);
                            maze[x, y] = Pinky;
                            break;
                        default:
                            continue;
                    }
                }
            }
            return maze;
        }
    }
}