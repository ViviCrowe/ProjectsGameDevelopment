using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RogueLike.Classes.Weapons;
using static System.Math;

namespace RogueLike.Classes
{
    public class Room
    {
        public Vector2 tileDimensions = new(64, 64);
        public Vector2 gridDimensions, offset;

        public Tile[,] Tiles;

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
            Position.X = this.offset.X =
                (viewport.Width / 2) - ((Tiles.GetLength(1) - 1) * 64 / 2);
            Position.Y = this.offset.Y =
                (viewport.Height / 2) - ((Tiles.GetLength(0) - 1) * 64 / 2);
            this.first = first;
            this.last = last;
            this.lastLevel = lastLevel;

            this.gridDimensions = new(Tiles.GetLength(0), Tiles.GetLength(1));
            }

        public void LoadAssets(ContentManager content)
        {
            for (int i = 0; i < Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < Tiles.GetLength(1); j++)
                {
                    if (((i == 0 && !last) || (i == Tiles.GetLength(0) - 1 && !first)) && j == Tiles.GetLength(1) / 2)
                    {
                        Tiles[i, j] = new Tile(Position, GameObject.TileType.Door);
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
                        Tiles[i, j] = new Tile(Position, GameObject.TileType.Wall);
                        Tiles[i, j].LoadAssets(content, "wall");
                        passiveObjects.Add(Tiles[i, j]);
                    }
                    else if (!lastLevel && last && i == 1 && j == Tiles.GetLength(1) / 2)
                    {
                        Tiles[i, j] = new Tile(Position, GameObject.TileType.Hole);
                        Tiles[i, j].LoadAssets(content, "hole");
                        passiveObjects.Add(Tiles[i, j]);
                    }
                    else
                    {
                        Tiles[i, j] = new Tile(Position, GameObject.TileType.Floor);
                        Tiles[i, j].LoadAssets(content, "Grass_normal");
                    }

                    Position.X += tileDimensions.X;
                }
                Position.X =
                    viewport.Width / 2 - (Tiles.GetLength(1) - 1) * tileDimensions.X / 2;
                Position.Y += tileDimensions.Y;
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
                    else if(item is Potion)
                    {
                        item.LoadAssets(content, "potion"); 
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

        public Tile GetTileFromPos(Vector2 position)
        {
            int col = (int) ((position.X - this.offset.X) / tileDimensions.X);
            int row = (int) ((position.Y - this.offset.Y) / tileDimensions.Y);
            return Tiles[row, col];
        }

        public Tile GetTile(int row, int col)
        {
            return Tiles[row, col];
        }
        
        public void FollowPath(Enemy enemy)
        {
            if(enemy.destinationTile != null)
            {
                Tile temp = enemy.destinationTile;
                while(temp.parent != null) 
                {
                    temp = temp.parent;
                }
                enemy.Move(this, temp);
            }
        }

        public bool StartAStar(Enemy enemy) 
        {
            for(int row = 0; row < gridDimensions.X; row++)
                for(int col = 0; col < gridDimensions.Y; col++)
                {
                    Tiles[row, col].visited = false;
                    Tiles[row, col].currentDistance = double.PositiveInfinity;
                    Tiles[row, col].fScore = double.PositiveInfinity;
                    Tiles[row, col].parent = null;
                }
            
            Tile currentTile = GetTileFromPos(enemy.position);
            currentTile.fScore = 0f;
            currentTile.currentDistance = CalculateHeuristic(currentTile, enemy.destinationTile);

            Stack<Tile> openList = new Stack<Tile>();
            openList.Push(currentTile);
            
            while(openList.Count > 0 && currentTile != enemy.destinationTile)
            {
                openList = Sort(openList);
        
                while(openList.Count > 0 && openList.Peek().visited)
                {
                    openList.Pop();
                }
                if(openList.Count > 0) 
                {
                    break;
                }
                
                currentTile = openList.Peek();
                currentTile.visited = true;

                int i = 0;
                int j = 0;
                while(Tiles[i, j] != currentTile) {
                    i++;
                    j++;
                }
                
                for (int x = -1; x <= 1; x++) // alle 4 Nachbartiles durchlaufen
                {
                    for (int y = -1; y <= 1; y++) 
                    {
                        Tile neighbourTile = Tiles[i+x, j+y]; 
                        if(!neighbourTile.visited && !neighbourTile.obstacle)
                        {
                            openList.Push(neighbourTile); 
                        }
                        double fScore_2 = currentTile.fScore + CalculateHeuristic(currentTile, neighbourTile);

                        if(fScore_2 < neighbourTile.fScore)
                        {
                            neighbourTile.parent = currentTile;
                            neighbourTile.fScore = fScore_2;
                            neighbourTile.currentDistance = neighbourTile.fScore + CalculateHeuristic(neighbourTile, enemy.destinationTile);
                        }
                    }
                }
            }
            return true;
        }

        public double CalculateHeuristic(Tile tileA, Tile tileB)
        {
            return Sqrt(Pow((tileA.position.X - tileB.position.X), 2.0) + Pow((tileA.position.Y - tileB.position.Y), 2.0));
        }

        private Stack<Tile> Sort(Stack<Tile> stack)
        {
            Stack<Tile> temp = new Stack<Tile>();
            while (stack.Count > 0)
            {
                Tile element = stack.Pop();
                while (temp.Count > 0 && element.currentDistance > temp.Peek().currentDistance)
                {
                    stack.Push(temp.Pop());
                }
                temp.Push(element);
            }
            return temp;
        } 
    }
    
}
