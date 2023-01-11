using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RogueLike.Classes;
using RogueLike.Classes.Weapons;

public abstract class Entity : GameObject
{
    // TODO: passive und aktive Abilities ergänzen
    public float movementSpeed;

    public static Viewport viewport;

    public int maximumHealth;

    public int currentHealth;

    public int attackDamage;

    public float attackRange;

    public float attackSpeed;

    public Weapon weapon;

    public Wallet teeth;
    public Vector2 tilePosition = new Vector2();

    public Entity(Viewport viewport, Weapon weapon) // ggf. mehr Attribute hinzufügen
    {
        Entity.viewport = viewport;
        this.weapon = weapon;
        movementSpeed = 5f; // TODO: ändern sodass jeder Entitätentyp eigene Geschwindigkeit hat
    }

    public Entity(Weapon weapon) // ggf. mehr Attribute hinzufügen
    {
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
            this.teeth = null;
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

/*
    public static Vector2 CoordinatesToTile(Vector2 position, Room room)
    {
        return new Vector2 ((int) Math.Ceiling(position.X / ((double) viewport.Width / room.Tiles.GetLength(0))), 
        (int) Math.Floor(position.Y / ((double) viewport.Height / room.Tiles.GetLength(1))));
    }

    public static Vector2 TileToCoordinates(Vector2 tilePosition, Room room)
    {
        return new Vector2 (tilePosition.X * (viewport.Width / room.Tiles.GetLength(0)), 
        tilePosition.Y * (viewport.Height / room.Tiles.GetLength(1)));
    }
*/

    public void Update(Room room, ContentManager content) 
    {
        // update tile location
        //tilePosition = CoordinatesToTile(this.position, room);
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

    
        public GameObject CheckForCollision(Room currentRoom, int factorX, int factorY, bool attack)
        {
            BoundingBox boundingBox_1 =
                this.CreateBoundingBox(factorX, factorY);

            foreach (GameObject obj in currentRoom.passiveObjects)
            {
                BoundingBox boundingBox_2 = obj.CreateBoundingBox();

                if (boundingBox_1.Intersects(boundingBox_2))
                {
                    return obj;
                }
            }

            if (currentRoom.activeObjects.Count > 0)
            {
                foreach (Entity entity_2 in currentRoom.activeObjects)
                {
                    if (this != entity_2 && entity_2 != null)
                    {
                        BoundingBox boundingBox_2 = entity_2.CreateBoundingBox();

                        if (attack)
                        {
                            boundingBox_1.Min.X -= this.weapon.weaponRange;
                            boundingBox_1.Min.Y -= this.weapon.weaponRange;
                            boundingBox_1.Max.X += this.weapon.weaponRange;
                            boundingBox_1.Max.Y += this.weapon.weaponRange;
                        }
                        if (boundingBox_1.Intersects(boundingBox_2))
                        {
                            return entity_2;
                        }
                    }
                }
            }
            return null;
        }

        public GameObject CheckForItemCollision(Room currentRoom)
        {
            BoundingBox boundingBox_1 = this.CreateBoundingBox();

            foreach (GameObject item in currentRoom.items)
            {
                BoundingBox boundingBox_2 = item.CreateBoundingBox();

                if (boundingBox_1.Intersects(boundingBox_2))
                {
                    return item;
                }
            }
            return null;
        }

        public BoundingBox CreateBoundingBox(int factorX, int factorY)
        {
            return new BoundingBox(new Vector3(this.position.X +
                    this.movementSpeed * factorX -
                    (this.texture.Width / 2) +
                    10,
                    this.position.Y +
                    this.movementSpeed * factorY -
                    (this.texture.Height / 2),
                    0),
                new Vector3(this.position.X +
                    this.movementSpeed * factorX +
                    (this.texture.Width / 2) -
                    10,
                    this.position.Y +
                    this.movementSpeed * factorY +
                    (this.texture.Height / 2),
                    0));
        }
    }


