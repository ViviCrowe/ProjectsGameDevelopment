using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RogueLike.Classes.AI;
using RogueLike.Classes.Weapons;

namespace RogueLike.Classes{
// Context Class
public class Enemy : Entity
{
    private EnemyAI enemyAI;
    private static Random random = new Random();
    public enum Type
    {
        ARCHER,
        MELEE,
        TANK    
    };
    public Type EnemyType { get; set; }
    public Tile DestinationTile { get; set; }
    public Vector2 PlayerDirection { get; set; }

    public Enemy(Viewport viewport, Type EnemyType, Vector2 Position, Room room)
    {
        this.EnemyType = EnemyType;

        switch(EnemyType) {
            case Type.ARCHER: 
            this.MovementSpeed = 3.5f;
            this.MaximumHealth = this.CurrentHealth = 200;
            this.EquippedWeapon = new Bow(this);
            this.VisionRange = 300;
            break;
            case Type.MELEE:
            this.MovementSpeed = 2.5f;
            this.MaximumHealth = this.CurrentHealth = 400;
            this.EquippedWeapon = new Sword();
            this.VisionRange = 150;
            break;
            case Type.TANK:
            this.MovementSpeed = 1.5f;
            this.MaximumHealth = this.CurrentHealth = 600;
            this.EquippedWeapon = new Spear();
            this.VisionRange = 100;
            break;
        }

        this.Position.X = Position.X;
        this.Position.Y = Position.Y;
        room.activeObjects.Add(this);

        this.enemyAI = new PatrolAI();
        
        Teeth = new((int) (random.NextDouble()*10));
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

    public void Move(Room room, Tile destinationTile)
    {
        enemyAI.Move(this, room, destinationTile);
    }

    private void setEnemyAI(EnemyAI enemyAI)
    {
        // switch at runtime
        this.enemyAI = enemyAI;
    }

    public void Update(Player player, Room room, ContentManager content)
    {
        base.Update(room);
        // enemy death
        if(this.CurrentHealth <= 0) 
        {
            room.activeObjects.Remove(this);

            if(random.NextDouble() < 0.9)
            {
                this.DropTeeth(room, content);
            }
            else if(random.NextDouble() < 0.4)
            {   
                this.DropWeapon(room, content);
            }
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

    public void LoadAssets(ContentManager content) 
    {
        string name = this.EnemyType.ToString().ToLower();
        this.Texture = content.Load<Texture2D>(name);
    }

}
}