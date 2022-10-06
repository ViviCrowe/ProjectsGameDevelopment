using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace ASimpleGame
{
    public class Game1 : Game
    {
        #region Variablen

        // Grafische Ausgabe
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Font
        private SpriteFont spriteFont;

        // Viewport
        private Viewport viewport;

        // Tastatur abfragen
        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;

        // Sprites
        private Texture2D ShipTexture;
        private Texture2D StarTexture;
        private Texture2D LaserTexture;
        private Texture2D EnemyTexture;

        // Raumschiff Variablen
        private Vector2 shipPosition;
        private float shipSpeed = 5f;

        // Laser Variablen
        private List<Vector2> laserShots = new List<Vector2>();
        private float laserSpeed = 10f;

        //Enemy Variablen
        private Vector2 enemyStartPosition = new Vector2(100, 100);
        private float enemySpeed = 1f;

        // Sound Effekte
        private SoundEffect laserSound;
        private SoundEffect explosionSound;

        // Spieler-Punkte und Zeichenposition der Punkte
        private int playerScore;
        private Vector2 scorePosition;
        private Ship ship;
        private Enemy enemy;
        private Laser laser;

        #endregion Variablen

        #region Init

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // Auflösung anpassen
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        #endregion Init

        protected override void LoadContent()
        {
            // Ein SpriteBatch zum Zeichnen
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Viewport speichern
            viewport = _graphics.GraphicsDevice.Viewport;

            // Texturen laden
            ShipTexture = Content.Load<Texture2D>("ship");
            StarTexture = Content.Load<Texture2D>("starfield");
            LaserTexture = Content.Load<Texture2D>("laser");
            EnemyTexture = Content.Load<Texture2D>("enemy");

            // Font laden
            spriteFont = Content.Load<SpriteFont>("Verdana");

            // Sounds laden
            laserSound = Content.Load<SoundEffect>("laserfire");
            explosionSound = Content.Load<SoundEffect>("explosion");

            // Das Raumschiff positionieren
            shipPosition.X = viewport.Width / 2;
            shipPosition.Y = viewport.Height - 100;

            // Position der Score Ausgabe festlegen
            scorePosition = new Vector2(25, 25);

            ship = new Ship(ShipTexture, shipPosition, shipSpeed);
            enemy = new Enemy(EnemyTexture, enemyStartPosition, enemySpeed);
            laser = new Laser(LaserTexture, laserSound, explosionSound, laserSpeed);
        }

        #region Update Methode

        protected override void Update(GameTime gameTime)
        {
            currentKeyboardState = Keyboard.GetState();

            // Left
            if (currentKeyboardState.IsKeyDown(Keys.A))
            {
                ship.MoveShipLeft();
            }

            // Right
            if (currentKeyboardState.IsKeyDown(Keys.D))
            {
                ship.MoveShipRight(_graphics);
            }

            // Space
            if (IsNewKeyPressed(Keys.Space))
            {
                laser.FireLaser(ship);
            }

            previousKeyboardState = currentKeyboardState;

            enemy.UpdateEnemies();

            playerScore = laser.UpdateLaserShots(enemy, playerScore);

            base.Update(gameTime);
        }

        #endregion

        #region Input Operationen

        public bool IsNewKeyPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key) &&
                    !previousKeyboardState.IsKeyDown(key);
        }

        #endregion

        #region Draw Methoden

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // Hintergrund zeichnen
            DrawBackground();

            // Das Schiff zeichnen
            ship.DrawSpaceShip(_spriteBatch);

            // Laser zeichnen
            laser.DrawLaser(_spriteBatch);

            // Feinde zeichnen
            enemy.DrawEnemy(_spriteBatch);

            // Punkte anzeigen
            DrawScore();

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawBackground()
        {
            // Die Sternenfeld Grafik an der Position 0,0 zeichnen
            _spriteBatch.Draw(StarTexture, Vector2.Zero, Color.White);
        }

        private void DrawScore()
        {
            // Die Punkte (playerScore) oben links (scorePosition) anzeigen
            _spriteBatch.DrawString(spriteFont, playerScore + "", scorePosition, Color.White);
        }

        #endregion
    }
}