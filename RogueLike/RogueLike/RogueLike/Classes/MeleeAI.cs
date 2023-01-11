using RogueLike.Classes;

public class MeleeAI : EnemyAI
{
    public void Attack(Player player)
    {
        throw new System.NotImplementedException();
    }

    public void Move(Enemy enemy, Room room, Tile destinationTile)
    {
        // Vector2 destinationPos = room.GetPosFromTile(destinationTile);
        int x = 0;
        int y = 0;
        while(room.Tiles[x, y] != destinationTile) 
        {
            x++;
            y++;
        }
        if(x > enemy.tilePosition.X) 
        {
            enemy.MoveRight();
        }
        else if(x < enemy.tilePosition.X)
        {
            enemy.MoveLeft();
        }
        else if(y > enemy.tilePosition.Y)
        {
            enemy.MoveDown();
        }
        else if(y < enemy.tilePosition.Y)
        {
            enemy.MoveUp();
        }
        else
        {
            if(enemy.CheckForCollision(room, 0, 0, true) is Player)
            this.Attack((Player) enemy.CheckForCollision(room, 0, 0, true));
        }
    }
}
