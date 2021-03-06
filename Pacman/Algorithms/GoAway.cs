﻿using System.Collections.Generic;
using PacMan.Interfaces;

namespace PacMan.Algorithms
{
    class GoAway : IStrategy
    {
        private readonly IStrategy _strategy;

        public GoAway()
        {
            _strategy = new AstarAlgorithmOptimization();
        }
        
        public Stack<Position> FindPath(IMap map, Position start, Position goal)
        {
            int x = map.Widht / 2;
            int y = map.Height / 2;
            Position value = goal;
            
                if (goal.X < x && goal.Y < y)
                    value = new Position(map.Widht - 3, map.Height - 2);
                if (goal.X >= x && goal.Y < y)
                    value = new Position(2, map.Height - 2);
                if (goal.X < x && goal.Y >= y)
                    value = new Position(map.Widht - 3, 1);
                if (goal.X >= x && goal.Y >= y)
                    value = new Position(2, 1);
               
            return _strategy.FindPath(map, start, value);
        }
    }
}
