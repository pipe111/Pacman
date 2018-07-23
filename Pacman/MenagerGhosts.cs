﻿using PacMan.Abstracts;
using PacMan.Algorithms;
using PacMan.ExtensionClasses;
using PacMan.Interfaces;
using PacMan.Players;
using PacMan.StateBehavior;
using System;
using System.Collections.ObjectModel;
using System.Timers;

namespace PacMan
{
    class MenagerGhosts
    {
        private ChangeStateGhosts ChangeStateChosts { set; get; }
        private readonly Timer timeFrightened;

        public Collection<Ghost> Ghosts { get; set; }
        public Blinky Blinky { get; set; }
        public Clyde Clyde { get; set; }
        public Inky Inky { get; set; }
        public Pinky Pinky { get; set; }
        public IState State { get; set; }

        public MenagerGhosts(Map map, int time)
        {
            timeFrightened = new Timer(10000);

            Ghosts = new Collection<Ghost>();
            State = new StateScatter();
            Blinky = new Blinky(map, time);
            Clyde = new Clyde(map, time);
            Inky = new Inky(map, time);
            Pinky = new Pinky(map, time);

            AddGhostsInCollection();

            ChangeStateChosts = new ChangeStateGhosts(this);
        }

        public void Restart()
        {
            StartPosition();
            State = new StateScatter();
            ChangeStateChosts = new ChangeStateGhosts(this);
            SetStrategy(new RandomMoving());
            OldCoordSetEmtry();
            foreach(var ghost in Ghosts)
            {
                if(ghost.Frightened)
                {
                    ghost.Frightened = false;
                    ghost.DefaultTime();
                }
            }
        }

        public void OldCoordSetEmtry()
        {
            foreach(var ghost in Ghosts)
            {
                ghost.OldCoord = new Empty(ghost.Position);
            }
        }

        public void SetDefaultMap(Map map)
        {
            foreach (var ghost in Ghosts)
            {
                ghost.Map = map;
            }
        }

        public void StartPosition()
        {
            foreach (var ghost in Ghosts)
            {
                ghost.StartPosition();
            }
        }
        public void SetStrategy(IStrategy strategy)
        {
            foreach (var ghost in Ghosts)
            {
                ghost.Strategy = strategy;
            }

        }

        public void SetGhosts()
        {
            foreach (var ghost in Ghosts)
            {
                ghost.SetOnMap();
                ghost.OldCoord = new Empty(ghost.Position);
            }
        }

        public void RemoveGhosts()
        {
            foreach (var ghost in Ghosts)
            {
                ghost.RemoveFromMap();
            }
        }

        public void AreFrightened()
        {
            foreach (var ghost in Ghosts)
            {
                ghost.SpeedDownAt(1.5);
                ghost.Frightened = true;
                ghost.OldStrategy = ghost.Strategy;
                ghost.Strategy = new GoAway();
            }
            timeFrightened.Start(Timer_Elapsed);

            ChangeStateChosts.Stop();
        }

        public void ArenotFrightened()
        {
            foreach (var ghost in Ghosts)
            {
                ghost.DefaultTime();
                ghost.Strategy = ghost.OldStrategy;
                ghost.Frightened = false;
            }
            timeFrightened.Stop(Timer_Elapsed);

            ChangeStateChosts.Start();
        }
        
        public void AddSinkAboutEatPacmanHandler(Action action)
        {
            foreach (var ghost in Ghosts)
            {
                ghost.SinkAboutEatPacman += action;
            }
        }
        
        public void AddMoveHandler(Action<ICoord> action)
        {
            foreach (var ghost in Ghosts)
            {
                ghost.Movement += action;
            }
        }

        public void StartTimer()
        {
            foreach (var ghost in Ghosts)
            {
                ghost.Start();
            }
            ChangeStateChosts.Start();
        }

        public void StopTimer()
        {
            foreach (var ghost in Ghosts)
            {
                ghost.Stop();
            }
            ChangeStateChosts.Stop();
        }

        private void AddGhostsInCollection()
        {
            Ghosts.Add(Blinky);
            Ghosts.Add(Clyde);
            Ghosts.Add(Inky);
            Ghosts.Add(Pinky);
        }
        
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ArenotFrightened();
        }
    }
}