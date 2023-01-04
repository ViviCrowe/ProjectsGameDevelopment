using System;
using Microsoft.Xna.Framework;

namespace RogueLike.Classes
{
    public class Tile : GameObject
    {

        public Tile(Vector2 tilePosition, bool isDoor)
        {
            position = tilePosition;
            this.isDoor = isDoor;
        }
    }
}
