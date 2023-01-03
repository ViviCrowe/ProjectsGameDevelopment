using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public abstract class GameObject
{
    public Texture2D texture;

    public Vector2 position;

    public BoundingBox boundingBox;

    public bool isDoor = false;

    public void Draw(SpriteBatch spriteBatch, float layerDepth)
    {
        spriteBatch
            .Draw(texture,
            position,
            null,
            Color.White,
            0f,
            new Vector2(texture.Width / 2f, texture.Height / 2f),
            1.0f,
            SpriteEffects.None,
            layerDepth);
    }

    public void LoadAssets(ContentManager content, string name)
    {
        texture = content.Load<Texture2D>(name);
    }
}
