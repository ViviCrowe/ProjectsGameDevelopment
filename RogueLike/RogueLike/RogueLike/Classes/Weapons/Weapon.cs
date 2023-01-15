using Microsoft.Xna.Framework.Audio;

namespace RogueLike.Classes.Weapons
{
    /*
     * Setting and Getting within the concrete Class
     */
    public abstract class Weapon : GameObject
    {
        public int AttackDamage { get; set; }
        public int TeethValue { get; set; }
        public int WeaponRange { get; set; }
        private SoundEffect soundEffect;

    }
}
