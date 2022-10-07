using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace ASimpleGame
{
    public class Laser
    {
        private Texture2D LaserTexture;
        private List<Vector2> laserShots = new List<Vector2>();
        private float laserSpeed;
        private SoundEffect laserSound;
        private SoundEffect explosionSound;

        public Laser()
        {
            laserSpeed = 10f;
        }

        public void FireLaser(Ship ship)
        {
            // aktuelle Position des Schiffes auf dem Bildschirm speichern
            Vector2 position = ship.shipPosition;

            // Laserschuss vor das Schiff mittig platzieren
            position.Y -= ship.ShipTexture.Height / 2;
            position.X -= LaserTexture.Width / 2;

            // Position in der Liste speichern
            laserShots.Add(position);

            PlayLaserSound();
        }

        public int UpdateLaserShots(Enemy enemy, int playerScore)
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

                    while (enemyIndex < enemy.enemyPositions.Count)
                    {
                        // Abstand zwischen Feind-Position und Schuss-Position ermitteln
                        float distance = Vector2.Distance(enemy.enemyPositions[enemyIndex], laserShots[laserIndex]);

                        // Treffer?
                        if (distance < enemy.enemyRadius)
                        {
                            // Schuss entfernen
                            laserShots.RemoveAt(laserIndex);
                            // Feind entfernen
                            enemy.enemyPositions.RemoveAt(enemyIndex);
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
            return playerScore;
        }

        private void PlayExplosionSound()
        {
            // Explosions WAV abspielen
            explosionSound.Play();
        }

        private void PlayLaserSound()
        {
            // Laserschuss WAV abspielen
            laserSound.Play();
        }

        public void DrawLaser(SpriteBatch _spriteBatch)
        {
            // Die Liste mit den Laser-Schüssen (laserShots) durchlaufen
            // und alle Schüsse (LaserTexture) zeichnen
            foreach (Vector2 laser in laserShots)
            {
                _spriteBatch.Draw(LaserTexture, laser, Color.White);
            }
        }

        public void LoadLaserAssets(ContentManager Content)
        {
            LaserTexture = Content.Load<Texture2D>("laser");
            explosionSound = Content.Load<SoundEffect>("explosion");
            laserSound = Content.Load<SoundEffect>("laserfire");
        }
    }
}
