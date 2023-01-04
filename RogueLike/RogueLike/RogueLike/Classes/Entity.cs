using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RogueLike.Classes;
using RogueLike.Classes.Weapons;

public abstract class Entity : GameObject
{
    // TODO: passive und aktive Abilities ergänzen
    public float movementSpeed;

    public Viewport viewport;

    public int maximumHealth;

    public int currentHealth;

    // attack Attribute == Fist == (Weapon==null), Basiswerte
    public int attackDamage;

    public float attackRange;

    public float attackSpeed;

    public Weapon weapon;

    public Wallet teeth;

    public Entity(Viewport viewport, Weapon weapon) // ggf. mehr Attribute hinzufügen
    {
        this.viewport = viewport;
        this.weapon = weapon;
        movementSpeed = 5f; // TODO: ändern sodass jeder Entitätentyp eigene Geschwindigkeit hat
    }

    public int Attack(Entity target)
    {
        int damageDealt = this.attackDamage + this.weapon.attackDamage;
        target.currentHealth -= damageDealt;
        return damageDealt;
    }

    public void Buy(GameObject item)
    {
        // TODO
    }

    public void DropWeapon(Room room, ContentManager content)
    {
        if (this.weapon != null && this.weapon is not Fist)
        {
            this.weapon.position = this.position;
            room.items.Add(this.weapon);
            room.LoadItemAssets (content); // nötig um null exception zu vermeiden
            this.weapon = null;
            this.LoadAssets(content, "character"); // TESTWEISE FÜR PLAYER NUR
        }
    }

    public void DropTeeth(Room room, ContentManager content)
    {
        if (this.teeth.value > 0)
        {
            this.teeth.position = this.position;
            room.items.Add(this.teeth);
            room.LoadItemAssets (content);
            this.teeth = null;        }
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
            else if (item is Wallet wallet)
            {
                this.UpdateTeethValue(wallet.value);
                room.items.Remove (wallet);
            }
        }
    }

    public void Sell(GameObject item)
    {
        // TODO
    }

    public void Update(Room room, ContentManager content) 
    {
        // ...
    }

    public void UseActiveAbility()
    {
        // TODO
    }

    //Sets the minimum value of the Helth bar
    public void SetCurrentHealth(int currentHealth)
    {
        if (currentHealth <= maximumHealth && currentHealth >= 0)
        {
            this.currentHealth = currentHealth;
        }
    }

    public void UpdateTeethValue(int teethValue)
    {
        if ((this.teeth.value + teethValue) >= 0)
        {
            this.teeth.value += teethValue;
        }
        else
        {
            this.teeth.value = 0;
        }
    }
}
