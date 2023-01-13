using RogueLike.Classes;

public class TankAI : EnemyAI
{
    public void Move(Enemy enemy, Room room, Tile destinationTile)
    {
        int x = (int) destinationTile.position.X;
        int y = (int) destinationTile.position.Y;

        if(x < room.GetTileFromPos(enemy.position).position.X && enemy.CheckForCollision(room, 1, 0, false, false) == null
        && y < room.GetTileFromPos(enemy.position).position.Y && enemy.CheckForCollision(room, 0, 1, false, false) == null) 
        {
            enemy.MoveUpLeft();
        }
        else if(x < room.GetTileFromPos(enemy.position).position.X && enemy.CheckForCollision(room, 1, 0, false, false) == null
        && y > room.GetTileFromPos(enemy.position).position.Y && enemy.CheckForCollision(room, 0, 1, false, false) == null)
        {
            enemy.MoveDownLeft();
        }
        else if(x > room.GetTileFromPos(enemy.position).position.X && enemy.CheckForCollision(room, -1, 0, false, false) == null
        && y < room.GetTileFromPos(enemy.position).position.Y && enemy.CheckForCollision(room, 0, 1, false, false) == null)
        {
            enemy.MoveUpRight();
        }
        else if(x > room.GetTileFromPos(enemy.position).position.X && enemy.CheckForCollision(room, -1, 0, false, false) == null
        && y > room.GetTileFromPos(enemy.position).position.Y && enemy.CheckForCollision(room, 0, 1, false, false) == null)
        {   
            enemy.MoveDownRight();
        }
        else if(x > room.GetTileFromPos(enemy.position).position.X && enemy.CheckForCollision(room, 5, 0, false, false) == null) 
        {
            enemy.MoveRight();
        }
        else if(x < room.GetTileFromPos(enemy.position).position.X && enemy.CheckForCollision(room, -5, 0, false, false) == null)
        {
            enemy.MoveLeft();
        }
        else if(y > room.GetTileFromPos(enemy.position).position.Y && enemy.CheckForCollision(room, 0, 5, false, false) == null)
        {
            enemy.MoveDown();
        }
        else if(y < room.GetTileFromPos(enemy.position).position.Y && enemy.CheckForCollision(room, 0, -5, false, false) == null)
        {
            enemy.MoveUp();
        }
        else if(enemy.CheckForCollision(room, 0, 0, true, false) is Player player)
        {
                enemy.Attack(player);
        }
        else if(enemy.CheckForCollision(room, 0, 0, false, false) is Enemy enemy_2) // stuck
        {
            enemy_2.MoveDown();
            enemy.MoveUp();
        }
    }

    public void UpdateDestination(Enemy enemy, Player player, Room room)
    {
        enemy.destinationTile = null;

        if(room.activeObjects.Count > 1) 
        {
            foreach(Entity temp in room.activeObjects)
            {
                if(temp != null && temp != enemy && temp is Enemy && ((Enemy) temp).type != Enemy.Type.TANK)
                {
                    enemy.destinationTile = room.GetTileFromPos(temp.position);
                }
            }
        }
        if(enemy.destinationTile == null)
        {
            enemy.destinationTile = room.GetTileFromPos(player.position);
        }
    }
}
