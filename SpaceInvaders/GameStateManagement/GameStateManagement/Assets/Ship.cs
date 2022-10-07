using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace ASimpleGame
{
    public class Ship
    {
        public Texture2D ShipTexture { get; set; }
        public Vector2 shipPosition;
        public float shipSpeed { get; set; }

        public Ship(Texture2D ShipTexture, Vector2 shipPosition, float shipSpeed)
        {
            this.shipSpeed = shipSpeed;
            this.ShipTexture = ShipTexture;
            this.shipPosition = shipPosition;
        }

        public void MoveShipLeft()
        {
            // Schiff nach links bewegen und verhindern, 
            // dass das Schiff den Bildschirm verlässt
            shipPosition.X -= shipSpeed;

            if (shipPosition.X < ShipTexture.Width / 2)
            {
                shipPosition.X = ShipTexture.Width / 2;
            }
        }

        public void MoveShipRight(GraphicsDeviceManager _graphics)
        {
            // Schiff nach rechts bewegen und verhindern, 
            // dass das Schiff den Bildschirm verlässt
            shipPosition.X += shipSpeed;

            if (shipPosition.X > _graphics.PreferredBackBufferWidth - ShipTexture.Width / 2)
            {
                shipPosition.X = _graphics.PreferredBackBufferWidth - ShipTexture.Width / 2;
            }

        }

        public void DrawSpaceShip(SpriteBatch _spriteBatch)
        {
            // Das Schiff mittig an den Koordinaten des Schiffes (shipPosition) zeichnen
            _spriteBatch.Draw(ShipTexture, shipPosition, null, Color.White, 0,
                new Vector2(ShipTexture.Width / 2, ShipTexture.Height / 2), 1, SpriteEffects.None, 0);
        }

    }
}
