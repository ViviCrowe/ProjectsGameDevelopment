using Microsoft.Xna.Framework.Graphics;

namespace RogueLike.Classes.Items
{
    public class Potion : GameObject
    {
        public int AdditionalHealth { get; }

        public Potion(int additionalHealth)
        {
            this.AdditionalHealth = additionalHealth;
        }
    }
}
