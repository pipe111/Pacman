﻿using PacMan.Algorithms.Astar;
using PacMan.Interfaces;

namespace PacMan.StateBehavior
{
    class StateScatter : IState
    {
        public void ChangeBehavior(MenegerGhosts ghosts)
        {
            foreach (var ghost in ghosts.Ghosts)
            {
                ghost.Strategy = new AstarAlgorithm();
            }
            ghosts.State = new StateAttack();
        }
    }
}
