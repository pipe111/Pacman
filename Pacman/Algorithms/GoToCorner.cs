﻿using System.Collections.Generic;
using PacMan.Interfaces;

namespace PacMan.Algorithms
{
    class GoToCorner : IStrategy
    {
        private readonly IStrategy strategy;

        public GoToCorner()
        {
            strategy = new AstarAlgorithmOptimization();
        }
        
        public Stack<Position> FindPath(IMap map, Position start, Position goal)
        {
            return strategy.FindPath(map, start, goal);
        }
    }
}
