using System.Collections.Generic;
using System.Numerics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RogueLike.Classes.Weapons;

namespace RogueLike.Classes
{
    public class Room
    {
        public Tile[,] Tiles { get; set; }

        public Vector2 Position;

        public List<GameObject> passiveObjects = new List<GameObject>();

        public List<Entity> activeObjects = new List<Entity>();

        public List<Weapon> items = new List<Weapon>(); // haben (noch?) keine item klasse

        private Viewport viewport;

        public Room(Viewport viewport)
        {
            this.viewport = viewport;

            //Es funktionieren aktuell nur ungerade Anzahlen
            Tiles = new Tile[17, 30];
            /*Position.X =
                (viewport.Width / 2) - ((Tiles.GetLength(0) - 1) * 64 / 2);
            Position.Y =
                (viewport.Height / 2) - ((Tiles.GetLength(0) - 1) * 64 / 2);*/

            Position.X = 32;
            Position.Y = 32;
        }

        public void LoadAssets(ContentManager content)
        {
            for (int i = 0; i < Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < Tiles.GetLength(1); j++)
                {
                    if (
                        i == 0 ||
                        j == 0 ||
                        i == Tiles.GetLength(0) - 1 ||
                        j == Tiles.GetLength(1) - 1
                    )
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
                Position.X = 32;
                Position.Y += 64;
            }
            LoadEntityAssets(content);
            LoadItemAssets(content);
        }

        public void LoadItemAssets(ContentManager content)
        {
            foreach (Weapon item in items) // bis jetzt nur Waffen keine Items
            {
                if (item != null)
                {
                    item.LoadAssets(content, "sword"); // TEST NAME
                }
            }
        }

        public void LoadEntityAssets(ContentManager content)
        {
            foreach(Entity entity in activeObjects) 
            {
                if(entity != null)
                {
                    entity.LoadAssets(content, "enemy"); // TEST NAME
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < Tiles.GetLength(1); j++)
                {
                    Tiles[i, j].Draw(spriteBatch, 1f);
                }
            }

            foreach (Weapon item in items)
            {
                if (item != null)
                {
                    item.Draw(spriteBatch, 0.9f);
                }
            }

            foreach (Entity entity in activeObjects) 
            {
                if(entity != null) 
                {
                    entity.Draw(spriteBatch, 0.2f);
                }
            }
        }
    }
}
