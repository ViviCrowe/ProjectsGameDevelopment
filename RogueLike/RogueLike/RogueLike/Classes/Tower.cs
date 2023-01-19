using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueLike.Classes
{
    public class Tower : GameObject
    {
        static int frame = 0;
        static int counter = 0;
        public Tower(Vector2 position)
        {
            this.Position = position;
            Position.Y -= 500;
        }

    public new void Draw(SpriteBatch spriteBatch, float layerDepth)
    {
        spriteBatch.Draw(Texture, Position, new Rectangle(frame*100, 0, 100, 140), Color.White, 0f, new Vector2(Texture.Width / 22f, 
                Texture.Height / 2f), 2.0f, SpriteEffects.None, layerDepth);
    }

    public BoundingBox GetBoundingBox()
    {
        return new BoundingBox(new Vector3(Position.X-Texture.Width/11, Position.Y-Texture.Height/2, 0), 
            new Vector3(Position.X+Texture.Width/22, Position.Y+Texture.Height/2, 0));
    }

    public static void Update()
    {
        if(counter == 4)
        {
            frame = (frame+1) % 11;
            counter = 0;
        }
        else
        {
            counter++;
        }
    }
}
}