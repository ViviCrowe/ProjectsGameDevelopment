using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RogueLike.Classes;

public abstract class Entity : GameObject
{
    // TODO: passive und aktive Abilities ergänzen
    public float movementSpeed;

    public Viewport viewport;

    public int maximumHealth;
    public int minimumHealth;

    public int teethValue;

    // attack Attribute == Fist == (Weapon==null), Basiswerte
    public int attackDamage;

    public float attackRange;

    public float attackSpeed;

    public Weapon weapon;

    public Entity(Viewport viewport) // ggf. mehr Attribute hinzufügen
    {
        this.viewport = viewport;
        movementSpeed = 5f; // TODO: ändern sodass jeder Entitätentyp eigene Geschwindigkeit hat
    }

    public void Attack(Entity target)
    {
        target.minimumHealth -= (this.attackDamage + this.weapon.weaponDamage);
    }

    public void Buy(GameObject item)
    {
        // TODO
    }

    public void DropWeapon(Room room, ContentManager content)
    {
        if (this.weapon != null)
        {
            this.weapon.position = this.position;
            room.items.Add(this.weapon);
            room.LoadItemAssets (content); // nötig um null exception zu vermeiden
            this.weapon = null;
            this.LoadAssets(content, "character"); // TESTWEISE FÜR PLAYER NUR
        }
    }

    public void MoveDown()
    {
        position.Y += movementSpeed;
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

    public void PickUpItem(GameObject item, Room room, ContentManager content)
    {
        if (item != null)
        {
            if (item is Weapon newWeapon)
            {
                this.weapon = newWeapon;
                room.items.Remove (newWeapon);
                this.LoadAssets(content, "character_with_sword"); // TESTWEISE FÜR PLAYER NUR
            }
        }
    }

    public void Sell(GameObject item)
    {
        // TODO
    }

    public void UseActiveAbility() 
    {
        // TODO
    }
}
