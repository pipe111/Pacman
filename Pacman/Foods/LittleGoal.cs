﻿using PacMan.Abstracts;
using PacMan.Interfaces;

namespace PacMan.Foods
{
    class LittleGoal: Food, IGetChar
    {
        public LittleGoal()
        { }

        public LittleGoal(Position position) : base(position)
        {
            Score = 10;
        }

        public override char GetCharElement()
        {
            return (char)183;
        }

        public override string GetId()
        {
            return "littlegoal";
        }
    }
}
