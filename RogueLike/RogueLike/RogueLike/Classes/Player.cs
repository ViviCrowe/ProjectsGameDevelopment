using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Player : Entity
{
    public Player(Viewport viewport) :
        base(viewport)
    {
        position = new Vector2(viewport.Width / 2, viewport.Height / 2);
        minimumHealth = maximumHealth = 100;
        teethValue = 0;
        this.attackDamage = 5;
        this.attackRange = 5;
    }
}
