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
        teeth = new((int) (random.NextDouble()*10));
        room.activeObjects.Add(this);
    }
    
    public void Attack()
    {
        enemyAI.Attack();
    }

    public void Move()
    {
        enemyAI.Move();
    }

    public void setEnemyAI(EnemyAI enemyAI)
    {
        // switch at runtime
        this.enemyAI = enemyAI;
    }

    public new void Update(Room room, ContentManager content)
    {
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
    }

    public void LoadAssets(ContentManager content) 
    {
        string name = this.type.ToString().ToLower();
        this.texture = content.Load<Texture2D>(name);
    }
}
