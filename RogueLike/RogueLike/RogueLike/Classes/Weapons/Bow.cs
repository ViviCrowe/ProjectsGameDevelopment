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
        private float attackSpeed;

        public Bow(Entity entity)
        {
            this.WeaponRange = 2000;
            this.attackSpeed = 6f;
            this.AttackDamage = 75;
        }

        public void FireArrow(Entity entity)
        {
            if(arrowCountdown > 0) return;
            Vector2 direction = new(0, 0);
            float x = entity.Position.X;
            float y = entity.Position.Y;
            if(entity is Player)
            {
                switch(entity.directionFacing)
                {
                    case Entity.Direction.UP:
                    y -= entity.Texture.Height/2 + Arrow.Texture.Height;
                    direction.Y = -1;
                    break;
                    case Entity.Direction.DOWN:
                    y += entity.Texture.Height/2 + Arrow.Texture.Height;
                    direction.Y = 1;
                    break;
                    case Entity.Direction.RIGHT:
                    x += entity.Texture.Width/2 + Arrow.Texture.Width*2;
                    direction.X = 1;
                    break;
                    case Entity.Direction.LEFT:
                    x -= entity.Texture.Width/2 + Arrow.Texture.Width*2;
                    direction.X = -1;
                    break;
                    case Entity.Direction.UPRIGHT:
                    y -= entity.Texture.Height/2 + Arrow.Texture.Height;
                    direction.Y = -1;
                    x += entity.Texture.Width/2 + Arrow.Texture.Width;
                    direction.X = 1;
                    break;
                    case Entity.Direction.UPLEFT:
                    y -= entity.Texture.Height/2 + Arrow.Texture.Height;
                    direction.Y = -1;
                    x -= entity.Texture.Width/2 + Arrow.Texture.Width;
                    direction.X = -1;
                    break;
                    case Entity.Direction.DOWNRIGHT:
                    y += entity.Texture.Height/2 + Arrow.Texture.Height;
                    direction.Y = 1;
                    x += entity.Texture.Width/2 + Arrow.Texture.Width;
                    direction.X = 1;
                    break;
                    case Entity.Direction.DOWNLEFT:
                    y += entity.Texture.Height/2 + Arrow.Texture.Height;
                    direction.Y = 1;
                    x -= entity.Texture.Width/2 + Arrow.Texture.Width;
                    direction.X = -1;
                    break;
                }
                direction = Vector2.Normalize(direction);
            }
            else if(entity is Enemy enemy)
            {
                enemy.PlayerDirection = Vector2.Normalize((enemy.PlayerDirection));
                x += enemy.PlayerDirection.X*64;
                y += enemy.PlayerDirection.Y*64;
                direction = enemy.PlayerDirection;
            }
            arrows.Add(new Arrow(new(x, y), direction));
            //arrowSound.Play();
            arrowCountdown = 55;
        }

        public void UpdateArrows(Entity entity, Room room)  
        {
            if(arrowCountdown > 0) arrowCountdown--;
            int arrowIndex = 0;
            while(arrowIndex < arrows.Count)
            {
                Arrow currentArrow = arrows[arrowIndex];
                currentArrow.Position.X += attackSpeed*currentArrow.Direction.X;
                currentArrow.Position.Y += attackSpeed*currentArrow.Direction.Y;
            
                if(currentArrow.CheckForCollision(room) != null) 
                {
                    if(currentArrow.CheckForCollision(room) is Entity target)
                    {
                        if((entity is Player && target is not Player) || (entity is Enemy && target is Player))
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
                double scalarProduct = startAngle.X*arrow.Direction.X + startAngle.Y*arrow.Direction.Y;
                float angle = (float) (Math.Acos(scalarProduct));
                if(arrow.Direction.X < 0) angle = -angle;
                spriteBatch.Draw(Arrow.Texture, arrow.Position, null, Color.White, angle, 
                    Vector2.Zero, 1.0f, SpriteEffects.None, 0.2f);
            }
        }

        public new void LoadAssets(ContentManager content, string name)
        {
            //Texture = content.Load<Texture2D>(name); // load character with specific weapon
            Arrow.Texture = content.Load<Texture2D>("arrow"); // load different arrow rotations according to the direction OR rotate it
        }
    }
}
