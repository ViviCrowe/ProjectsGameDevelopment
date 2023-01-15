﻿using System;
using Microsoft.Xna.Framework.Graphics;

namespace RogueLike.Classes
{
    public class Level
    {
        public Room[] Rooms { get; set; }

        private Viewport viewport;

        private static Random random = new Random();

        private bool lastLevel;

        public Level(Viewport viewport, int roomNumber, bool lastLevel)
        {
            this.viewport = viewport;
            Rooms = new Room[roomNumber];
            this.lastLevel = lastLevel;
        }

        public void generateLevel()
        {
            int
                width,
                height;

            for (int i = 0; i < Rooms.Length; i++)
            {
                width = random.Next(9, 15);
                if (width % 2 == 0) width++;
                height = random.Next(15, 29);
                if (height % 2 == 0) height++;
                if (i == 0)
                {
                    Rooms[i] = new Room(viewport, true, false, lastLevel, width, height);
                }
                else if (i == Rooms.Length - 1)
                {
                    Rooms[i] = new Room(viewport, false, true, lastLevel, width, height);
                }
                else
                {
                    Rooms[i] = new Room(viewport, false, false, lastLevel, width, height);
                }
            }
        }
    }
}
