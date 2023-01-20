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
using RogueLike.Classes.Items;
using System;
using Microsoft.Xna.Framework.Media;


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

        private Level[] level;

        private Level currentLevel;

        private Room currentRoom;

        private int roomCounterRow, roomCounterCol;

        private int levelCounter;

        private Viewport viewport;

        private PlayerHUD playerHUD;
        private Song backgroundMusic;
        private Song bossMusic;
        private Song currentSong;

        // for cheating
        private int tempMaxHealth = -1;
        private float tempSpeed = -1;
        private int tempAttack = -1;
        private int cheatCountdown = 0;

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

            viewport = ScreenManager.GraphicsDevice.Viewport;

            Wallet.Texture = content.Load<Texture2D>("teeth");
            gameFont = content.Load<SpriteFont>("gamefont");
            backgroundMusic = currentSong = content.Load<Song>("background_music");
            bossMusic = content.Load<Song>("boss_music");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 1.5f;
            MediaPlayer.Play(currentSong);

            //Initialising and Generating Attributes of Player
            player = new Player(viewport, new Fist());
            player.LoadAssets(content);

            Entity.LoadAssets(content);
            Bow.LoadAssets(content);

            level = new Level[3];
            level[0] = new Level(viewport, 7, false);
            level[1] = new Level(viewport, 7, false);
            level[2] = new Level(viewport, 8, true);

            levelCounter = 0;
            NewLevel();


            //playerHUD initialization
            playerHUD = new PlayerHUD(player);
            playerHUD.LoadContent(content);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }

        private void NewLevel()
        {
            currentLevel = level[levelCounter];
            currentLevel.GenerateLevel(content);
            currentLevel.AddDoors(content);
            foreach (Room room in currentLevel.Rooms)
            {
                if (room != null)
                {
                    room.activeObjects.Add(player);
                    room.LoadAssets(content);
                }
            }

            roomCounterRow = 3;
            roomCounterCol = 1;
            currentRoom = currentLevel.Rooms[roomCounterRow, roomCounterCol];
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
            //Return back to title screen if player is dead
            if(player.CurrentHealth <= 0)
            {
                LoadingScreen.Load(ScreenManager, false, null, 
                    new BackgroundScreen(), new MainMenuScreen());
            }

            foreach (Entity entity in currentRoom.activeObjects)
            {
                if(entity is Boss)
                {
                    Boss boss = (Boss)entity;
                    if(boss.Level == 3 && boss.CurrentHealth <= 0)
                    {
                        LoadingScreen.Load(ScreenManager, false, null, 
                            new BackgroundScreen(), new MainMenuScreen());
                    } 
                }
            }

            base.Update(gameTime, otherScreenHasFocus, false);

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
                foreach(Entity e in currentRoom.activeObjects.ToArray()) 
                {
                    if(e is Boss boss) 
                    {
                        boss.Update(player, currentRoom, content);
                    }
                    else if(e is Enemy enemy)
                    {
                        enemy.Update(player, currentRoom, content);
                    }
                    else if(e is Player player)
                    {
                        player.Update(currentRoom);
                    }
                }

                //Updates the HUD if Reference is different
                playerHUD.Update(player);
                
                GameObject item = player.CheckForItemCollision(currentRoom);
                if(item is Wallet || item is Potion)
                {
                    player.PickUpItem(item, currentRoom, content);
                }
            }

            if (player.CheckForCollision(currentRoom, 0, -1, false, false) != null && player.CheckForCollision(currentRoom, 0, -1, false, false).ObjType == GameObject.ObjectType.Door)
            {
                player.Position.Y = viewport.Height / 2;

                roomCounterRow--;
                currentRoom = currentLevel.Rooms[roomCounterRow, roomCounterCol];

                player.Position.Y = currentRoom.Tiles[currentRoom.Tiles.GetLength(0) - 1, currentRoom.Tiles.GetLength(0) / 2].Position.Y - 85;
            }
            else if (player.CheckForCollision(currentRoom, 0, 1, false, false) != null && player.CheckForCollision(currentRoom, 0, 1, false, false).ObjType == GameObject.ObjectType.Door)
            {
                player.Position.Y = viewport.Height / 2;

                roomCounterRow++;
                currentRoom = currentLevel.Rooms[roomCounterRow, roomCounterCol];

                player.Position.Y = currentRoom.Tiles[0, currentRoom.Tiles.GetLength(0) / 2].Position.Y + 85;

            }
            else if (player.CheckForCollision(currentRoom, -1, 0, false, false) != null && player.CheckForCollision(currentRoom, -1, 0, false, false).ObjType == GameObject.ObjectType.Door)
            {
                player.Position.Y = viewport.Height / 2;

                roomCounterCol--;
                currentRoom = currentLevel.Rooms[roomCounterRow, roomCounterCol];

                player.Position.X = currentRoom.Tiles[currentRoom.Tiles.GetLength(0) / 2, currentRoom.Tiles.GetLength(1) -1].Position.X - 85;

            }
            else if (player.CheckForCollision(currentRoom, 1, 0, false, false) != null && player.CheckForCollision(currentRoom, 1, 0, false, false).ObjType == GameObject.ObjectType.Door)
            {
                player.Position.Y = viewport.Height / 2;

                roomCounterCol++;
                currentRoom = currentLevel.Rooms[roomCounterRow, roomCounterCol];

                player.Position.X = currentRoom.Tiles[currentRoom.Tiles.GetLength(0) / 2, 0].Position.X + 85;

            }
            else if (checkTrapDoor(player))
            {
                player.Position.X = viewport.Width / 2;
                player.Position.Y = viewport.Height / 2;
                levelCounter++;
                NewLevel();
            }
            else if (player.HasKey)
            {
                GameObject temp = player.CheckForCollision(currentRoom, 0, -1, false, false);
                if(temp != null && temp.ObjType == GameObject.ObjectType.LockedDoor)
                {
                    temp.ObjType =  GameObject.ObjectType.Door;
                    temp.LoadAssets(content, "tuer_offen2");
                    player.HasKey = false;
                }
            }

            //Updates the HUD if Reference is different
            playerHUD.Update(player);

            if(currentRoom.Last && currentRoom.LastLevel && currentSong != bossMusic)
            {
                currentSong = bossMusic;
                MediaPlayer.Play(currentSong);
            }
            else if(!(currentRoom.Last && currentRoom.LastLevel) && currentSong == bossMusic)
            {
                currentSong = backgroundMusic;
                MediaPlayer.Play(currentSong);
            }

            Tower.Update();
            if(cheatCountdown > 0) cheatCountdown--;

        }

        private bool checkTrapDoor(Entity entity)
        {
            if (player.CheckForCollision(currentRoom, -1, 0, false, false) != null && (player.CheckForCollision(currentRoom, -1, 0, false, false).ObjType == GameObject.ObjectType.Hole))
            {
                return true;
            }
            else if (player.CheckForCollision(currentRoom, 1, 0, false, false) != null && (player.CheckForCollision(currentRoom, 1, 0, false, false).ObjType == GameObject.ObjectType.Hole))
            {
                return true;
            }
            else if (player.CheckForCollision(currentRoom, 0, -1, false, false) != null && (player.CheckForCollision(currentRoom, 0, -1, false, false).ObjType == GameObject.ObjectType.Hole))
            {
                return true;
            }
            else return false;
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
                if (keyboardState.IsKeyDown(Keys.A) && !keyboardState.IsKeyDown(Keys.W) && !keyboardState.IsKeyDown(Keys.S))
                {
                    if (player.CheckForCollision(currentRoom, -1, 0, false, false) == null)
                        player.MoveLeft();
                }

                if (keyboardState.IsKeyDown(Keys.D) && !keyboardState.IsKeyDown(Keys.S) && !keyboardState.IsKeyDown(Keys.W))
                {
                    if (player.CheckForCollision(currentRoom, 1, 0, false, false) == null)
                        player.MoveRight();
                }

                if (keyboardState.IsKeyDown(Keys.W) && !keyboardState.IsKeyDown(Keys.A) && !keyboardState.IsKeyDown(Keys.D))
                {
                    if (player.CheckForCollision(currentRoom, 0, -1, false, false) == null)
                        player.MoveUp();
                }

                if (keyboardState.IsKeyDown(Keys.S) && !keyboardState.IsKeyDown(Keys.A) && !keyboardState.IsKeyDown(Keys.D))
                {
                    if (player.CheckForCollision(currentRoom, 0, 1, false, false) == null)
                        player.MoveDown();
                }

                if(keyboardState.IsKeyDown(Keys.W) && keyboardState.IsKeyDown(Keys.A))
                {
                    if (player.CheckForCollision(currentRoom, -1, -1, false, false) == null)
                        player.MoveUpLeft();     
                }
                if(keyboardState.IsKeyDown(Keys.W) && keyboardState.IsKeyDown(Keys.D))
                {
                    if (player.CheckForCollision(currentRoom, 1, -1, false, false) == null)
                        player.MoveUpRight();     
                }
                if(keyboardState.IsKeyDown(Keys.S) && keyboardState.IsKeyDown(Keys.A))
                {
                    if (player.CheckForCollision(currentRoom, -1, 1, false, false) == null)
                        player.MoveDownLeft();      
                }
                if(keyboardState.IsKeyDown(Keys.S) && keyboardState.IsKeyDown(Keys.D))
                {
                    if (player.CheckForCollision(currentRoom, 1, 1, false, false) == null)
                        player.MoveDownRight();     
                }
                if (keyboardState.IsKeyDown(Keys.J))
                {
                    player.DropWeapon (currentRoom, content);
                }

                if (keyboardState.IsKeyDown(Keys.K))
                {
                    player.PickUpItem(player.CheckForItemCollision(currentRoom), currentRoom, content);
                }

                if (keyboardState.IsKeyDown(Keys.Space))
                {
                    if(player.EquippedWeapon is Bow bow) 
                    {
                        bow.FireArrow(player);
                    }  
                    else
                    {
                         Entity targetEntity =
                            (Entity) player.CheckForCollision(currentRoom, 0, 0, true, false);
                        if (targetEntity != null)
                        {
                            int damageDealt = player.Attack (targetEntity);
                            // TODO: display damage dealt
                        }
                    }
                }

                // cheat
                if(keyboardState.IsKeyDown(Keys.Enter) && cheatCountdown == 0)
                {
                    cheatCountdown = 30;
                    if(player.MaximumHealth < 99999)
                    {
                        tempMaxHealth = player.MaximumHealth;
                        tempSpeed = player.MovementSpeed;
                        tempAttack = player.BaseAttack;
    
                        player.MaximumHealth = player.CurrentHealth = 99999;
                        player.MovementSpeed = 7f;
                        player.BaseAttack = 1000;
                    }
                    else if(player.MaximumHealth == 99999 && tempMaxHealth > -1)
                    {
                        player.MaximumHealth = player.CurrentHealth = tempMaxHealth;
                        player.MovementSpeed = tempSpeed;
                        player.BaseAttack = tempAttack;
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
            ScreenManager
                .GraphicsDevice
                .Clear(ClearOptions.Target, Color.Black, 0, 0);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin(SpriteSortMode.BackToFront);

            player.Draw(spriteBatch, 0.2f);
            currentRoom.Draw(spriteBatch);
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

    }
}