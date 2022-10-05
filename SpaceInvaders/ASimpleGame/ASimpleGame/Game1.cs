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

        // Zufallszahlen
        private Random random = new Random();

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

        // Gegner Variablen
        private readonly List<Vector2> enemyPositions = new List<Vector2>();

        private Vector2 enemyStartPosition = new Vector2(100, 100);
        private float enemyRadius;
        private float enemySpeed = 1f;
        private Color enemyColor;

        // Sound Effekte
        private SoundEffect laserSound;

        private SoundEffect explosionSound;

        // Spieler-Punkte und Zeichenposition der Punkte
        private int playerScore;

        private Vector2 scorePosition;

        private Ship ship;

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

            // Radius der Feinde festlegen
            if (EnemyTexture != null)
            {
                if (EnemyTexture.Width > EnemyTexture.Height)
                {
                    enemyRadius = EnemyTexture.Width;
                }
                else
                {
                    enemyRadius = EnemyTexture.Height;
                }

                // Gegner erzeugen
                CreateEnemies();
            }

            // Position der Score Ausgabe festlegen
            scorePosition = new Vector2(25, 25);

            ship = new Ship(ShipTexture, shipPosition, shipSpeed);
        }

        #region Gegner erzeugen

        public void CreateEnemies()
        {
            // Feinde erzeugen
            Vector2 position = enemyStartPosition;
            position.X -= EnemyTexture.Width / 2;

            // Eine Zufallszahl zwischen 3 und 10 ermitteln
            int count = random.Next(3, 11);

            // Gegener erzeugen
            for (int i = 0; i < count; i++)
            {
                enemyPositions.Add(position);
                position.X += EnemyTexture.Width + 15f;
            }

            // Farbwert ändern
            switch (count)
            {
                case 3:
                    enemyColor = Color.Red;
                    break;
                case 4:
                    enemyColor = Color.Green;
                    break;
                case 5:
                    enemyColor = Color.Yellow;
                    break;
                case 6:
                    enemyColor = Color.Blue;
                    break;
                case 7:
                    enemyColor = Color.Magenta;
                    break;
                case 8:
                    enemyColor = Color.Yellow;
                    break;
                case 9:
                    enemyColor = Color.White;
                    break;
                case 10:
                    enemyColor = Color.DarkGreen;
                    break;
                default:
                    break;
            }
        }

        #endregion

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
                FireLaser();
            }

            previousKeyboardState = currentKeyboardState;

            UpdateEnemies();

            UpdateLaserShots();

            base.Update(gameTime);
        }

        #endregion

        #region Input Operationen

        public bool IsNewKeyPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key) &&
                    !previousKeyboardState.IsKeyDown(key);
        }

        public void FireLaser()
        {
            // aktuelle Position des Schiffes auf dem Bildschirm speichern
            Vector2 position = ship.shipPosition;

            // Laserschuss vor das Schiff mittig platzieren
            position.Y -= ShipTexture.Height / 2;
            position.X -= LaserTexture.Width / 2;

            // Position in der Liste speichern
            laserShots.Add(position);

            PlayLaserSound();
        }

        #endregion

        #region Update von Lasern und Gegnern

        public void UpdateLaserShots()
        {
            int laserIndex = 0;

            while (laserIndex < laserShots.Count)
            {
                // hat der Schuss den Bildschirm verlassen?
                if (laserShots[laserIndex].Y < 0)
                {
                    laserShots.RemoveAt(laserIndex);
                }
                else
                {
                    // Position des Schusses aktualiesieren
                    Vector2 pos = laserShots[laserIndex];
                    pos.Y -= laserSpeed;
                    laserShots[laserIndex] = pos;

                    // Überprüfen ob ein Treffer vorliegt
                    int enemyIndex = 0;

                    while (enemyIndex < enemyPositions.Count)
                    {
                        // Abstand zwischen Feind-Position und Schuss-Position ermitteln
                        float distance = Vector2.Distance(enemyPositions[enemyIndex], laserShots[laserIndex]);

                        // Treffer?
                        if (distance < enemyRadius)
                        {
                            // Schuss entfernen
                            laserShots.RemoveAt(laserIndex);
                            // Feind entfernen
                            enemyPositions.RemoveAt(enemyIndex);
                            // Punkte erhöhen
                            playerScore++;

                            PlayExplosionSound();

                            // Schleife verlassen
                            break;
                        }
                        else
                        {
                            enemyIndex++;
                        }
                    }
                    laserIndex++;
                }
            }
        }

        public void UpdateEnemies()
        {
            // Startposition verändern
            enemyStartPosition.X += enemySpeed;

            // Bewegungsrichtung umkehren
            if (enemyStartPosition.X > 250)
            {
                enemySpeed *= -1;
            }
            else if (enemyStartPosition.X < 100f)
            {
                enemySpeed *= -1;
            }

            // Alle Feinde abgeschossen? Dann Neue Gegener
            if (enemyPositions.Count == 0 && EnemyTexture != null)
            {
                CreateEnemies();
            }

            // Aktualisieren
            for (int i = 0; i < enemyPositions.Count; i++)
            {
                Vector2 position = enemyPositions[i];
                position.X += enemySpeed;
                enemyPositions[i] = position;
            }
        }

        #endregion

        public void PlayExplosionSound()
        {
            // Explosions WAV abspielen
            explosionSound.Play();
        }

        public void PlayLaserSound()
        {
            // Laserschuss WAV abspielen
            laserSound.Play();
        }

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
            DrawLaser();

            // Feinde zeichnen
            DrawEnemy();

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

        private void DrawLaser()
        {
            // Die Liste mit den Laser-Schüssen (laserShots) durchlaufen
            // und alle Schüsse (LaserTexture) zeichnen
            foreach(Vector2 laser in laserShots)
            {
                _spriteBatch.Draw(LaserTexture, laser, Color.White);
            }
        }

        private void DrawEnemy()
        {
            // Die Liste mit allen Gegnern (enemyPositions) durchlaufen
            // und alle Feinde (EnemyTexture) zeichnen
            foreach(Vector2 enemy in enemyPositions)
            {
                _spriteBatch.Draw(EnemyTexture, enemy, enemyColor);
            }
        }

        private void DrawScore()
        {
            // Die Punkte (playerScore) oben links (scorePosition) anzeigen
            _spriteBatch.DrawString(spriteFont, playerScore + "", scorePosition, Color.White);
        }

        #endregion
    }
}