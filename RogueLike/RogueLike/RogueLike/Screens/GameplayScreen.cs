#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#endregion File Description

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueLike.Classes;
using RogueLike.Classes.Weapons;
using RogueLike.Classes.Abilities;
using System;
using System.Threading;
using System.Xml.Linq;


#endregion Using Statements


namespace GameStateManagement
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    internal class GameplayScreen : GameScreen
    {
#region Fields

        private ContentManager content;

        private SpriteFont gameFont;

        private float pauseAlpha;

        private KeyboardState previousKeyboardState;

        private Player player;

        private Enemy enemy; // STATISCHER TEST GEGNER
        
        private Weapon weapon; // TEST WAFFE

        private Room room;

        private PlayerHUD playerHUD;

#endregion Fields



#region Initialization
        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {

            if (content == null)
                content =
                    new ContentManager(ScreenManager.Game.Services, "Content");


            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;

            gameFont = content.Load<SpriteFont>("gamefont");

            //Initialising and Generating Atributes of Player
            player = new Player(viewport, new Sword(content.Load<Texture2D>("sword")));
            player.aktivAbility = new AktivAbility();
            player.aktivAbility.abilityTexture = content.Load<Texture2D>("enemy");
            player.LoadAssets(content, "character");

            //player.LoadAssets(content, "character_with_sword"); // TEST
            enemy = new Enemy(viewport, new PatrolAI(), new Sword()); // TEST  

            weapon = new Sword();   
            weapon.position.X = viewport.Width/2 + 60; //TEST
            weapon.position.Y = viewport.Height/2 - 60; //TEST  

            room = new Room(viewport);
            room.activeObjects.Add(enemy); // TEST
            room.items.Add(weapon);
            room.LoadAssets(content);

            enemy.LoadAssets(content, "enemy");

            //playerHUD initiolaztion
            playerHUD = new PlayerHUD(player);
            playerHUD.LoadContent(content);

            // simulate longer loading time
            // Thread.Sleep(1000);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }

        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


#endregion Initialization



#region Update and Draw
        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(
            GameTime gameTime,
            bool otherScreenHasFocus,
            bool coveredByOtherScreen
        )
        {
            base.Update(gameTime, otherScreenHasFocus, false);
            //Updates the HUD if Reference is different
            playerHUD.Update(player);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
            {
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            }
            else
            {
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);
            }

            if (IsActive)
            {
                enemy.Update(room, content);
            }
        }

        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            // Look up inputs for the active player profile.
            int playerIndex = (int) ControllingPlayer.Value;

            KeyboardState keyboardState =
                input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected =
                !gamePadState.IsConnected &&
                input.GamePadWasConnected[playerIndex];

            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                ScreenManager
                    .AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                if (keyboardState.IsKeyDown(Keys.A))
                {
                    if (CheckForCollision(-1, 0, false) == null)
                        player.MoveLeft();
                }

                if (keyboardState.IsKeyDown(Keys.D))
                {
                    if (CheckForCollision(1, 0, false) == null)
                        player.MoveRight();
                }

                if (keyboardState.IsKeyDown(Keys.W))
                {
                    if (CheckForCollision(0, -1, false) == null)
                        player.MoveUp();
                }

                if (keyboardState.IsKeyDown(Keys.S))
                {
                    if (CheckForCollision(0, 1, false) == null)
                        player.MoveDown();
                }

                if (keyboardState.IsKeyDown(Keys.J))
                {
                    player.DropWeapon (room, content);
                }

                if (keyboardState.IsKeyDown(Keys.K))
                {
                    player.PickUpItem(CheckForItemCollision(), room, content);
                }

                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    Entity targetEntity =
                        (Entity) CheckForCollision(0, 0, true);
                    if (targetEntity != null)
                    {
                        player.Attack (targetEntity);
                        // TODO: display damage taken
                    }
                }
            }
            previousKeyboardState = keyboardState;
        }

        public bool IsNewKeyPressed(Keys key, KeyboardState keyboardState)
        {
            return keyboardState.IsKeyDown(key) &&
            !previousKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager
                .GraphicsDevice
                .Clear(ClearOptions.Target, Color.Black, 0, 0);

            // Our player and enemy are both actually just text strings.
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin(SpriteSortMode.BackToFront);

                player.Draw(spriteBatch, 0.2f);
                room.Draw(spriteBatch);
                enemy.Draw(spriteBatch, 2.0f);
                playerHUD.Draw(spriteBatch);

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha =
                    MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack (alpha);
            }
        }


#endregion Update and Draw


        private GameObject
        CheckForCollision(int factorX, int factorY, bool attack)
        {
            BoundingBox boundingBox_1 =
                CreateBoundingBox(player, factorX, factorY);

            foreach (GameObject obj in room.passiveObjects)
            {
                BoundingBox boundingBox_2 = CreateBoundingBox(obj);

                if (boundingBox_1.Intersects(boundingBox_2))
                {
                    return obj;
                }
            }

            if (room.activeObjects.Count > 0)
            {
                foreach (Entity entity_2 in room.activeObjects)
                {
                    if (player != entity_2)
                    {
                        BoundingBox boundingBox_2 = CreateBoundingBox(entity_2);

                        if (attack && player.weapon != null)
                        {
                            boundingBox_1.Min.X -= player.weapon.weaponRange;
                            boundingBox_1.Min.Y -= player.weapon.weaponRange;
                            boundingBox_1.Max.X += player.weapon.weaponRange;
                            boundingBox_1.Max.Y += player.weapon.weaponRange;
                        }
                        if (boundingBox_1.Intersects(boundingBox_2))
                        {
                            return entity_2;
                        }
                    }
                }
            }
            return null;
        }

        private GameObject CheckForItemCollision()
        {
            BoundingBox boundingBox_1 = CreateBoundingBox(player);

            foreach (GameObject item in room.items)
            {
                BoundingBox boundingBox_2 = CreateBoundingBox(item);

                if (boundingBox_1.Intersects(boundingBox_2))
                {
                    return item;
                }
            }
            return null;
        }

        private BoundingBox
        CreateBoundingBox(Entity entity, int factorX, int factorY)
        {
            return new BoundingBox(new Vector3(entity.position.X +
                    entity.movementSpeed * factorX -
                    (entity.texture.Width / 2) +
                    10,
                    entity.position.Y +
                    entity.movementSpeed * factorY -
                    (entity.texture.Height / 2),
                    0),
                new Vector3(entity.position.X +
                    entity.movementSpeed * factorX +
                    (entity.texture.Width / 2) -
                    10,
                    entity.position.Y +
                    entity.movementSpeed * factorY +
                    (entity.texture.Height / 2) -
                    5,
                    0));
        }

        private BoundingBox CreateBoundingBox(GameObject obj)
        {
            return new BoundingBox(new Vector3(obj.position.X -
                    (obj.texture.Width / 2),
                    obj.position.Y - (obj.texture.Height / 2),
                    0),
                new Vector3(obj.position.X + (obj.texture.Width / 2),
                    obj.position.Y + (obj.texture.Height / 2),
                    0));
        }
    }
}
