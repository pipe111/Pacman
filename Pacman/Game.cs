﻿using PacMan.Interfaces;
using PacMan.Players;
using PacMan.Foods;
using System;
using PacMan.Enums;

namespace PacMan
{
    public sealed class Game : IGame, IDisposable
    {
        private const int TIME = 400;
        private const int TIMEFORPACMAN = 300;
        private Pacman Pacman { get; set; }
        private Cherry Cherry { get; set; }
        private MenagerGhosts Ghosts { get; set; }
        private Map DefaultMap { get; set; }

        public event Action PacmanIsDied;
        public event Action UpdateMap;
        public bool PacmanIsLive { get; private set; }
        public GameStatus Status { get; set; }
        public Map Map { get; private set; }
        public Game DefaultGame { get; private set; }
        public int Score
        {
            get
            {
                return Pacman.Count;
            }
        }
        public Direction Direction
        {
            get
            {
                return Pacman.Direction;
            }
        }
        public int Lives
        {
            get
            {
                return Pacman.Lives;
            }
        }
        public int Level
        {
            get
            {
                return Pacman.Level;
            }
        }

        public Game(string path, ISize size)
        {
            Status = GameStatus.ReadyToStart;
            PacmanIsLive = true;
            Map = new Map(path, size);
            DefaultMap = (Map)Map.Clone();
            Pacman = new Pacman(Map, TIMEFORPACMAN);
            Cherry = new Cherry(new Position(14, 17), Map);
            Ghosts = new MenagerGhosts(Map, TIME);

            Pacman.SinkAboutEatEnergizer += Ghosts.AreFrightened;
            Pacman.SinkAboutCreateCherry += () => Cherry.Start();
            Pacman.SinkAboutNextLevel += Pacman_SinkAboutNextLevel;
            Ghosts.AddSinkAboutEatPacmanHandler(PacmanIsKilled);
        }

        public void Restart()
        {
            Status = GameStatus.InProcess;
            SetDirection(Direction.None);
            Pacman.Stop();
            Ghosts.StopTimer();
            Map = (Map)DefaultMap.Clone();
            Pacman.StartPosition();
            Ghosts.StartPosition();
            Pacman.Map = Map;
            Ghosts.SetDefaultMap(Map);
            Pacman.Level = 1;
            Pacman.Count = 0;
            Pacman.Lives = 3;
            Ghosts.Restart();
            Pacman.Start();
            Ghosts.StartTimer();
        }

        private void Pacman_SinkAboutNextLevel()
        {
            Stop();
            SetDirection(Direction.None);
            RemovePlayers();
            Map = (Map)DefaultMap.Clone();
            Pacman.Map = Map;
            Ghosts.SetDefaultMap(Map);
            UpdateMap();
            Ghosts.ArenotFrightened();
            CreatePlayers();
        }

        public void AddMoveHandlerToGhosts(Action<ICoord> action)
        {
            Ghosts.AddMoveHandler(action);
            Cherry.Movement += action;
        }

        public void AddMoveHandlerToPacman(Action<ICoord> action)
        {
            Pacman.Movement += action;
        }

        public void SetDirection(Direction direction)
        {
            Pacman.OldDirection = Pacman.Direction;
            Pacman.Direction = direction;
        }

        public void Start()
        {
            Status = GameStatus.InProcess;
            Ghosts.StartTimer();
            Pacman.Start();
        }

        public void Stop()
        {
            Status = GameStatus.Stop;
            Pacman.Direction = Direction.None;
            Pacman.Stop();
            Ghosts.StopTimer();
            if (!PacmanIsLive)
            {
                RemovePlayers();
                Pacman.Lives--;
                if (Pacman.Lives > 0)
                {
                    PacmanIsLive = true;
                    CreatePlayers();
                }
                else
                {
                    Status = GameStatus.TheEnd;
                }
            }
        }

        public void End()
        {
            Status = GameStatus.TheEnd;
            Dispose();
        }

        private void PacmanIsKilled()
        {
            RemovePlayers();
            PacmanIsLive = false;
            PacmanIsDied();
        }

        private void CreatePlayers()
        {
            Pacman.SetOnMap();
            Ghosts.SetGhosts();
        }

        private void RemovePlayers()
        {
            Pacman.RemoveFromMap();
            Ghosts.RemoveGhosts();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }
}