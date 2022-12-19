using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLike.Classes.Weapons
{
    internal class FistWeapon : Weapon
    {
        public FistWeapon(Texture2D weaponTexture)
        {
            weaponRange = 0;
            this.weaponTexture = weaponTexture;
        }
    }
}
