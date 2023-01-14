using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RogueLike.Classes.Weapons
{
    internal class Bow : Weapon
    {
        private List<Arrow> arrows = new List<Arrow>();
        private int arrowCountdown = 0;

        public Bow(Entity entity)
        {
            this.weaponRange = 2000;
            this.attackSpeed = 8f;
            this.attackDamage = 100;
        }

        public void FireArrow(Entity entity)
        {
            if(arrowCountdown > 0) return;
            Vector2 direction = new(0, 0);
            float x = entity.position.X;
            float y = entity.position.Y;
            if(entity is Player)
            {
                switch(entity.directionFacing)
                {
                    case Entity.Direction.UP:
                    y -= entity.texture.Height/2 + Arrow.texture.Height;
                    direction.Y = -1;
                    break;
                    case Entity.Direction.DOWN:
                    y += entity.texture.Height/2 + Arrow.texture.Height;
                    direction.Y = 1;
                    break;
                    case Entity.Direction.RIGHT:
                    x += entity.texture.Width/2 + Arrow.texture.Width*2;
                    direction.X = 1;
                    break;
                    case Entity.Direction.LEFT:
                    x -= entity.texture.Width/2 + Arrow.texture.Width*2;
                    direction.X = -1;
                    break;
                    case Entity.Direction.UPRIGHT:
                    y -= entity.texture.Height/2 + Arrow.texture.Height;
                    direction.Y = -1;
                    x += entity.texture.Width/2 + Arrow.texture.Width;
                    direction.X = 1;
                    break;
                    case Entity.Direction.UPLEFT:
                    y -= entity.texture.Height/2 + Arrow.texture.Height;
                    direction.Y = -1;
                    x -= entity.texture.Width/2 + Arrow.texture.Width;
                    direction.X = -1;
                    break;
                    case Entity.Direction.DOWNRIGHT:
                    y += entity.texture.Height/2 + Arrow.texture.Height;
                    direction.Y = 1;
                    x += entity.texture.Width/2 + Arrow.texture.Width;
                    direction.X = 1;
                    break;
                    case Entity.Direction.DOWNLEFT:
                    y += entity.texture.Height/2 + Arrow.texture.Height;
                    direction.Y = 1;
                    x -= entity.texture.Width/2 + Arrow.texture.Width;
                    direction.X = -1;
                    break;
                }
                direction = Vector2.Normalize(direction);
            }
            else if(entity is Enemy enemy)
            {
                enemy.playerDirection = Vector2.Normalize((enemy.playerDirection));
                x += enemy.playerDirection.X*64;
                y += enemy.playerDirection.Y*64;
                direction = enemy.playerDirection;
            }
            arrows.Add(new Arrow(new(x, y), direction));
            //arrowSound.Play();
            arrowCountdown = 40;
        }

        public void UpdateArrows(Entity entity, Room room)  
        {
            if(arrowCountdown > 0) arrowCountdown--;
            int arrowIndex = 0;
            while(arrowIndex < arrows.Count)
            {
                Arrow currentArrow = arrows[arrowIndex];
                currentArrow.position.X += attackSpeed*currentArrow.direction.X;
                currentArrow.position.Y += attackSpeed*currentArrow.direction.Y;
            
                if(currentArrow.CheckForCollision(room) != null) 
                {
                    if(currentArrow.CheckForCollision(room) is Entity target)
                    {
                        entity.Attack(target);
                    }
                    arrows.RemoveAt(arrowIndex);
                    break;
                }
                    arrowIndex++;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(Arrow arrow in arrows)
            {                
                Vector2 startAngle = new(0, -1);
                double scalarProduct = startAngle.X*arrow.direction.X + startAngle.Y*arrow.direction.Y;
                float angle = (float) (Math.Acos(scalarProduct));
                if(arrow.direction.X < 0) angle = -angle;
                spriteBatch.Draw(Arrow.texture, arrow.position, null, Color.White, angle, 
                    Vector2.Zero, 1.0f, SpriteEffects.None, 0.2f);
            }
        }

        public new void LoadAssets(ContentManager content, string name)
        {
            //texture = content.Load<Texture2D>(name); // load character with specific weapon
            Arrow.texture = content.Load<Texture2D>("arrow"); // load different arrow rotations according to the direction OR rotate it
        }
    }
}
