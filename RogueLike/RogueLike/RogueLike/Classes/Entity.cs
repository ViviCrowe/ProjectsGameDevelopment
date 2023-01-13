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

    public Weapon weapon;

    public Wallet teeth;
    public int visionRange;
    public enum Direction
    {
        DOWN,
        UP,
        LEFT,
        RIGHT,
        DOWNLEFT,
        DOWNRIGHT,
        UPLEFT,
        UPRIGHT

    }
    public Direction directionFacing;

    public Entity(Viewport viewport, Weapon weapon) 
    {
        Entity.viewport = viewport;
        this.weapon = weapon;
    }

    public Entity() 
    {
    }

    public int Attack(Entity target)
    {
        int damageDealt = this.weapon.attackDamage;
        System.Console.WriteLine(damageDealt);
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
        if (this.teeth.value > 0 && this.teeth != null)
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
        this.directionFacing = Direction.DOWN;
    }

    public void MoveLeft()
    {
        position.X -= movementSpeed;
        this.directionFacing = Direction.LEFT;
    }

    public void MoveRight()
    {
        position.X += movementSpeed;
        this.directionFacing = Direction.RIGHT;
    }

    public void MoveUp()
    {
        position.Y -= movementSpeed;
        this.directionFacing = Direction.UP;
    }

    public void MoveUpRight()
    {
        position.X += movementSpeed;
        position.Y -= movementSpeed;
        this.directionFacing = Direction.UPRIGHT;
    }

    public void MoveUpLeft()
    {
        position.X -= movementSpeed;
        position.Y -= movementSpeed;
        this.directionFacing = Direction.UPLEFT;
    }

    public void MoveDownRight()
    {
        position.X += movementSpeed;
        position.Y += movementSpeed;
        this.directionFacing = Direction.DOWNRIGHT;
    }

    public void MoveDownLeft()
    {
        position.X -= movementSpeed;
        position.Y += movementSpeed;
        this.directionFacing = Direction.DOWNLEFT;
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
            else if(item is Potion potion)
            {
                this.currentHealth += potion.additionalHealth;
                room.items.Remove(potion);
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

    public void Update(Room room)
    {
        if(this.weapon is Bow bow)
        {
            bow.UpdateArrows(this, room);
        }
    }

 
    public GameObject CheckForCollision(Room currentRoom, int factorX, int factorY, bool attack, bool vision)
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
        if (currentRoom.activeObjects.Count > 1)
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
                    if(vision)
                    {
                        boundingBox_1.Min.X -= this.visionRange;
                        boundingBox_1.Min.Y -= this.visionRange;
                        boundingBox_1.Max.X += this.visionRange;
                        boundingBox_1.Max.Y += this.visionRange;
                    }
                    if (boundingBox_1.Intersects(boundingBox_2) && vision == false)
                    {
                        return entity_2;
                    }
                    if (boundingBox_1.Intersects(boundingBox_2) && vision == true) {
                        if(entity_2 is Player) return entity_2;
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

        public new void LoadAssets(ContentManager content, string name)
        {
            texture = content.Load<Texture2D>(name);
            if(this.weapon is Bow bow)
            {
                bow.LoadAssets(content, "bow"); // bla idk
            }
            else if(this.weapon != null)
            {
                //this.weapon.LoadAssets(content, "........"); // character with specific weapon = change entity texture!
            }
        }

        public new void Draw(SpriteBatch spriteBatch, float layerDepth)
        {
            spriteBatch.Draw(texture,position,null,Color.White,0f,new Vector2(texture.Width / 2f, texture.Height / 2f),1.0f,SpriteEffects.None,layerDepth);
            if(this.weapon is Bow bow)
            {
                ((Bow)bow).Draw(spriteBatch); // depth richtig?
            }
        }
}


