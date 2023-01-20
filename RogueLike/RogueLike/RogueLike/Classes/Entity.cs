using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RogueLike.Classes.Items;
using RogueLike.Classes.Weapons;

namespace RogueLike.Classes
{
    public abstract class Entity : GameObject
    {
        public float MovementSpeed { get; set; }

        public static Viewport Viewport { get; set; }

        public int MaximumHealth { get; set; }

        public int CurrentHealth { get; set; }

        public int AttackCountdown { get; set; } = 60;

        public Weapon EquippedWeapon { get; set; }

        public Wallet Teeth { get; set; }

        public int VisionRange { get; set; }

        private List<int> damageTaken = new List<int>();
        public static SpriteFont Font { get; set; }
        private int displayDamageCountdown = 45;
        public static SoundEffect AttackMissSound { get; set; }
        public static SoundEffect AttackSwordSound { get; set; }
        public static SoundEffect AttackBowSound { get; set; }
        public static SoundEffect AttackSpearSound { get; set; }
        public static SoundEffect AttackFistSound { get; set; }
        public static SoundEffect DamagePlayerSound { get; set; }
        public static SoundEffect DamageEnemySound { get; set; }
        public static SoundEffect PickupDropSound { get; set; }

        public enum Direction
        {
            DOWN,
            UP,
            LEFT,
            RIGHT,
            DOWNLEFT,
            DOWNRIGHT,
            UPLEFT,
            UPRIGHT
        }

        public Direction directionFacing;

        public Entity(Viewport viewport, Weapon weapon)
        {
            Entity.Viewport = viewport;
            this.EquippedWeapon = weapon;
        }

        public Entity()
        {
        }

        public int Attack(Entity target)
        {
            int damageDealt = 0;

            if (this.AttackCountdown == 0 || this.EquippedWeapon is Bow) // attack possible
            {
                Random random = new Random();
                if(random.NextDouble() < 0.1) // miss
                {
                    damageDealt = -1;
                    AttackMissSound.Play();
                }
                else
                {
                    if(this.EquippedWeapon is Bow) AttackBowSound.Play();
                    else if(this.EquippedWeapon is Spear) AttackSpearSound.Play();
                    else if(this.EquippedWeapon is Sword) AttackSwordSound.Play();
                    else if(this.EquippedWeapon is Fist) AttackFistSound.Play();
                    
                    damageDealt = this.EquippedWeapon.AttackDamage;
                    if(this is Player player) damageDealt += player.BaseAttack;
                    else if(target is Player _player) damageDealt -= _player.BaseDefense;     
                    target.CurrentHealth -= damageDealt;
                    
                    if(target is Player) DamagePlayerSound.Play();
                    else DamageEnemySound.Play();
                    
                }
                AttackCountdown = 55;
                target.damageTaken.Add(damageDealt);
                return damageDealt;
            }
            else
            {
                return 0;
            }
        }

        public void DropWeapon(Room room, ContentManager content)
        {
            if (this.EquippedWeapon != null && this.EquippedWeapon is not Fist)
            {
                this.EquippedWeapon.Position = this.Position;
                room.items.Add(this.EquippedWeapon);
                room.LoadItemAssets (content);
                this.EquippedWeapon = new Fist();
                if (this is Player)
                {
                    this.LoadAssets(content, "Player_no_weapon");
                }
            }
        }

        public void MoveDown()
        {
            Position.Y += MovementSpeed;
            this.directionFacing = Direction.DOWN;
        }

        public void MoveLeft()
        {
            Position.X -= MovementSpeed;
            this.directionFacing = Direction.LEFT;
        }

        public void MoveRight()
        {
            Position.X += MovementSpeed;
            this.directionFacing = Direction.RIGHT;
        }

        public void MoveUp()
        {
            Position.Y -= MovementSpeed;
            this.directionFacing = Direction.UP;
        }

        public void MoveUpRight()
        {
            Position.X += MovementSpeed;
            Position.Y -= MovementSpeed;
            this.directionFacing = Direction.UPRIGHT;
        }

        public void MoveUpLeft()
        {
            Position.X -= MovementSpeed;
            Position.Y -= MovementSpeed;
            this.directionFacing = Direction.UPLEFT;
        }

        public void MoveDownRight()
        {
            Position.X += MovementSpeed;
            Position.Y += MovementSpeed;
            this.directionFacing = Direction.DOWNRIGHT;
        }

        public void MoveDownLeft()
        {
            Position.X -= MovementSpeed;
            Position.Y += MovementSpeed;
            this.directionFacing = Direction.DOWNLEFT;
        }

        //Sets the minimum value of the Health bar
        public void SetCurrentHealth(int CurrentHealth)
        {
            if (CurrentHealth <= MaximumHealth && CurrentHealth >= 0)
            {
                this.CurrentHealth = CurrentHealth;
            }
        }

        public void Update(Room room)
        {
            if (this.EquippedWeapon is Bow bow)
            {
                bow.UpdateArrows(this, room);
            }

            if (AttackCountdown > 0) AttackCountdown--;
        }

        public GameObject CheckForCollision(Room CurrentRoom, int factorX, int factorY, bool attack, bool vision)
        {
            BoundingBox boundingBox_1 = this.CreateBoundingBox(factorX, factorY);
            foreach (GameObject obj in CurrentRoom.passiveObjects)
            {
                BoundingBox boundingBox_2;
                if(obj is Tower tower) boundingBox_2 = tower.GetBoundingBox();
                else boundingBox_2 = obj.CreateBoundingBox();
                if (boundingBox_1.Intersects(boundingBox_2))
                {
                    return obj;
                }
            }
            if (CurrentRoom.activeObjects.Count > 1)
            {
                foreach (Entity entity_2 in CurrentRoom.activeObjects)
                {
                    if (this != entity_2 && entity_2 != null)
                    {
                        BoundingBox boundingBox_2 = entity_2.CreateBoundingBox();
                        if (attack)
                        {
                            boundingBox_1.Min.X -= this.EquippedWeapon.WeaponRange;
                            boundingBox_1.Min.Y -= this.EquippedWeapon.WeaponRange;
                            boundingBox_1.Max.X += this.EquippedWeapon.WeaponRange;
                            boundingBox_1.Max.Y += this.EquippedWeapon.WeaponRange;
                            if(this is Player player) 
                            {
                                boundingBox_1.Min.X -= player.BaseRange;
                                boundingBox_1.Min.Y -= player.BaseRange;
                                boundingBox_1.Max.X += player.BaseRange;
                                boundingBox_1.Max.Y += player.BaseRange;
                            }
                        }
                        if (vision)
                        {
                            boundingBox_1.Min.X -= this.VisionRange;
                            boundingBox_1.Min.Y -= this.VisionRange;
                            boundingBox_1.Max.X += this.VisionRange;
                            boundingBox_1.Max.Y += this.VisionRange;
                        }
                        if (
                            boundingBox_1.Intersects(boundingBox_2) &&
                            vision == false
                        )
                        {
                            return entity_2;
                        }
                        if (
                            boundingBox_1.Intersects(boundingBox_2) &&
                            vision == true
                        )
                        {
                            if (entity_2 is Player) return entity_2;
                        }
                    }
                }
            }
            return null;
        }

        public GameObject CheckForItemCollision(Room CurrentRoom)
        {
            BoundingBox boundingBox_1 = this.CreateBoundingBox();

            foreach (GameObject item in CurrentRoom.items)
            {
                BoundingBox boundingBox_2 = item.CreateBoundingBox();

                if (boundingBox_1.Intersects(boundingBox_2))
                {
                    return item;
                }
            }
            return null;
        }

        public BoundingBox CreateBoundingBox(int factorX, int factorY)
        {
            return new BoundingBox(new Vector3(
                    this.Position.X + this.MovementSpeed * factorX - (this.Texture.Width / 2) + 10,
                    this.Position.Y + this.MovementSpeed * factorY - (this.Texture.Height / 2),
                    0),
                    new Vector3(
                    this.Position.X + this.MovementSpeed * factorX + (this.Texture.Width / 2) - 10,
                    this.Position.Y + this.MovementSpeed * factorY + (this.Texture.Height / 2),
                    0));
        }

        public static void LoadAssets(ContentManager content)
        {
            Font = content.Load<SpriteFont>("gamefont");
            AttackMissSound = content.Load<SoundEffect>("attack_miss");
            AttackSwordSound = content.Load<SoundEffect>("attack_sword");
            AttackBowSound = content.Load<SoundEffect>("attack_arrow");
            AttackSpearSound = content.Load<SoundEffect>("attack_spear");
            AttackFistSound = content.Load<SoundEffect>("attack_fist");
            DamagePlayerSound = content.Load<SoundEffect>("damage_player");
            DamageEnemySound = content.Load<SoundEffect>("damage_enemy");
            PickupDropSound = content.Load<SoundEffect>("drop_pickup");
        }

        public new void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            // draw entity
            spriteBatch.Draw(Texture, Position, null, Color.White, 0f, new Vector2(Texture.Width / 2f, 
                Texture.Height / 2f), 1.0f, SpriteEffects.None, layerDepth);
            
            // case: draw arrows
            if (this.EquippedWeapon is Bow bow)
            {
                ((Bow) bow).Draw(spriteBatch);
            }

            // display damage taken
            if(damageTaken.Count > 0)
            { 
                displayDamageCountdown--;
                if(displayDamageCountdown == 0)
                {
                    damageTaken.RemoveAt(0);
                    displayDamageCountdown = 45;
                }
                foreach(int hit in damageTaken)
                {
                    String damage;
                    if(hit == -1)
                    {
                        damage = "miss";
                    }
                    else
                    {
                        damage = hit.ToString();
                    }
                    spriteBatch.DrawString(Font, damage, this.Position - new Vector2(this.Texture.Width/4, (damageTaken.IndexOf(hit))*32 + this.Texture.Height), 
                        Color.Red, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
                    }

            }
        }
    }
}
