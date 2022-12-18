public class Weapon : GameObject
{
    // sword, bow, spear, fistWeapon
    public int weaponDamage;

    private float weaponSpeed;

    private int teethValue;

    public float weaponRange;

    public Weapon()
    {
        this.weaponDamage = 5;
        this.weaponRange = 10;
    }
}
