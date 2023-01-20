using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RogueLike.Classes.AI;
using RogueLike.Classes.Items;
using RogueLike.Classes.Weapons;

namespace RogueLike.Classes
{
    public class Boss : Enemy
    {
        public Weapon SecondaryWeapon { get; set; }
        private Spear spear = new Spear();
        private Bow bow;
        private Sword sword = new Sword();
        public int MinionCountdown { get; set; }= 0;
        private Treasure treasure = new Treasure(Vector2.Zero);
        private Texture2D minionTexture;

        public Boss(Viewport viewport, Room room, Vector2 position) :
            base(viewport, Type.BOSS, position, room)
        {
            this.bow = new Bow(this);
            this.MovementSpeed = 2f;
            this.MaximumHealth = this.CurrentHealth = 3000;
            this.EquippedWeapon = spear;
            this.spear.AttackDamage = 50;
            this.spear.WeaponRange = 30;
            this.sword.WeaponRange = 5;
            this.SecondaryWeapon = bow;
            this.bow.AttackDamage = 50;
            this.VisionRange = 1000;
            this.ExperiencePoints = 3000;
            this.setEnemyAI(new BossAI()); // phase 1
        }

        public new void Update(Player player, Room room, ContentManager content)
        {
            base.Update(player, room, content);

            if(this.CurrentHealth < this.MaximumHealth*0.33) // phase 3
            {
                this.sword.AttackDamage = 300;
                this.MovementSpeed = 3f;
                this.bow.AttackDamage = 200;
            }
            else if(this.CurrentHealth < this.MaximumHealth*0.66) // phase 2
            {
                if(this.EquippedWeapon is Spear) this.EquippedWeapon = sword;
                else if(this.EquippedWeapon is Bow) this.SecondaryWeapon = sword;
                this.sword.AttackDamage = 100;
                this.bow.AttackDamage = 100;
            }
        }

        public void SummonMinions(Room room)
        {/*
            int count = (int) _random.NextInt64(1, 5);
            int type = (int) _random.NextInt64(0, 3);
            Enemy temp = null;
            for(; count > 0; count--)
            {
                do
                {
                    int xCoord = (int) _random.NextInt64((long) (room.Offset.X + room.tileDimensions.X*3), (long) (room.Offset.X + room.tileDimensions.X*(room.GridDimensions.X-3)));
                    int yCoord = (int) _random.NextInt64((long) (room.Offset.Y + room.tileDimensions.Y*3), (long) (room.Offset.Y + room.tileDimensions.Y*(room.GridDimensions.Y-3)));
                    temp = new Enemy(Entity.Viewport, (Type) type, new Vector2(xCoord, yCoord), room);
                    temp.Texture = this.minionTexture;
                } while(temp.CheckForCollision(room, 0, 0, false, false) != null);
                room.activeObjects.Add(temp);
            }
            if(this.enemyAI is TankAI) this.MinionCountdown = 200;
            else if(this.enemyAI is MeleeAI) this.MinionCountdown = 400;
            else if(this.enemyAI is ArcherAI) this.MinionCountdown = 600;*/
        }

        public void DropTreasure(Room room, ContentManager content)
        {
            this.treasure.Position = this.Position;
            room.items.Add(treasure);
        }

        public new void LoadAssets(ContentManager content)
        {
            base.LoadAssets(content, "boss"); //temp
            this.treasure.Texture = content.Load<Texture2D>("treasure");
        }
    }
}
