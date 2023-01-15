namespace RogueLike.Classes.Items
{
    public class Wallet : GameObject
    {
        public int Value { get; set; }

        public Wallet(int value)
        {
            this.Value = value;
        }
    }
}
