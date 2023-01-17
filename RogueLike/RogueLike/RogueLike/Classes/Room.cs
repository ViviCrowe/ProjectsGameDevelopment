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

        public List<GameObject> items = new List<GameObject>(); // haben (noch?) keine item klasse

        private Viewport viewport;

        bool first, last, lastLevel;

        public Room(Viewport viewport, bool first, bool last, bool lastLevel, int width, int height)
        {
            this.viewport = viewport;

            //Es funktionieren aktuell nur ungerade Anzahlen
            Tiles = new Tile[width, height];
            Position.X =
                (viewport.Width / 2) - ((Tiles.GetLength(1) - 1) * 64 / 2);
            Position.Y =
                (viewport.Height / 2) - ((Tiles.GetLength(0) - 1) * 64 / 2);
            this.first = first;
            this.last = last;
            this.lastLevel = lastLevel; 
        }

        public void LoadAssets(ContentManager content)
        {
            for (int i = 0; i < Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < Tiles.GetLength(1); j++)
                {
                    if (((i == 0 && !last) || (i == Tiles.GetLength(0) - 1 && !first)) && j == Tiles.GetLength(1) / 2)
                    {
                        Tiles[i, j] = new Tile(Position, GameObject.ObjectType.Door);
                        Tiles[i, j].LoadAssets(content, "tuer_offen");
                        passiveObjects.Add(Tiles[i, j]);
                    }
                    else if (
                        i == 0 ||
                        j == 0 ||
                        i == Tiles.GetLength(0) - 1 ||
                        j == Tiles.GetLength(1) - 1
                    )
                    {
                        Tiles[i, j] = new Tile(Position, GameObject.ObjectType.Wall);
                        Tiles[i, j].LoadAssets(content, "wall");
                        passiveObjects.Add(Tiles[i, j]);
                    }
                    else if (!lastLevel && last && i == 1 && j == Tiles.GetLength(1) / 2)
                    {
                        Tiles[i, j] = new Tile(Position, GameObject.ObjectType.Hole);
                        Tiles[i, j].LoadAssets(content, "hole");
                        passiveObjects.Add(Tiles[i, j]);
                    }
                    else
                    {
                        Tiles[i, j] = new Tile(Position, GameObject.ObjectType.Floor);
                        Tiles[i, j].LoadAssets(content, "Grass_normal");
                    }

                    Position.X += 64;
                }
                Position.X =
                    viewport.Width / 2 - (Tiles.GetLength(1) - 1) * 64 / 2;
                Position.Y += 64;
            }
            LoadEntityAssets(content);
            LoadItemAssets(content);
        }

        public void LoadItemAssets(ContentManager content)
        {
            foreach (GameObject item in items) 
            {
                if (item != null)
                {
                    if(item is Weapon)
                    {
                        item.LoadAssets(content, "sword"); // TEST NAME
                    }
                    else if(item is Wallet)
                    {
                        item.LoadAssets(content, "teeth");
                    }
                }
            }
        }

        public void LoadEntityAssets(ContentManager content)
        {
            foreach (Entity entity in activeObjects)
            {
                if(entity is Enemy)
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

            foreach (GameObject item in items)
            {
                if (item != null)
                {
                    item.Draw(spriteBatch, 0.9f);
                }
            }

            foreach (Entity entity in activeObjects)
            {
                if (entity != null)
                {
                    entity.Draw(spriteBatch, 0.2f);
                }
            }
        }
    }
}
