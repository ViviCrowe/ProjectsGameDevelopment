﻿using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLike.Classes.Weapons
{
    public abstract class Item
    {
        protected int attackDamage { get; set; }
        protected float attackSpeed { get; set; }
        protected int teethValue { get; set; }
        protected int weaponRange { get; set; }
        protected Texture2D weaponTexture { get; set; }


    }
}
