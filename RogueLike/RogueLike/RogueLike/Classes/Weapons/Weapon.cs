using Microsoft.Xna.Framework.Audio;

namespace RogueLike.Classes.Weapons
{
    /*
     * Setting and Getting within the concrete Class
     */
    public abstract class Weapon : GameObject
    {
        public int attackDamage { get; set; }
        public float attackSpeed { get; set; }
        public int teethValue { get; set; }
        public int weaponRange { get; set; }
        private SoundEffect soundEffect;

    }
}
