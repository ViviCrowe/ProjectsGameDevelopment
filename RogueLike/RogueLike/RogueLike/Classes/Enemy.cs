using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RogueLike.Classes;

// Context Class
public class Enemy : Entity
{
    // defense, attack bool values?
    private float visionRange;

    private EnemyAI enemyAI;

    public Enemy(Viewport viewport, EnemyAI enemyAI) :
        base(viewport)
    {
        this.enemyAI = enemyAI;
        this.position.X = viewport.Width / 2;
        this.position.Y = viewport.Height / 2 + 200; // TEST WERTE
        this.health = 100;
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

    public void Update(Room room, ContentManager content)
    {
        if(this.health <= 0) {
            room.activeObjects.Remove(this);
            if(this.weapon != null) {
                this.DropWeapon(room, content);
            }
        }
    }
}
