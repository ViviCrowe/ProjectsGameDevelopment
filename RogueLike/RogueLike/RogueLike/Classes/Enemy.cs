using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RogueLike.Classes.AI;
using RogueLike.Classes.Items;
using RogueLike.Classes.Weapons;

namespace RogueLike.Classes{
// Context Class
public class Enemy : Entity
{
    public EnemyAI enemyAI { get; set; }
    public static Random _random { get; set; } = new Random();
    public enum Type
    {
        ARCHER,
        MELEE,
        TANK,
        BOSS
    };
    public Type EnemyType { get; set; }
    public Tile DestinationTile { get; set; }
    public Vector2 PlayerDirection { get; set; }
    public int ExperiencePoints { get; set; }

    public Enemy(Viewport viewport, Type enemyType, Vector2 position, Room room)
    {
        this.EnemyType = enemyType;

        switch(EnemyType) {
            case Type.ARCHER: 
            this.MovementSpeed = 3.5f;
            this.MaximumHealth = this.CurrentHealth = 200;
            this.EquippedWeapon = new Bow(this);
            this.VisionRange = 300;
            this.ExperiencePoints = 25;
            room.activeObjects.Add(this);
            break;
            case Type.MELEE:
            this.MovementSpeed = 2.5f;
            this.MaximumHealth = this.CurrentHealth = 400;
            this.EquippedWeapon = new Sword();
            this.VisionRange = 150;
            this.ExperiencePoints = 50;
            room.activeObjects.Add(this);
            break;
            case Type.TANK:
            this.MovementSpeed = 1.5f;
            this.MaximumHealth = this.CurrentHealth = 600;
            this.EquippedWeapon = new Spear();
            this.VisionRange = 100;
            this.ExperiencePoints = 75;
            room.activeObjects.Add(this);
            break;
        }

        this.Position = position;

        this.enemyAI = new PatrolAI();
        
        Teeth = new((int) (_random.NextDouble()*10));
    }

    public void DropTeeth(Room room, ContentManager content)
    {
        if (this.Teeth.Value > 0 && this.Teeth != null)
        {
            this.Teeth.Position = this.Position;
            room.items.Add(this.Teeth);
            room.LoadItemAssets (content);
            this.Teeth = null;
        }
    }

    public void DropPotion(Room room, ContentManager content)
    {
        double randomNumber = _random.NextDouble();
        if(randomNumber < 0.3) room.items.Add(new Potion(200, this.Position, Potion.PotionType.HEALING));
        else if(randomNumber < 0.6) room.items.Add(new Potion(10, this.Position, Potion.PotionType.ATTACK));
        else room.items.Add(new Potion(10, this.Position, Potion.PotionType.DEFENSE));
        room.LoadItemAssets(content);
    }

    public void Move(Room room, Tile destinationTile)
    {
        enemyAI.Move(this, room, destinationTile);
    }

    public void setEnemyAI(EnemyAI enemyAI)
    {
        // switch at runtime
        this.enemyAI = enemyAI;
    }

    public void Update(Player player, Room room, ContentManager content)
    {
        base.Update(room);

        // enemy death
        if(this.CurrentHealth <= 0 ) 
        {
            room.activeObjects.Remove(this);
            double randomDrop = _random.NextDouble();
            if(this is Boss boss)
            {
                boss.DropTreasure(room, content);
            }
            else if(randomDrop < 0.4)
            {
                this.DropWeapon(room, content);
            }
            else if(randomDrop < 0.8)
            {   
               this.DropPotion(room, content);
            }
            else if(randomDrop > 0.2)
            {
                this.DropTeeth(room, content);
            }
            player.Experience += this.ExperiencePoints;
        }

        // enemy movement
        this.enemyAI.UpdateDestination(this, player, room);
        
        if(room.StartAStar(this))
        {   
            room.FollowPath(this);
        }

        // vision field
        UpdatePlayerInVision(room);

    }

    private void UpdatePlayerInVision(Room room)
    {
        if((PlayerInVision(room) || CurrentHealth < MaximumHealth) && enemyAI is PatrolAI)
        {
            switch(EnemyType) 
            {
                case Type.ARCHER: setEnemyAI(new ArcherAI());
                break;
                case Type.MELEE: setEnemyAI(new MeleeAI());
                break;
                case Type.TANK: setEnemyAI(new TankAI());
                break;
            }
        }
        else if(!PlayerInVision(room) && enemyAI is not PatrolAI)
        {
            setEnemyAI(new PatrolAI());
        }
    }

    private bool PlayerInVision(Room room)
    {
        if(this.CheckForCollision(room, 0, 0, false, true) != null) return true;
        return false;
    }

    public new void LoadAssets(ContentManager content) 
    {
        string name = this.EnemyType.ToString().ToLower();
        this.Texture = content.Load<Texture2D>(name);
    }
}
}