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

        public Level (Viewport viewport)
        {
            this.viewport = viewport;
            Rooms = new Room[3]; 
        }
        

        public void generateLevel()
        {
            Rooms[0] = new Room(viewport);
            Rooms[1] = new Room(viewport);
            Rooms[2] = new Room(viewport);
        }
    }
}
