using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLike.Classes.Weapons
{
    internal class Bow : Weapon
    {
        public Bow(Texture2D weaponTexture)
        {
            weaponRange = 6;
            this.weaponTexture = weaponTexture;
        }
    }
}
