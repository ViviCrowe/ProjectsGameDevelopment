using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public abstract class GameObject {
    public Texture2D texture;
    public Vector2 position;
    public BoundingBox boundingBox;    
                
    public void Draw(SpriteBatch spriteBatch) {
        spriteBatch.Draw(texture, position, null, Color.White, 0f, new Vector2(texture.Width/2f, texture.Height/2f),
         1.0f, SpriteEffects.None, 1.0f);
    }
    public abstract void LoadAssets(ContentManager Content);
}
