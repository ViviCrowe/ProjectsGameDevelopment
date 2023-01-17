using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

namespace RogueLike.Classes
{
    public class Tile : GameObject
    {

        public Tile(Vector2 tilePosition, ObjectType type)
        {
            position = tilePosition;
            objectType = type;
        }
    }
}
