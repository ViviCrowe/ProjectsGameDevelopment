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
        private Treasure treasure = new Treasure(Vector2.Zero);
        public int Level { get; set; }
        public static int BossCounter { get; set; } = 1;

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
            this.SetEnemyAI(new BossAI()); // phase 1
            this.Level = BossCounter;
            if(BossCounter < 3) BossCounter++;
            else BossCounter = 1;
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

        public void DropTreasure(Room room, ContentManager content)
        {
            this.treasure.Position = this.Position;
            room.items.Add(treasure);
        }

        public new void LoadAssets(ContentManager content, string name)
        {
            base.LoadAssets(content, name); 
            this.treasure.Texture = content.Load<Texture2D>("treasure");
        }
    }
}
