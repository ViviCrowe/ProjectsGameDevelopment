using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RogueLike.Classes.Abilities;
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

        private SpriteFont font;
        private Texture2D weaponTexture;
        private Texture2D abilityTexture;

        public PlayerHUD(Player player)
        {
            this.player = player;
            
            teethValue =  player.teeth.value.ToString();
            healthValue = player.currentHealth + "/" + player.maximumHealth;
            weaponSlot = player.weapon;
            aktivAbility = player.aktivAbility;
        }

        public void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("gamefont");
            if(player.weapon != null) {
                weaponTexture = weaponSlot.texture;
            }
            if(player.aktivAbility != null) {
                abilityTexture = aktivAbility.abilityTexture;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font,teethValue,new Vector2(100,12.5f),Color.White);
            spriteBatch.DrawString(font, healthValue, new Vector2(200, 12.5f), Color.White);
            if(weaponTexture != null)
                spriteBatch.Draw(weaponTexture, new Vector2(400,0), Color.White);
            if(abilityTexture != null)
                spriteBatch.Draw(abilityTexture,new Vector2(600,0),Color.White);
        }

        public void Update(Player player)
        {
            if(this.player.maximumHealth != player.maximumHealth)
            {
                this.player.maximumHealth = player.maximumHealth;
                healthValue = player.currentHealth + "/" + player.maximumHealth;
            }

            if(this.player.currentHealth != player.currentHealth)
            {
                this.player.currentHealth = player.currentHealth;
                healthValue = player.currentHealth + "/" + player.maximumHealth;
            }

            if(this.player.weapon != player.weapon)
            {
                this.player.weapon = player.weapon;
            }

            if(this.player.aktivAbility != player.aktivAbility)
            {
                this.player.aktivAbility =player.aktivAbility;
            }

            if(this.teethValue != player.teeth.value.ToString())
            {
                this.teethValue = player.teeth.value.ToString();
            }

            if (this.player != player)
            {
                this.player = player;
            }
        }
    }
}
