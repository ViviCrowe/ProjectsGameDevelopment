using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RogueLike.Classes.Abilities;
using RogueLike.Classes.Weapons;
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
        private Texture2D weaponTexture;
        private Texture2D abilityTexture;

        public PlayerHUD(Player player)
        {
            this.player = player;


            teethValue =  player.teethValue.ToString();
            healthValue = player.minimumHealth + "/" + player.maximumHealth;
            weaponSlot = player.weapon;
            aktivAbility = player.aktivAbility;
        }
        public void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("gamefont");
            weaponTexture = content.Load<Texture2D>("sword");
            abilityTexture = content.Load<Texture2D>("enemy");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font,teethValue,new Vector2(100,0),Color.Black);
            spriteBatch.DrawString(font, healthValue, new Vector2(300, 0), Color.Black);
            spriteBatch.Draw(weaponTexture,new Vector2(500,0),Color.Black);
            spriteBatch.Draw(abilityTexture,new Vector2(700,0),Color.Black);
        }
    }
}
