using Microsoft.Xna.Framework;

namespace RogueLike.Classes
{
    public class Tile : GameObject
    {
        public double fScore, currentDistance;
        public bool obstacle, visited;
        public Tile parent;
        
        public Tile(Vector2 position, TileType type)
        {
            this.position = position;
            this.tileType = type;
        }

        public Tile(Vector2 position, TileType type, double fScore)
        {
            this.position = position;
            tileType = type;
            if(tileType != GameObject.TileType.Floor) {
                this.obstacle = true;
            }
        }
    }
}
