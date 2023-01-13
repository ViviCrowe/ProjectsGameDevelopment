using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RogueLike.Classes;
using RogueLike.Classes.Weapons;

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
    public Type type;
    public Tile destinationTile;
    public Vector2 playerDirection;

    public Enemy(Viewport viewport, Type type, Vector2 position, Room room)
    {
        this.type = type;

        switch(type) {
            case Type.ARCHER: 
            this.movementSpeed = 3.5f;
            this.maximumHealth = this.currentHealth = 200;
            this.weapon = new Bow(this);
            this.visionRange = 300;
            break;
            case Type.MELEE:
            this.movementSpeed = 2.5f;
            this.maximumHealth = this.currentHealth = 400;
            this.weapon = new Sword();
            this.visionRange = 150;
            break;
            case Type.TANK:
            this.movementSpeed = 1.5f;
            this.maximumHealth = this.currentHealth = 800;
            this.weapon = new Spear();
            this.visionRange = 100;
            break;
        }

        this.position.X = position.X;
        this.position.Y = position.Y;
        room.activeObjects.Add(this);

        this.enemyAI = new PatrolAI();
        
        teeth = new((int) (random.NextDouble()*10));
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
        if(this.currentHealth <= 0) 
        {
            room.activeObjects.Remove(this);

            if(random.NextDouble() < 0.75)
            {
                this.DropTeeth(room, content);
            }
            else if(random.NextDouble() < 0.2)
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
        if((PlayerInVision(room) || currentHealth < maximumHealth) && enemyAI is PatrolAI)
        {
            switch(type) 
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
        string name = this.type.ToString().ToLower();
        this.texture = content.Load<Texture2D>(name);
    }

}
