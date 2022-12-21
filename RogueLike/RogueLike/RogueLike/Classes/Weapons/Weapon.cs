using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLike.Classes.Weapons
{
    /*
     * Setteing and Getting within the concreat Class
     */
    public abstract class Weapon : GameObject
    {
        public int attackDamage { get; set; }
        public float attackSpeed { get; set; }
        public int teethValue { get; set; }
        public int weaponRange { get; set; }
        public Texture2D weaponTexture { get; set; }

    }
}
