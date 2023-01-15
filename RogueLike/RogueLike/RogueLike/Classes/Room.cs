using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RogueLike.Classes.Items;
using RogueLike.Classes.Weapons;
using static System.Math;

namespace RogueLike.Classes
{
    public class Room
    {
        private Vector2 tileDimensions = new(64, 64);
        public Vector2 GridDimensions;
        private Vector2 offset;

        public Tile[,] Tiles { get; set; }

        private Vector2 position;
        public List<GameObject> passiveObjects { get; set; } = new List<GameObject>();

        public List<Entity> activeObjects { get; set; } = new List<Entity>();

        public List<GameObject> items { get; set; } = new List<GameObject>();

        private Viewport viewport;

        public bool Last { get; set; }
        public bool LastLevel { get; set; }

        public enum DoorType
        {
            None,
            Left,
            Right,
            Top,
            Bottom
        }

        public Room(Viewport viewport, bool last, bool lastLevel, int width, int height)
        {
            this.viewport = viewport;

            //Es funktionieren aktuell nur ungerade Anzahlen
            Tiles = new Tile[width, height];
            position.X = this.offset.X =
                (viewport.Width / 2) - ((Tiles.GetLength(1) - 1) * 64 / 2);
            position.Y = this.offset.Y =
                (viewport.Height / 2) - ((Tiles.GetLength(0) - 1) * 64 / 2);
            this.Last = last;
            this.LastLevel = lastLevel;

            this.GridDimensions = new(Tiles.GetLength(0), Tiles.GetLength(1));
            }

        public void LoadAssets(ContentManager content)
        {
            position.X =
                (viewport.Width / 2) - ((Tiles.GetLength(1) - 1) * 64 / 2);
            position.Y =
                (viewport.Height / 2) - ((Tiles.GetLength(0) - 1) * 64 / 2);

            for (int i = 0; i < Tiles.GetLength(0); i++)
            {
                for (int j = 0; j < Tiles.GetLength(1); j++)
                {
                    /*if (((i == 0 && !last) || (i == Tiles.GetLength(0) - 1 && !first)) && j == Tiles.GetLength(1) / 2)
                    {
                        Tiles[i, j] = new Tile(position, GameObject.ObjectType.Door);
                        Tiles[i, j].LoadAssets(content, "tuer_offen");
                        passiveObjects.Add(Tiles[i, j]);
                    }*/
                    


                    if (Tiles[i, j] == null)
                    {
                        if (
                            (Tiles[i, j] == null) &&
                            (i == 0 ||
                            j == 0 ||
                            i == Tiles.GetLength(0) - 1 ||
                            j == Tiles.GetLength(1) - 1)
                        )
                        {
                            Tiles[i, j] = new Tile(position, GameObject.ObjectType.Wall);
                            Tiles[i, j].LoadAssets(content, "wall");
                            passiveObjects.Add(Tiles[i, j]);
                        }
                        else if (!LastLevel && Last && i == 1 && j == Tiles.GetLength(1) / 2)
                        {
                            Tiles[i, j] = new Tile(position, GameObject.ObjectType.Hole);
                            Tiles[i, j].LoadAssets(content, "hole");
                            passiveObjects.Add(Tiles[i, j]);
                        }
                        else
                        {
                            Tiles[i, j] = new Tile(position, GameObject.ObjectType.Floor);
                            Tiles[i, j].LoadAssets(content, "Grass_normal");
                        }
                    }

                    position.X += tileDimensions.X;
                }
                position.X =
                    viewport.Width / 2 - (Tiles.GetLength(1) - 1) * tileDimensions.X / 2;
                position.Y += tileDimensions.Y;
            }
            LoadEntityAssets(content);
            LoadItemAssets(content);
        }

        public void addDoor(DoorType type, ContentManager content)
        {
            position.X = 
                (viewport.Width / 2) - ((Tiles.GetLength(1) - 1) * 64 / 2);
            position.Y = 
                (viewport.Height / 2) - ((Tiles.GetLength(0) - 1) * 64 / 2);

            if (type == DoorType.Top)
            {
                position.X = position.X + tileDimensions.X * (Tiles.GetLength(1) / 2);
                Tiles[0, Tiles.GetLength(1)/2] = new Tile(position, GameObject.ObjectType.Door);
                Tiles[0, Tiles.GetLength(1)/2].LoadAssets(content, "tuer_offen");
                passiveObjects.Add(Tiles[0, Tiles.GetLength(1) / 2]);
            }
            else if(type == DoorType.Bottom)
            {
                position.X = position.X + tileDimensions.X * (Tiles.GetLength(1) / 2);
                position.Y += (Tiles.GetLength(0)-1) * tileDimensions.Y;
                Tiles[Tiles.GetLength(0) -1, Tiles.GetLength(1)/2] = new Tile(position, GameObject.ObjectType.Door);
                Tiles[Tiles.GetLength(0) - 1, Tiles.GetLength(1) / 2].LoadAssets(content, "tuer_offen");
                passiveObjects.Add(Tiles[Tiles.GetLength(0) - 1, Tiles.GetLength(1) / 2]);
            }
            else if (type == DoorType.Left)
            {
                position.Y += (Tiles.GetLength(0) / 2) * tileDimensions.Y;
                Tiles[Tiles.GetLength(0) / 2, 0] = new Tile(position, GameObject.ObjectType.Door);
                Tiles[Tiles.GetLength(0) / 2, 0].LoadAssets(content, "tuer_offen");
                passiveObjects.Add(Tiles[Tiles.GetLength(0) / 2, 0]);
            }
            else if(type == DoorType.Right)
            {
                position.Y += (Tiles.GetLength(0) / 2) * tileDimensions.Y;
                position.X += (Tiles.GetLength(1) - 1) * tileDimensions.X;
                Tiles[Tiles.GetLength(0) / 2, Tiles.GetLength(1)-1] = new Tile(position, GameObject.ObjectType.Door);
                Tiles[Tiles.GetLength(0) / 2, Tiles.GetLength(1) - 1].LoadAssets(content, "tuer_offen");
                passiveObjects.Add(Tiles[Tiles.GetLength(0) / 2, Tiles.GetLength(1) - 1]);
            }
        }

        public void LoadItemAssets(ContentManager content)
        {
            foreach (GameObject item in items) 
            {
                if (item != null)
                {
                    if(item is Sword)
                    {
                        item.LoadAssets(content, "sword");
                    }
                    else if(item is Bow)
                    {
                        item.LoadAssets(content, "bow");
                    }
                    else if(item is Spear)
                    {
                        item.LoadAssets(content, "spear");
                    }
                    else if(item is Wallet)
                    {
                        item.LoadAssets(content, "teeth");
                    }
                    else if(item is Potion potion)
                    {
                        switch(potion.Type) 
                        {
                            case Potion.PotionType.HEALING: item.LoadAssets(content, "potion"); 
                            break;
                            case Potion.PotionType.ATTACK: item.LoadAssets(content, "purple_potion"); 
                            break;
                            case Potion.PotionType.DEFENSE: item.LoadAssets(content, "green_potion"); 
                            break;
                        }
                    }
                    else if(item is Treasure)
                    {
                        item.LoadAssets(content, "treasure");
                    }
                }
            }
        }

        public void LoadEntityAssets(ContentManager content)
        {
            foreach (Entity entity in activeObjects)
            {
                if(entity is Enemy enemy)
                {
                    //if(enemy.type == Enemy.Type.ARCHER) enemy.LoadAssets(content, "archer");
                    //else if(enemy.type == Enemy.Type.MELEE) enemy.LoadAssets(content, "melee");
                    //else if(enemy.type == Enemy.Type.TANK) enemy.LoadAssets(content, "tank");
                    entity.LoadAssets(content, "enemy"); // TEST 
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

        public Tile GetTileFromPos(Vector2 Position)
        {
            int col = (int) ((Position.X - this.offset.X) / tileDimensions.X);
            int row = (int) ((Position.Y - this.offset.Y) / tileDimensions.Y);
            return Tiles[row, col];
        }

        public Tile GetTile(int row, int col)
        {
            return Tiles[row, col];
        }
        
        public void FollowPath(Enemy enemy)
        {
            if(enemy.DestinationTile != null)
            {
                Tile temp = enemy.DestinationTile;
                while(temp.Parent != null) 
                {
                    temp = temp.Parent;
                }
                enemy.Move(this, temp);
            }
        }

        public bool StartAStar(Enemy enemy) 
        {
            for(int row = 0; row < GridDimensions.X; row++)
                for(int col = 0; col < GridDimensions.Y; col++)
                {
                    Tiles[row, col].Visited = false;
                    Tiles[row, col].CurrentDistance = double.PositiveInfinity;
                    Tiles[row, col].FScore = double.PositiveInfinity;
                    Tiles[row, col].Parent = null;
                }
            
            Tile currentTile = GetTileFromPos(enemy.Position);
            currentTile.FScore = 0f;
            currentTile.CurrentDistance = CalculateHeuristic(currentTile, enemy.DestinationTile);

            Stack<Tile> openList = new Stack<Tile>();
            openList.Push(currentTile);
            
            while(openList.Count > 0 && currentTile != enemy.DestinationTile)
            {
                openList = Sort(openList);
        
                while(openList.Count > 0 && openList.Peek().Visited)
                {
                    openList.Pop();
                }
                if(openList.Count > 0) 
                {
                    break;
                }
                
                currentTile = openList.Peek();
                currentTile.Visited = true;

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
                        if(!neighbourTile.Visited && !neighbourTile.Obstacle)
                        {
                            openList.Push(neighbourTile); 
                        }
                        double FScore_2 = currentTile.FScore + CalculateHeuristic(currentTile, neighbourTile);

                        if(FScore_2 < neighbourTile.FScore)
                        {
                            neighbourTile.Parent = currentTile;
                            neighbourTile.FScore = FScore_2;
                            neighbourTile.CurrentDistance = neighbourTile.FScore + CalculateHeuristic(neighbourTile, enemy.DestinationTile);
                        }
                    }
                }
            }
            return true;
        }

        public double CalculateHeuristic(Tile tileA, Tile tileB)
        {
            return Sqrt(Pow((tileA.Position.X - tileB.Position.X), 2.0) + Pow((tileA.Position.Y - tileB.Position.Y), 2.0));
        }

        private Stack<Tile> Sort(Stack<Tile> stack)
        {
            Stack<Tile> temp = new Stack<Tile>();
            while (stack.Count > 0)
            {
                Tile element = stack.Pop();
                while (temp.Count > 0 && element.CurrentDistance > temp.Peek().CurrentDistance)
                {
                    stack.Push(temp.Pop());
                }
                temp.Push(element);
            }
            return temp;
        } 
    }   
}
