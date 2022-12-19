using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLike.Classes.Weapons
{
    public class Spear : Weapon
    {
        public Spear(Texture2D weaponTexture)
        {
            weaponRange = 3;
            this.weaponTexture = weaponTexture;
        }
    }
}
