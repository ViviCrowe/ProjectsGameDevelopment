using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RogueLike.Classes.Items;
using RogueLike.Classes.Weapons;

namespace RogueLike.Classes
{
    public class PlayerHUD
    {
        private Player player;

        private Weapon weaponSlot;

        private String teethValue;

        private String healthValue;
        private String levelValue;

        private SpriteFont font;

        private Texture2D healthbar;
        private Texture2D healthbar100;
        private Texture2D healthbar87_5;
        private Texture2D healthbar75;
        private Texture2D healthbar62_5;
        private Texture2D healthbar50;
        private Texture2D healthbar37_5;
        private Texture2D healthbar25;
        private Texture2D healthbar12_5;
        private Texture2D healthbar0;

        private Texture2D weaponTexture;
        private Texture2D bowTexture;
        private Texture2D swordTexture;
        private Texture2D spearTexture;
        private Texture2D fistTexture;

        private Texture2D abilityTexture;
        private Texture2D teethTexture;

        public PlayerHUD(Player player)
        {
            this.player = player;
            this.Update(player);
        }

        public void LoadContent(ContentManager content)
        {

            healthbar100 = content.Load<Texture2D>("100");
            healthbar87_5 = content.Load<Texture2D>("87-5");
            healthbar75 = content.Load<Texture2D>("75");
            healthbar62_5 = content.Load<Texture2D>("62-5");
            healthbar50 = content.Load<Texture2D>("50");
            healthbar37_5 = content.Load<Texture2D>("37-5");
            healthbar25 = content.Load<Texture2D>("25");
            healthbar12_5 = content.Load<Texture2D>("12-5");
            healthbar0 = content.Load<Texture2D>("0");

            font = content.Load<SpriteFont>("gamefont");

            bowTexture = content.Load<Texture2D>("bow");
            swordTexture = content.Load<Texture2D>("sword");
            spearTexture = content.Load<Texture2D>("spear");
            fistTexture = content.Load<Texture2D>("empty");
            

            teethTexture = Wallet.Texture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font,teethValue,new Vector2(100,12.5f),Color.White);
            spriteBatch.DrawString(font, healthValue, new Vector2(200, 12.5f), Color.White);


            if(player.CurrentHealth == player.MaximumHealth)
            {
                spriteBatch.Draw(healthbar100, new Vector2(200, -60), null, Color.White, 0.0f, new Vector2(20, 20), 2.0f, SpriteEffects.None, 1.0f);
            }
            else if (player.CurrentHealth <= (player.MaximumHealth / 100) * 87.5 &&
                player.CurrentHealth >= (player.MaximumHealth / 100) * 75)
            {
                spriteBatch.Draw(healthbar87_5, new Vector2(200, -60), null, Color.White, 0.0f, new Vector2(20, 20), 2.0f, SpriteEffects.None, 0.0f);
            }
            else if (player.CurrentHealth <= (player.MaximumHealth / 100) * 75 &&
                player.CurrentHealth >= (player.MaximumHealth / 100) * 62.5)
            {
                spriteBatch.Draw(healthbar75, new Vector2(200, -60), null, Color.White, 0.0f, new Vector2(20, 20), 2.0f, SpriteEffects.None, 0.0f);
            }
            else if (player.CurrentHealth <= (player.MaximumHealth / 100) * 62.5 &&
                player.CurrentHealth >= (player.MaximumHealth / 100) * 50)
            {
                spriteBatch.Draw(healthbar62_5, new Vector2(200, -60), null, Color.White, 0.0f, new Vector2(20, 20), 2.0f, SpriteEffects.None, 0.0f);
            }
            else if (player.CurrentHealth <= (player.MaximumHealth / 100) * 50 &&
                player.CurrentHealth >= (player.MaximumHealth / 100) * 37.5)
            {
                spriteBatch.Draw(healthbar50, new Vector2(200, -60), null, Color.White, 0.0f, new Vector2(20, 20), 2.0f, SpriteEffects.None, 0.0f);
            }
            else if (player.CurrentHealth <= (player.MaximumHealth / 100) * 37.5 &&
                player.CurrentHealth >= (player.MaximumHealth / 100) * 25)
            {
                spriteBatch.Draw(healthbar37_5, new Vector2(200, -60), null, Color.White, 0.0f, new Vector2(20, 20), 2.0f, SpriteEffects.None, 0.0f);
            }
            else if (player.CurrentHealth <= (player.MaximumHealth / 100) * 25 &&
                player.CurrentHealth >= (player.MaximumHealth / 100) * 12.5)
            {
                spriteBatch.Draw(healthbar25, new Vector2(200, -60), null, Color.White, 0.0f, new Vector2(20, 20), 2.0f, SpriteEffects.None, 0.0f);
            }
            else if (player.CurrentHealth <= (player.MaximumHealth / 100) * 12.5 &&
                player.CurrentHealth > 0)
            {
                spriteBatch.Draw(healthbar12_5, new Vector2(200, -60), null, Color.White, 0.0f, new Vector2(20, 20), 2.0f, SpriteEffects.None, 0.0f);
            }
            else if (player.CurrentHealth <= 0)
            {
                spriteBatch.Draw(healthbar0, new Vector2(200, -60), null, Color.White, 0.0f, new Vector2(20, 20), 2.0f, SpriteEffects.None, 0.0f);
            }


            if (player.EquippedWeapon is Bow)
                spriteBatch.Draw(bowTexture, new Vector2(400, 0), Color.White);
            else if(player.EquippedWeapon is Sword)
                spriteBatch.Draw(swordTexture, new Vector2(400, 0), Color.White);
            else if(player.EquippedWeapon is Spear)
                spriteBatch.Draw(spearTexture, new Vector2(400, 0), Color.White);
            else if(player.EquippedWeapon is Fist)
                spriteBatch.Draw(fistTexture, new Vector2(400, 0), Color.White);


            if (abilityTexture != null)
                spriteBatch.Draw(abilityTexture,new Vector2(600,0),Color.White);
        }

        public void Update(Player player)
        {
            this.teethValue = player.Teeth.Value.ToString();
            this.healthValue = player.CurrentHealth + "/" + player.MaximumHealth;
            this.weaponSlot = player.EquippedWeapon;
            this.levelValue = player.Level.ToString();
        }
    }
}
