using Microsoft.Xna.Framework;

namespace RogueLike.Classes
{
    public class Tile : GameObject
    {
        public double FScore { get; set; }
        public double CurrentDistance { get; set; }

        public bool Obstacle { get; set; }
        public bool Visited { get; set; }

        public Tile Parent { get; set; }

        public Tile(Vector2 Position, ObjectType type)
        {
            this.Position = Position;
            this.ObjType = type;
        }

        public Tile(Vector2 Position, ObjectType type, double fScore)
        {
            this.Position = Position;
            ObjType = type;
            if (ObjType != GameObject.ObjectType.Floor)
            {
                this.Obstacle = true;
            }
        }
    }
}
