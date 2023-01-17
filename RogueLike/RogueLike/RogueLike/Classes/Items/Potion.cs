using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueLike.Classes.Items
{
    public class Potion : GameObject
    {
        public int Value { get; }

        public enum PotionType
        {
            HEALING,
            ATTACK,
            DEFENSE
        }

        public PotionType Type { get; }

        public Potion(int value, Vector2 position, PotionType type)
        {
            this.Value = value;
            this.Type = type;
            this.Position = position;
        }
    }
}
