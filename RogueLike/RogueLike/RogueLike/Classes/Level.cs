using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLike.Classes
{
    public class Level
    {
        public Room[] Rooms { get; set; }

        private Viewport viewport;

        int roomNumber;

        public Level (Viewport viewport, int roomNumber)
        {
            this.viewport = viewport;
            this.roomNumber = roomNumber;
            Rooms = new Room[roomNumber]; 
        }
        

        public void generateLevel()
        {
            for (int i = 0; i < Rooms.Length; i++)
            {
                if(i == 0)
                {
                    Rooms[i] = new Room(viewport, true, false);
                }
                else if(i == Rooms.Length - 1)
                {
                    Rooms[i] = new Room(viewport, false, true);
                }
                else
                {
                    Rooms[i] = new Room(viewport, false, false);
                }
            }
        }
    }
}
