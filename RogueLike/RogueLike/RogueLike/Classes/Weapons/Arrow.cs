using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueLike.Classes;

public class Arrow : GameObject
{
    public static Texture2D texture;
    public Vector2 direction;
    public Arrow(Vector2 position, Vector2 direction)
    {   
        this.position = position;
        this.direction = direction;
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

    public new BoundingBox CreateBoundingBox() {
        return new BoundingBox(new Vector3(this.position.X -
            (Arrow.texture.Width / 2),
            this.position.Y - (Arrow.texture.Height / 2),
            0),
        new Vector3(this.position.X + (Arrow.texture.Width / 2),
            this.position.Y + (Arrow.texture.Height / 2),
            0));
    }
}