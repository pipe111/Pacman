﻿using PacMan.Abstracts;
using PacMan.Foods;
using PacMan.Interfaces;
using PacMan.Enums;
using System;
using System.Linq;
using System.Timers;
using System.Threading.Tasks;

namespace PacMan.Players
{
    class Pacman : Player, IPacman
    {
        public override event Func<ICoord, Task> Movement;
        public event Action SinkAboutCreateCherry;
        public event Action SinkAboutEatEnergizer;
        public event Action SinkAboutNextLevel;
        public event Action SinkAboutEatGhost;
        public event Func<Task> SinkAboutChangeScore;

        private int _count;

        public Direction NewDirection { get; set; }
        public int Lives { get; set; }
        public int Level { get; set; }
        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                SinkAboutChangeScore?.Invoke();
                if (_count % 1000 == 700)
                {
                    SinkAboutCreateCherry?.Invoke();
                }
            }
        }

        public Pacman(Position start, Map map) : base(start, map)
        {
            DefaultCoord();
            Id = "pacman";
            idchar = 'P';

            Direction = Direction.None;
            NewDirection = Direction.None;
            Count = 0;
            Lives = 3;
            Level = 1;
        }

        public override void StartPosition()
        {
            Map[Position] = new Empty(Position);
            Movement?.Invoke(new Empty(Position));
            DefaultCoord();
            Map[Position] = this;
            Movement?.Invoke(this);
        }

        public override void DefaultMap(Map map)
        {
            Direction = Direction.None;
            NewDirection = Direction.None;
            Level = 1;
            Count = 0;
            Lives = 3;
            base.DefaultMap(map);
        }

        protected override void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Movement.Invoke(new Empty(Position));
            if (NewDirection != Direction)
            {
                if (Move(NewDirection))
                {
                    Direction = NewDirection;
                }
                else
                {
                    Move();
                }
            }
            else
            {
                Move();
            }
            Movement.Invoke(Map[Position]);
            MaybeNextLevel();
        }

        public void Eat(IFood food)
        {
            if (food is IGhost ghost)
            {
                if (ghost.Frightened)
                {
                    Count += food.Score;
                    ghost.Restart();
                    SinkAboutEatGhost?.Invoke();
                }
            }
            else
            {
                if (food is Energizer)
                {
                    SinkAboutEatEnergizer?.Invoke();
                }
                Count += food.Score;
            }
        }

        public override bool Move()
        {
            return Move(Direction);
        }

        public override string GetId()
        {
            return Id + Direction.ToString().ToLower();
        }

        protected override bool MoveRight()
        {
            if (Position.X + 2 > Map.Widht)
            {
                Map[Position] = new Empty(Position);
                Position position = Position;
                position.X = 0;
                Position = position;
                Map[Position] = this;
                return true;
            }
            else
            {
                if (Map[Position.Right] is IFood food)
                    Eat(food);
                return base.MoveRight();
            }
        }

        protected override bool MoveLeft()
        {
            if (Position.X - 1 < 0)
            {
                Map[Position] = new Empty(Position);
                Position position = Position;
                position.X = Map.Height - 2;
                Position = position;
                Map[Position] = this;
                return true;
            }
            else
            {
                if (Map[Position.Left] is IFood food)
                    Eat(food);
                return base.MoveLeft();
            }
        }

        protected override bool MoveDown()
        {
            if (Map[Position.Down] is IFood food)
                Eat(food);
            return base.MoveDown();
        }

        protected override bool MoveUp()
        {
            if (Map[Position.Up] is IFood food)
                Eat(food);
            return base.MoveUp();
        }

        private void MaybeNextLevel()
        {
            var IsLittleGoal = Map.map.OfType<ICoord>().AsQueryable().Any(m => m is LittleGoal);
            if (!IsLittleGoal)
            {
                Level++;
                SinkAboutNextLevel();
            }
        }

        private bool Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return MoveLeft();
                case Direction.Right:
                    return MoveRight();
                case Direction.Up:
                    return MoveUp();
                case Direction.Down:
                    return MoveDown();
                default:
                    return false;
            }
        }
    }
}