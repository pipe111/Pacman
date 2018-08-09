﻿using PacMan.Abstracts;
using PacMan.Algorithms;

namespace PacMan.Players
{
    class Clyde : Ghost
    {
        public override Position StartCoord
        {
            get => base.StartCoord;
            set
            {
                base.StartCoord = value;
                homePosition = new Position(3, Map.Height - 5);
            }
        }

        public Clyde(Position start, Map map) : base(start, map)
        {
            id = "clyde";
            idchar = 'C';
        }

        public override void StrategyRunForPacman() => Strategy = new AlgorithmForClyde();
        protected override void GoToCircle() => Strategy = new GoAgainstClockwise();

    }
}
