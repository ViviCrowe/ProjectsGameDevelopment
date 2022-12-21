using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueLike.Classes.Abilities;
using RogueLike.Classes.Weapons;

public class Player : Entity
{
    public AktivAbility aktivAbility { get; set; }
    public Player(Viewport viewport,Weapon weapon) :
        base(viewport, weapon)
    {
        position = new Vector2(viewport.Width / 2, viewport.Height / 2);
        minimumHealth = maximumHealth = 100;
        teethValue = 0;
        this.attackDamage = 5;
        this.attackRange = 5;
    }
}
