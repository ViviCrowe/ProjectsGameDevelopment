using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RogueLike.Classes.Abilities;
using SharpDX.Direct2D1;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

namespace RogueLike.Classes
{
    public class PlayerHUD
    {
        private Weapon weaponSlot;
        private AktivAbility aktivAbility;
        private Player player;

        private String teethValue;
        private String healthValue;

        private SpriteFont font;

        public PlayerHUD(Player player)
        {
            this.player = player;

            teethValue =  player.teethValue.ToString();
            healthValue = player.minimumHealth + "/" + player.maximumHealth;

        }
        public void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("gamefont");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font,teethValue,new Vector2(100,100),Color.Black);
            spriteBatch.DrawString(font, healthValue, new Vector2(300, 100), Color.Black);
        }
    }
}
