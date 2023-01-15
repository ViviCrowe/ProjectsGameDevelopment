using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RogueLike.Classes.Abilities;
using RogueLike.Classes.Items;
using RogueLike.Classes.Weapons;

namespace RogueLike.Classes
{
    public class PlayerHUD
    {
        private Player player;

        private Weapon weaponSlot;

        private AktivAbility aktivAbility;

        private String teethValue;

        private String healthValue;
        private String levelValue;

        private SpriteFont font;

        private Texture2D weaponTexture;

        private Texture2D abilityTexture;
        private Texture2D teethTexture;

        public PlayerHUD(Player player)
        {
            this.player = player;
            this.Update(player);
        }

        public void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("gamefont");
            if (player.EquippedWeapon != null)
            {
                weaponTexture = weaponSlot.Texture;
            }
            if (player.aktivAbility != null)
            {
                abilityTexture = aktivAbility.abilityTexture;
            }
            teethTexture = Wallet.Texture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(teethTexture, new Vector2(60, 14f), Color.White);
            spriteBatch.DrawString(font, teethValue, new Vector2(100, 14f), Color.White);
            spriteBatch.DrawString(font, "Health    " + healthValue, new Vector2(200, 14f), Color.White);
            spriteBatch.DrawString(font, "Level    " + levelValue, new Vector2(600, 14f), Color.White);
            if (weaponTexture != null)
                spriteBatch.Draw(weaponTexture, new Vector2(700, 0), Color.White);
            if (abilityTexture != null)
                spriteBatch.Draw(abilityTexture, new Vector2(900, 0), Color.White);
        }

        public void Update(Player player)
        {
            this.teethValue = player.Teeth.Value.ToString();
            this.healthValue = player.CurrentHealth + "/" + player.MaximumHealth;
            this.weaponSlot = player.EquippedWeapon;
            this.aktivAbility = player.aktivAbility;
            this.levelValue = player.Level.ToString();
        }
    }
}
