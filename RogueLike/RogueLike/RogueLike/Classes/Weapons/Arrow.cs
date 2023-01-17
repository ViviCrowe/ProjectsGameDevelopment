using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueLike.Classes.Weapons
{
    internal class Arrow : GameObject
    {
        public new static Texture2D Texture { get; set; }

        public Vector2 Direction { get; set; }

        public Arrow(Vector2 position, Vector2 direction)
        {
            this.Position = position;
            this.Direction = direction;
        }

        public GameObject CheckForCollision(Room currentRoom)
        {
            BoundingBox boundingBox_1 = this.CreateBoundingBox();
            foreach (GameObject obj in currentRoom.passiveObjects)
            {
                BoundingBox boundingBox_2 = obj.CreateBoundingBox();
                if (boundingBox_1.Intersects(boundingBox_2))
                {
                    return obj;
                }
            }
            if (currentRoom.activeObjects.Count > 1)
            {
                foreach (Entity entity_2 in currentRoom.activeObjects)
                {
                    if (entity_2 != null)
                    {
                        BoundingBox boundingBox_2 = entity_2.CreateBoundingBox();
                        if (boundingBox_1.Intersects(boundingBox_2))
                        {
                            return entity_2;
                        }
                    }
                }
            }
            return null;
        }

        public new BoundingBox CreateBoundingBox()
        {
            return new BoundingBox(new Vector3(this.Position.X - (Arrow.Texture.Width / 2),
                    this.Position.Y - (Arrow.Texture.Height / 2),
                    0),
                new Vector3(this.Position.X + (Arrow.Texture.Width / 2),
                    this.Position.Y + (Arrow.Texture.Height / 2),
                    0));
        }
    }
}
