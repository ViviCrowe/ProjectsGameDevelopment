using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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

        public override void LoadAssets(ContentManager content) 
        {
            throw new NotSupportedException();
        }

        public void LoadAssets(ContentManager content, string name) 
        {
            texture = content.Load<Texture2D>(name);
        }
    }
}
