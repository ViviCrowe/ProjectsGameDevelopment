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
        currentHealth = maximumHealth = 100;
        teeth = new(0);
        //this.weapon = new Fist();
        this.weapon = new Bow(this); // TEST
        this.movementSpeed = 5f;
    }
}
