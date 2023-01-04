namespace RogueLike.Classes.Weapons
{
    /*
     * Setteing and Getting within the concrete Class
     */
    public abstract class Weapon : GameObject
    {
        public int attackDamage { get; set; }
        public float attackSpeed { get; set; }
        public int teethValue { get; set; }
        public int weaponRange { get; set; }

    }
}
