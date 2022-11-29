using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RogueLike.Classes
{
    public class Room
    {
        public Tile[,] Tiles { get; set; }
        public Vector2 Position;

        public Room()
        {
            Tiles = new Tile[11,11];
            Position.X = (1920f / 2) - (10*64 /2);
            Position.Y = (1080f / 2) - (10*64 / 2);
        }

        public void DrawRoom(ContentManager content, SpriteBatch spriteBatch)
        {
            
        }
    }
}
