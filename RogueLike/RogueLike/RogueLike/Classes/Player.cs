using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RogueLike.Classes
{
    public class Player : Entity
    {
        public Player(Viewport viewport) : base(viewport)
        {
            position = new Vector2(viewport.Width/2, viewport.Height/2);
        }

        public override void LoadAssets(ContentManager content)
        {
            texture = content.Load<Texture2D>("character");
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0,
                new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 0);
        }
    }
}
