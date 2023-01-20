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

        private String teethValue;

        private String healthValue;

        private SpriteFont font;

        private Texture2D healthbar100;
        private Texture2D healthbar87_5;
        private Texture2D healthbar75;
        private Texture2D healthbar62_5;
        private Texture2D healthbar50;
        private Texture2D healthbar37_5;
        private Texture2D healthbar25;
        private Texture2D healthbar12_5;
        private Texture2D healthbar0;

        private Texture2D xpbar100;
        private Texture2D xpbar87_5;
        private Texture2D xpbar75;
        private Texture2D xpbar62_5;
        private Texture2D xpbar50;
        private Texture2D xpbar37_5;
        private Texture2D xpbar25;
        private Texture2D xpbar12_5;
        private Texture2D xpbar0;

        private Texture2D bowTexture;
        private Texture2D swordTexture;
        private Texture2D spearTexture;
        private Texture2D fistTexture;

        private Texture2D abilityTexture;
        private Texture2D teethTexture;

        private Texture2D keyTexture;
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

            xpbar100 = content.Load<Texture2D>("xp100");
            xpbar87_5 = content.Load<Texture2D>("xp87-5");
            xpbar75 = content.Load<Texture2D>("xp75");
            xpbar62_5 = content.Load<Texture2D>("xp62-5");
            xpbar50 = content.Load<Texture2D>("xp50");
            xpbar37_5 = content.Load<Texture2D>("xp37-5");
            xpbar25 = content.Load<Texture2D>("xp25");
            xpbar12_5 = content.Load<Texture2D>("xp12-5");
            xpbar0 = content.Load<Texture2D>("xp0");


            font = content.Load<SpriteFont>("gamefont");

            bowTexture = content.Load<Texture2D>("bow");
            swordTexture = content.Load<Texture2D>("sword");
            spearTexture = content.Load<Texture2D>("spear");
            fistTexture = content.Load<Texture2D>("empty");
            
            teethTexture = Wallet.Texture;

            keyTexture = content.Load<Texture2D>("key");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(teethTexture, new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(font, teethValue, new Vector2(100, 12.5f), Color.White);
            spriteBatch.DrawString(font, healthValue, new Vector2(200, 12.5f), Color.White);
            
            if(player.HasKey)
                spriteBatch.Draw(keyTexture, new Vector2(1100, 50), null, Color.White,
                    0.0f, new Vector2(20, 20), 2.0f, SpriteEffects.None, 1.0f);


            if (player.CurrentHealth == player.MaximumHealth && 
                player.CurrentHealth >= (player.MaximumHealth / 100) * 87.5)
            {
                spriteBatch.Draw(healthbar100, new Vector2(200, -60), null, Color.White,
                    0.0f, new Vector2(20, 20), 2.0f, SpriteEffects.None, 1.0f);
            }
            else if (player.CurrentHealth <= (player.MaximumHealth / 100) * 87.5 &&
                player.CurrentHealth >= (player.MaximumHealth / 100) * 75)
            {
                spriteBatch.Draw(healthbar87_5, new Vector2(200, -60), null, Color.White,
                    0.0f, new Vector2(20, 20), 2.0f, SpriteEffects.None, 1.0f);
            }
            else if (player.CurrentHealth <= (player.MaximumHealth / 100) * 75 &&
                player.CurrentHealth >= (player.MaximumHealth / 100) * 62.5)
            {
                spriteBatch.Draw(healthbar75, new Vector2(200, -60), null, Color.White,
                    0.0f, new Vector2(20, 20), 2.0f, SpriteEffects.None, 1.0f);
            }
            else if (player.CurrentHealth <= (player.MaximumHealth / 100) * 62.5 &&
                player.CurrentHealth >= (player.MaximumHealth / 100) * 50)
            {
                spriteBatch.Draw(healthbar62_5, new Vector2(200, -60), null, Color.White,
                    0.0f, new Vector2(20, 20), 2.0f, SpriteEffects.None, 1.0f);
            }
            else if (player.CurrentHealth <= (player.MaximumHealth / 100) * 50 &&
                player.CurrentHealth >= (player.MaximumHealth / 100) * 37.5)
            {
                spriteBatch.Draw(healthbar50, new Vector2(200, -60), null, Color.White,
                    0.0f, new Vector2(20, 20), 2.0f, SpriteEffects.None, 1.0f);
            }
            else if (player.CurrentHealth <= (player.MaximumHealth / 100) * 37.5 &&
                player.CurrentHealth >= (player.MaximumHealth / 100) * 25)
            {
                spriteBatch.Draw(healthbar37_5, new Vector2(200, -60), null, Color.White,
                    0.0f, new Vector2(20, 20), 2.0f, SpriteEffects.None, 1.0f);
            }
            else if (player.CurrentHealth <= (player.MaximumHealth / 100) * 25 &&
                player.CurrentHealth >= (player.MaximumHealth / 100) * 12.5)
            {
                spriteBatch.Draw(healthbar25, new Vector2(200, -60), null, Color.White,
                    0.0f, new Vector2(20, 20), 2.0f, SpriteEffects.None, 1.0f);
            }
            else if (player.CurrentHealth <= (player.MaximumHealth / 100) * 12.5 &&
                player.CurrentHealth > 0)
            {
                spriteBatch.Draw(healthbar12_5, new Vector2(200, -60), null, Color.White,
                    0.0f, new Vector2(20, 20), 2.0f, SpriteEffects.None, 1.0f);
            }
            else if (player.CurrentHealth <= 0)
            {
                spriteBatch.Draw(healthbar0, new Vector2(200, -60), null, Color.White,
                    0.0f, new Vector2(20, 20), 2.0f, SpriteEffects.None, 1.0f);
            }


            if (player.EquippedWeapon is Bow)
                spriteBatch.Draw(bowTexture, new Vector2(450, 0), Color.White);
            else if (player.EquippedWeapon is Sword)
                spriteBatch.Draw(swordTexture, new Vector2(450, 0), Color.White);
            else if (player.EquippedWeapon is Spear)
                spriteBatch.Draw(spearTexture, new Vector2(450, 0), Color.White);
            else if (player.EquippedWeapon is Fist)
                spriteBatch.Draw(fistTexture, new Vector2(450, 0), Color.White);

            if (player.Experience == 0)
            {
                spriteBatch.Draw(xpbar0, new Vector2(550, -60), null, Color.White,
                   0.0f, new Vector2(20, 20), 2.0f, SpriteEffects.None, 0.0f);
            }
            else if (player.Experience <= (player.LevelUpAt / 100) * 12.5 &&
                player.Experience > 0)
            {
                spriteBatch.Draw(xpbar12_5, new Vector2(550, -60), null, Color.White,
                   0.0f, new Vector2(20, 20), 2.0f, SpriteEffects.None, 0.0f);
            }
            else if (player.Experience <= (player.LevelUpAt / 100) * 25 &&
                player.Experience > (player.LevelUpAt / 100) * 12.5)
            {
                spriteBatch.Draw(xpbar25, new Vector2(550, -60), null, Color.White,
                   0.0f, new Vector2(20, 20), 2.0f, SpriteEffects.None, 0.0f);
            }
            else if (player.Experience <= (player.LevelUpAt / 100) * 37.5 &&
                player.Experience > (player.LevelUpAt / 100) * 25)
            {
                spriteBatch.Draw(xpbar37_5, new Vector2(550, -60), null, Color.White,
                   0.0f, new Vector2(20, 20), 2.0f, SpriteEffects.None, 0.0f);
            }
            else if (player.Experience <= (player.LevelUpAt / 100) * 50 &&
                player.Experience > (player.LevelUpAt / 100) * 37.5)
            {
                spriteBatch.Draw(xpbar50, new Vector2(550, -60), null, Color.White,
                   0.0f, new Vector2(20, 20), 2.0f, SpriteEffects.None, 0.0f);
            }
            else if (player.Experience <= (player.LevelUpAt / 100) * 62.5 &&
                player.Experience > (player.LevelUpAt / 100) * 50)
            {
                spriteBatch.Draw(xpbar62_5, new Vector2(550, -60), null, Color.White,
                   0.0f, new Vector2(20, 20), 2.0f, SpriteEffects.None, 0.0f);
            }
            else if (player.Experience <= (player.LevelUpAt / 100) * 75 &&
                player.Experience > (player.LevelUpAt / 100) * 62.5)
            {
                spriteBatch.Draw(xpbar75, new Vector2(550, -60), null, Color.White,
                   0.0f, new Vector2(20, 20), 2.0f, SpriteEffects.None, 0.0f);
            }
            else if (player.Experience <= (player.LevelUpAt / 100) * 87.5 &&
                player.Experience > (player.LevelUpAt / 100) * 75)
            {
                spriteBatch.Draw(xpbar87_5, new Vector2(550, -60), null, Color.White,
                   0.0f, new Vector2(20, 20), 2.0f, SpriteEffects.None, 0.0f);
            }
            else if (player.Experience <= player.LevelUpAt &&
                player.Experience > (player.LevelUpAt / 100) * 87.5)
            {
                spriteBatch.Draw(xpbar100, new Vector2(550, -60), null, Color.White,
                   0.0f, new Vector2(20, 20), 2.0f, SpriteEffects.None, 0.0f);
            }

            spriteBatch.DrawString(font, player.Experience.ToString() +"/"+ player.LevelUpAt, new Vector2(800, 12.5f), Color.White);
            spriteBatch.DrawString(font, player.Level.ToString(), new Vector2(500, 12.5f), Color.White);
        }

        public void Update(Player player)
        {
            this.teethValue = player.Teeth.Value.ToString();
            this.healthValue = player.CurrentHealth + "/" + player.MaximumHealth;
        }
    }
}