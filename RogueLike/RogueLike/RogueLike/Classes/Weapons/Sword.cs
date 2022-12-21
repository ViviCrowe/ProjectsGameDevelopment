using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLike.Classes.Weapons
{
    public class Sword : Weapon
    {
        public Sword() { }
        public Sword(Texture2D weaponTexture)
        {
            weaponRange = 2;
            this.weaponTexture = weaponTexture;
        }
    }
}
