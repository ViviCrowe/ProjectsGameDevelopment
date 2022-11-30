using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLike.Classes
{
    public class Tile
    {
        public Texture2D TileTexture { get; set; }
        public Vector2 TilePosition;
        public bool isWall;

        public Tile(Vector2 tilePosition, bool isWall)
        {
            TilePosition = tilePosition;
            this.isWall = isWall;
        }

        public void LoadTileAssets(ContentManager content, string name) 
        {
            TileTexture = content.Load<Texture2D>(name);
        }

        public void DrawTile(SpriteBatch spriteBatch)
        {
            if(TileTexture.Width != 64)
            {
                spriteBatch.Draw(TileTexture, TilePosition, null, Color.White, 0,
                new Vector2(TileTexture.Width / 2, TileTexture.Height / 2), 0.1f, SpriteEffects.None, 1);
            }
            else
            {
                spriteBatch.Draw(TileTexture, TilePosition, null, Color.White, 0,
                new Vector2(TileTexture.Width / 2, TileTexture.Height / 2), 1, SpriteEffects.None, 1);
            }
            
        }
    }
}
