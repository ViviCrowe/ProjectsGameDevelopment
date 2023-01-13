using RogueLike.Classes;
using RogueLike.Classes.Weapons;

public class ArcherAI : EnemyAI
{
    public void Move(Enemy enemy, Room room, Tile destinationTile)
    {
        int x = (int) destinationTile.position.X;
        int y = (int) destinationTile.position.Y;

        if(enemy.CheckForCollision(room, 0, 0, true, false) is Player player && room.CalculateHeuristic(room.GetTileFromPos(enemy.position), room.GetTileFromPos(player.position)) > 300)
        {
            if(enemy.weapon is Bow bow)
            {
                enemy.playerDirection = player.position - enemy.position;
                bow.FireArrow(enemy);
            }
        }
        else if(x < room.GetTileFromPos(enemy.position).position.X && enemy.CheckForCollision(room, 1, 0, false, false) == null
        && y < room.GetTileFromPos(enemy.position).position.Y && enemy.CheckForCollision(room, 0, 1, false, false) == null) 
        {
            enemy.MoveDownRight();
        }
        else if(x < room.GetTileFromPos(enemy.position).position.X && enemy.CheckForCollision(room, 1, 0, false, false) == null
        && y > room.GetTileFromPos(enemy.position).position.Y && enemy.CheckForCollision(room, 0, -1, false, false) == null)
        {
            enemy.MoveUpRight();
        }
        else if(x > room.GetTileFromPos(enemy.position).position.X && enemy.CheckForCollision(room, -1, 0, false, false) == null
        && y < room.GetTileFromPos(enemy.position).position.Y && enemy.CheckForCollision(room, 0, 1, false, false) == null)
        {
            enemy.MoveDownLeft();
        }
        else if(x > room.GetTileFromPos(enemy.position).position.X && enemy.CheckForCollision(room, -1, 0, false, false) == null
        && y > room.GetTileFromPos(enemy.position).position.Y && enemy.CheckForCollision(room, 0, -1, false, false) == null)
        {   
            enemy.MoveUpLeft();
        }
        else if(x < room.GetTileFromPos(enemy.position).position.X && enemy.CheckForCollision(room, 1, 0, false, false) == null)
        {
            enemy.MoveRight();
        }
        else if(x > room.GetTileFromPos(enemy.position).position.X && enemy.CheckForCollision(room, -1, 0, false, false) == null)
        {
            enemy.MoveLeft();
        }
        else if(y < room.GetTileFromPos(enemy.position).position.Y && enemy.CheckForCollision(room, 0, 1, false, false) == null)
        {
            enemy.MoveDown();
        }
        else if(y > room.GetTileFromPos(enemy.position).position.Y && enemy.CheckForCollision(room, 0, -1, false, false) == null)
        {
            enemy.MoveUp();
        }
        else if(enemy.CheckForCollision(room, 0, 0, false, false) is Enemy enemy_2) // stuck
        {
            enemy_2.MoveDownRight();
            enemy.MoveUpLeft();
        }
    }

    public void UpdateDestination(Enemy enemy, Player player, Room room)
    {
        enemy.destinationTile = room.GetTileFromPos(player.position);
    }
}
