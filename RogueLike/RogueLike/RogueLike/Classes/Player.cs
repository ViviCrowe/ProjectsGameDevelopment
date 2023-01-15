using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RogueLike.Classes.Abilities;
using RogueLike.Classes.Items;
using RogueLike.Classes.Weapons;

namespace RogueLike.Classes
{
public class Player : Entity
{
    public AktivAbility aktivAbility { get; set; }
    private int pickUpWeaponCountdown = 0;
    public Player(Viewport viewport, Weapon weapon) :
        base(viewport, weapon)
    {
        this.Position = new Vector2(viewport.Width / 2, viewport.Height / 2);
        this.CurrentHealth = MaximumHealth = 800;
        this.Teeth = new(0);
        //this.weapon = new Fist();
        this.EquippedWeapon = new Bow(this); // TEST
        this.MovementSpeed = 5f;
    }

    public void Buy(GameObject item)
    {
        // TODO
    }

    public new void Update(Room room)
    {
        base.Update(room);
        if(pickUpWeaponCountdown > 0) pickUpWeaponCountdown--;
    }

    public void PickUpItem(GameObject item, Room room, ContentManager content)
    {
        if (item != null)
        {
            if (item is Weapon newWeapon && this is Player player && player.pickUpWeaponCountdown == 0)
            {
                if(this.EquippedWeapon is not Fist) DropWeapon(room, content);
                this.EquippedWeapon = newWeapon;
                room.items.Remove (newWeapon);
                if(newWeapon is Sword)
                {}
                else if(newWeapon is Bow)
                {}
                else if(newWeapon is Spear)
                {}
                this.LoadAssets(content, "character_with_sword"); // TESTWEISE 
                player.pickUpWeaponCountdown = 40;
            }
            else if (item is Wallet wallet)
            {
                this.UpdateTeethValue(wallet.Value);
                room.items.Remove (wallet);
            }
            else if(item is Potion potion)
            {
                if(this.CurrentHealth + potion.AdditionalHealth > this.MaximumHealth)
                {
                    this.CurrentHealth = this.MaximumHealth;
                }
                else
                {
                    this.CurrentHealth += potion.AdditionalHealth;
                }
                room.items.Remove(potion);
            }
        }
    }

    public void Sell(GameObject item)
    {
        // TODO
    }

    public void UpdateTeethValue(int TeethValue)
    {
        if ((this.Teeth.Value + TeethValue) >= 0)
        {
            this.Teeth.Value += TeethValue;
        }
        else
        {
            this.Teeth.Value = 0;
        }
    }
    
    public void UseActiveAbility()
    {
        // TODO
    }

}}