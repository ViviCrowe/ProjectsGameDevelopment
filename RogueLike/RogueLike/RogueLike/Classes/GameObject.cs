using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RogueLike.Classes
{
    public abstract class GameObject
    {
        public Texture2D Texture { get; set; }

        public Vector2 Position;

        public BoundingBox BoundingBox { get; set; }

        public ObjectType ObjType { get; set; }

        public enum ObjectType
        {
            Entity,
            Floor,
            Hole,
            Wall,
            Door
        }

        public void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, 0f, new Vector2(Texture.Width / 2f, 
                Texture.Height / 2f), 1.0f, SpriteEffects.None, layerDepth);
        }

        public void LoadAssets(ContentManager content, string name)
        {
            Texture = content.Load<Texture2D>(name);
        }

        public BoundingBox CreateBoundingBox()
        {
            int playerFactor = 0;
            if (this is Player) playerFactor = 20;
            return new BoundingBox(new Vector3(
                this.Position.X - (this.Texture.Width / 2) - playerFactor,
                    this.Position.Y - (this.Texture.Height / 2) - playerFactor,
                    0),
                new Vector3(this.Position.X + (this.Texture.Width / 2) + playerFactor,
                    this.Position.Y + (this.Texture.Height / 2) + playerFactor,
                    0));
        }
    }
}
