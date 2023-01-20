using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
    public int Experience { get; set; }
    public int Level { get; set; }
    public int LevelUpAt = 200;
    private SoundEffect drinkPotionSound;
    private SoundEffect levelUpSound;
    public int BaseAttack { get; set; } = 0;
    public int BaseDefense { get; set; } = 0;
    public bool HasKey { get; set; } = false;

    public Player(Viewport viewport, Weapon weapon) :
        base(viewport, weapon)
    {
        this.Position = new Vector2(viewport.Width / 2, viewport.Height / 2);
        this.CurrentHealth = MaximumHealth = 800;
        this.Teeth = new(0);
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
        if(this.Experience >= this.LevelUpAt) this.LevelUp();
    } 

    public void LevelUp()
    {
        this.MaximumHealth += 100;
        this.Level++;
        this.LevelUpAt = this.Experience + 200;
        levelUpSound.Play();
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
                    if (newWeapon is Sword)this.LoadAssets(content, "Player_with_Sword");
                    else if (newWeapon is Bow) this.LoadAssets(content, "Player_with_Bow");
                    else if (newWeapon is Spear) this.LoadAssets(content, "Player_with_Spear");
                player.pickUpWeaponCountdown = 40;
                PickupDropSound.Play();
            }
            else if (item is Wallet wallet)
            {
                this.UpdateTeethValue(wallet.Value);
                room.items.Remove (wallet);
            }
            else if(item is Potion potion)
            {
                switch(potion.Type) 
                {
                    case Potion.PotionType.HEALING: 
                    if(this.CurrentHealth + potion.Value > this.MaximumHealth) 
                        this.CurrentHealth = this.MaximumHealth;
                    else
                        this.CurrentHealth += potion.Value;
                    break;
                    case Potion.PotionType.ATTACK: item.LoadAssets(content, "purple_potion"); 
                    this.BaseAttack += potion.Value;
                    break;
                    case Potion.PotionType.DEFENSE: item.LoadAssets(content, "green_potion"); 
                    this.BaseDefense += potion.Value;
                    break;
                }
                room.items.Remove(potion);
                drinkPotionSound.Play();
            }
            else if(item is Key key)
            {
                this.HasKey = true;
                room.items.Remove(key);
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

    public new void LoadAssets(ContentManager content)
    {
        base.LoadAssets(content, "Player_no_weapon");
        levelUpSound = content.Load<SoundEffect>("level_up");
        drinkPotionSound = content.Load<SoundEffect>("drink_potion");
    }
}}