﻿using System;
using System.Timers;
using PacMan.ExtensionClasses;
using PacMan.Interfaces;
using PacMan.Enums;
using System.Threading.Tasks;

namespace PacMan.Abstracts
{
    abstract class Player : IPlayer
    {
        public abstract event Func<ICoord, Task> Movement;
        public abstract bool Move();
        public abstract void StartPosition();
        protected abstract void TimerElapsed(object sender, ElapsedEventArgs e);

        protected virtual string Id { get; set; }
        protected char idchar;
        protected readonly Timer timer;

        public Map Map { get; set; }
        public Position Position { get; set; }
        public Direction Direction { get; set; }
        public int Time { get; set; }
        public Position StartCoord { get; set; }

        protected Player(Position start, Map map)
        {
            Map = map;
            StartCoord = start;
            timer = new Timer();
        }

        public virtual string GetId() => Id;
        public char GetCharElement() => idchar;

        public virtual void DefaultCoord() => Position = StartCoord;

        public virtual void Start() => timer.Start(TimerElapsed);
        public virtual void Stop() => timer.Stop(TimerElapsed);

        public virtual void DefaultMap(Map map)
        {
            Map = map;
            DefaultCoord();

        }

        public void SetTime(int time)
        {
            timer.Interval = time;
            Time = time;
        }

        protected virtual bool MoveLeft()
        {
            if (!(Map[Position.Left] is Wall))
            {
                SwapPlacesX(Position.X - 1);
                return true;
            }
            return false;
        }

        protected virtual bool MoveRight()
        {
            if (!(Map[Position.Right] is Wall))
            {
                SwapPlacesX(Position.X + 1);
                return true;
            }
            return false;
        }

        protected virtual bool MoveUp()
        {
            if (!(Map[Position.Up] is Wall))
            {
                SwapPlacesY(Position.Y - 1);
                return true;
            }
            return false;
        }

        protected virtual bool MoveDown()
        {
            if (!(Map[Position.Down] is Wall))
            {
                SwapPlacesY(Position.Y + 1);
                return true;
            }
            return false;
        }

        private void SwapPlacesX(int x)
        {
            var value = Map[Position];
            Map[Position] = new Empty(Position);

            Position position = Position;
            position.X = x;
            Position = position;

            value.Position = Position;
            Map[Position] = value;
        }

        private void SwapPlacesY(int y)
        {
            var value = Map[Position];
            Map[Position] = new Empty(Position);

            Position position = Position;
            position.Y = y;
            Position = position;

            value.Position = Position;
            Map[Position] = value;
        }
    }
}