using Microsoft.Xna.Framework.Graphics;

public abstract class Entity : GameObject
{
    private Viewport viewport;
    public float movementSpeed;
    private int health;
    private int attackDamage;
    private int teethValue;
    // private Weapon weapon; or Item

    public Entity(Viewport viewport) // add more attributes?
    {
        this.viewport = viewport;
        movementSpeed = 5f; // todo: change so that every type of entity has a different speed
    }

    public void MoveLeft()
    {
        position.X -= movementSpeed;
    }

    public void MoveRight()
    {
        position.X += movementSpeed;
    }

    public void MoveUp()
    {
        position.Y -= movementSpeed;
    }

    public void MoveDown()
    {
        position.Y += movementSpeed;
    }

    public void DropWeapon() { // or Item
        // todo
    }
}