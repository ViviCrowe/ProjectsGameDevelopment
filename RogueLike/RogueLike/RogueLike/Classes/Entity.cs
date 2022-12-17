using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RogueLike.Classes;

public abstract class Entity : GameObject
{
    public float movementSpeed;

    public Viewport viewport;

    private int health;

    private int teethValue;
    
    // attack Attribute == Fist == (Weapon==null), Basiswerte
    private int attackDamage;
    private float attackRange;
    private float attackSpeed;

    public Weapon weapon;

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

    public void DropWeapon(Room room, ContentManager content) 
    {
        if (this.weapon != null)
        {
            this.weapon.position = this.position;
            room.items.Add(this.weapon);
            room.LoadItemAssets(content); // necessary to avoid null exception
            this.weapon = null;
            this.LoadAssets(content, "character"); // TESTWEISE FÜR PLAYER NUR
        } 
    }

    public void PickUpItem(GameObject item, Room room, ContentManager content)
    {
        if (item != null)
        {
            if(item is Weapon newWeapon) 
            {
                this.weapon = newWeapon;
                room.items.Remove(newWeapon);
                this.LoadAssets(content, "character_with_sword"); // TESTWEISE FÜR PLAYER NUR
            }
        }
    }
    // attack
    // buy/sell things
    // use abilities
}
