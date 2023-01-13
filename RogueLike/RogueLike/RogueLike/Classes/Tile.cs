using Microsoft.Xna.Framework;

namespace RogueLike.Classes
{
    public class Tile : GameObject
    {
        public double fScore, currentDistance;
        public bool obstacle, visited;
        public Tile parent;
        
        public Tile(Vector2 position, ObjectType type)
        {
            this.position = position;
            this.objectType = type;
        }

        public Tile(Vector2 position, ObjectType type, double fScore)
        {
            this.position = position;
            objectType = type;
            if(objectType != GameObject.ObjectType.Floor) {
                this.obstacle = true;
            }
        }
    }
}
