using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLike.Classes.Abilities
{
    public class AktivAbility
    {
        private float castDuration;
        private int abilityDamage;
        private ConcreteAktiveAbility abilityType;
        private int abilityHealthPoints;

        private Texture2D abilityTexture;
        private SoundEffect abilitySoundEffect;

        public AktivAbility(Texture2D abilityTexture, SoundEffect abilitySoundEffects, ConcreteAktiveAbility abilityType)
        {
            this.abilityTexture = abilityTexture;
            this.abilitySoundEffect = abilitySoundEffects;
            this.abilityType = abilityType;
        }

        public void CastAbility()
        {
            if (abilityType == ConcreteAktiveAbility.FIREBALL)
            {
                Fireball();
            }
            else if (abilityType == ConcreteAktiveAbility.EARTHBALL)
            {
                Earthball();
            }
            else if (abilityType == ConcreteAktiveAbility.AIRBALL)
            {
                Airball();
            }
        }

        private void Fireball()
        {
            //TODO
        }

        private void Earthball()
        {
            //TODO
        }

        private void Airball()
        {
            //TODO
        }
    }
}
