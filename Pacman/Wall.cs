﻿using PacMan.Interfaces;


namespace PacMan
{
    public class Wall: ICoord
    {
        public Position Position { get; set; }

        public Wall(Position position)
        {
            this.Position = position;
        }

        public static char GetCharElement()
        {
            return '#';
        }
    }
}
