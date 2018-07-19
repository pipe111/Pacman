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

        private void Pacman_SinkAboutNextLevel()
        {
            Stop();
            SetDirection(Direction.None);
            RemovePlayers();
            Map = (Map)DefaultMap.Clone();
            Pacman.Map = Map;
            Ghosts.SetDefaultMap(Map);
            UpdateMap();
            //RemovePlayers();
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
                PacmanIsLive = true;
                CreatePlayers();
            }
        }

        public void End()
        {
            Status = GameStatus.TheEnd;
            Dispose();
        }

        private void PacmanIsKilled()
        {
            PacmanIsLive = false;
            PacmanIsDied();
        }

        private void CreatePlayers()
        {
            Pacman.SeteOnMap();
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