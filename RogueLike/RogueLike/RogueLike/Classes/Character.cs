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
    public class Character
    {
        public Texture2D CharacterTexture { get; set; }
        public Vector2 CharacterPosition;

        public Character()
        {
            CharacterPosition = new Vector2(1920f/2, 1080f/2);
        }

        public void LoadCharacterStartAssets(ContentManager content)
        {
            CharacterTexture = content.Load<Texture2D>("character");
        }

        public void DrawCharacter(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(CharacterTexture, CharacterPosition, null, Color.White, 0,
                new Vector2(CharacterTexture.Width / 2, CharacterTexture.Height / 2), 0.3f, SpriteEffects.None, 0);
        }

        public void MoveLeft()
        {
            CharacterPosition.X -= 10;
        }

        public void MoveRight()
        {
            CharacterPosition.X += 10;
        }

        public void MoveUp()
        {
            CharacterPosition.Y -= 10;
        }

        public void MoveDown()
        {
            CharacterPosition.Y += 10;
        }
    }
}
