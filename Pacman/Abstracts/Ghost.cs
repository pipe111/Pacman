﻿using PacMan.Foods;
using PacMan.Interfaces;
using PacMan.Players;
using System;
using System.Collections.Generic;
using System.Timers;

namespace PacMan.Abstracts
{
    abstract public class Ghost : Player, IGhost, IFood, ISinkAboutEatPacman
    {
        public abstract event Action SinkAboutEatPacman;

        protected object obj = new object();
        protected bool pacmanIsLive = true;
        protected Stack<Position> path;
        
        public IStrategy Strategy { get; set; }
        public IStrategy OldStrategy { get; set; }
        public ICoord OldCoord { get; set; }
        public Position PacmanPosition { get; set; }
        public bool Frightened { get; set; }
        public int Score { get; set; }
        public bool IsLive { get; set; }

        protected Ghost()
        { }

        protected Ghost(Map map, int time) : base(map, time)
        {
            Timer = new Timer(time);
            StartPosition();
            PacmanPosition = SearchPacman();
            path = new Stack<Position>();
            OldCoord = new Empty(Position);

            Score = 200;
            Frightened = false;
            IsLive = true;
        }

        protected Position SearchPacman()
        {
            for (int y = 0; y < Map.Height; y++)
                for (int x = 0; x < Map.Width; x++)
                    if (Map.map[x, y] is Pacman)
                        return new Position(x, y);
            return PacmanPosition;
        }

        protected virtual ICoord Go(Stack<Position> list, ICoord coord)
        {
            Map.SetElement(coord);
            if (list.Count != 0)
                Position = list.Pop();
            ICoord old = Map.GetElement(Position);
            Map.SetElement(this);
            return old;
        }

        protected bool GhostIsFrightened()
        {
            if (Frightened)
                return true;
            else
            {
                OldCoord = new Empty(Position);
                return false;
            }
        }
    }
}
