using Microsoft.Xna.Framework.Graphics;

namespace RogueLike.Classes.Items
{
    public class Wallet : GameObject
    {
        public int Value { get; set; }
        public new static Texture2D Texture { get; set; }

        public Wallet(int value)
        {
            this.Value = value;
        }
    }
}
