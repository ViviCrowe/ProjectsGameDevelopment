﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Numerics;

namespace RogueLike.Classes
{
    public class Room
    {
        public Tile[,] Tiles { get; set; }
        public Vector2 Position;
        public List<GameObject> passiveObjects = new List<GameObject>();
        public List<Entity> activeObjects = new List<Entity>();
        private Viewport viewport;

        public Room(Viewport viewport)
        {
            this.viewport = viewport;
            //Es funktionieren aktuell nur ungerade Anzahlen
            Tiles = new Tile[11,11];
            Position.X = (viewport.Width / 2) - ((Tiles.GetLength(0) - 1)*64 /2);
            Position.Y = (viewport.Height / 2) - ((Tiles.GetLength(0) - 1)*64 / 2);
        }

        public void LoadRoomAssets(ContentManager content)
        {
            for (int i = 0; i < Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < Tiles.GetLength(1); j++)
                {
                    if(i == 0 || j == 0 || i == Tiles.GetLength(0) -1 || j == Tiles.GetLength(0) - 1)
                    {
                        Tiles[i, j] = new Tile(Position, true);
                        Tiles[i, j].LoadAssets(content, "wall");
                        passiveObjects.Add(Tiles[i, j]);
                    } 
                    else
                    {
                        Tiles[i, j] = new Tile(Position, false);
                        Tiles[i, j].LoadAssets(content, "Grass_normal");
                    }
                    
                    Position.X += 64;
                }
                Position.X = viewport.Width / 2 - (Tiles.GetLength(0) - 1) * 64 / 2;
                Position.Y += 64;
            }
        }

        public void DrawRoom(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < Tiles.GetLength(1); j++)
                {                   
                    Tiles[i, j].Draw(spriteBatch);
                }
            }
        }
    }
}
