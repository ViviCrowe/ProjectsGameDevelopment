using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RogueLike.Classes;
using RogueLike.Classes.Weapons;

// Context Class
public class Enemy : Entity
{
    private float visionRange;
    private EnemyAI enemyAI;
    private static Random random = new Random();
    public enum Type
    {
        ARCHER,
        MELEE,
        TANK    
    };
    private Type type;

    public Enemy(Viewport viewport, Type type, Vector2 position, Weapon weapon, Room room) :
        base(viewport, weapon)
    {
        this.enemyAI = new PatrolAI();
        this.position.X = position.X;
        this.position.Y = position.Y;
        this.maximumHealth = this.currentHealth = 100;
        room.activeObjects.Add(this);
        teeth = new((int) (random.NextDouble()*10));
    }
    
    public new void Attack(Entity entity)
    {
        enemyAI.Attack((Player) entity);
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
        base.Update(room, content);

        // handle enemy death
        if(this.currentHealth <= 0) 
        {
            room.activeObjects.Remove(this);

            if(random.NextDouble() < 0.75)
            {
                this.DropTeeth(room, content);
            }
            else 
            {   
                this.DropWeapon(room, content);
            }
        }

        //room.StartAStar(this);
        //room.FollowPath(this);
    }

    public void LoadAssets(ContentManager content) 
    {
        string name = this.type.ToString().ToLower();
        this.texture = content.Load<Texture2D>(name);
    }

}
