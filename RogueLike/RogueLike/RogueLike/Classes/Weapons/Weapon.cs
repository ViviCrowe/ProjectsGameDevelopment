using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace RogueLike.Classes.Weapons
{
    /*
     * Setting and Getting within the concrete Class
     */
    public abstract class Weapon : GameObject
    {
        public int AttackDamage { get; set; }
        public int WeaponRange { get; set; }
    }
}
