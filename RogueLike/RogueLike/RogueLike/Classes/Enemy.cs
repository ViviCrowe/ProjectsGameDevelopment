using Microsoft.Xna.Framework;
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
        this.position.X = 200;
        this.position.Y = 200;
    }

    public void setEnemyAI(EnemyAI enemyAI)
    {
        // switch at runtime
        this.enemyAI = enemyAI;
    }

    public void Move()
    {
        enemyAI.Move();
    }

    public void Attack()
    {
        enemyAI.Attack();
    }

}
