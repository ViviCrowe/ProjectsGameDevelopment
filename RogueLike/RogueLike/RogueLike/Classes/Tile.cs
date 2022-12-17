using System;
using Microsoft.Xna.Framework;

namespace RogueLike.Classes
{
    public class Tile : GameObject
    {
        public bool isWall;

        public Tile(Vector2 tilePosition, bool isWall)
        {
            position = tilePosition;
            this.isWall = isWall;
        }
    }
}
