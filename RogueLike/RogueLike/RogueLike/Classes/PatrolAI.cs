using System;
using RogueLike.Classes;

public class PatrolAI : EnemyAI
{
    private Tile startTile;

    public void Move(Enemy enemy, Room room, Tile destinationTile)
    {
        int x = (int) destinationTile.position.X;
        int y = (int) destinationTile.position.Y;

        if(x < room.GetTileFromPos(enemy.position).position.X && enemy.CheckForCollision(room, 3, 0, false, false) == null
        && y < room.GetTileFromPos(enemy.position).position.Y && enemy.CheckForCollision(room, 0, 3, false, false) == null) 
        {
            enemy.MoveUpLeft();
        }
        else if(x < room.GetTileFromPos(enemy.position).position.X && enemy.CheckForCollision(room, 3, 0, false, false) == null
        && y > room.GetTileFromPos(enemy.position).position.Y && enemy.CheckForCollision(room, 0, 3, false, false) == null)
        {
            enemy.MoveDownLeft();
        }
        else if(x > room.GetTileFromPos(enemy.position).position.X && enemy.CheckForCollision(room, -3, 0, false, false) == null
        && y < room.GetTileFromPos(enemy.position).position.Y && enemy.CheckForCollision(room, 0, 3, false, false) == null)
        {
            enemy.MoveUpRight();
        }
        else if(x > room.GetTileFromPos(enemy.position).position.X && enemy.CheckForCollision(room, -3, 0, false, false) == null
        && y > room.GetTileFromPos(enemy.position).position.Y && enemy.CheckForCollision(room, 0, 3, false, false) == null)
        {   
            enemy.MoveDownRight();
        }
        else if(x > room.GetTileFromPos(enemy.position).position.X + 2 && enemy.CheckForCollision(room, 3, 0, false, false) == null) 
        {
            enemy.MoveRight();
        }
        else if(x < room.GetTileFromPos(enemy.position).position.X && enemy.CheckForCollision(room, -3, 0, false, false) == null)
        {
            enemy.MoveLeft();
        }
        else if(y > room.GetTileFromPos(enemy.position).position.Y + 2 && enemy.CheckForCollision(room, 0, 3, false, false) == null)
        {
            enemy.MoveDown();
        }
        else if(y < room.GetTileFromPos(enemy.position).position.Y && enemy.CheckForCollision(room, 0, -3, false, false) == null)
        {
            enemy.MoveUp();
        }
        else if(enemy.CheckForCollision(room, 0, 0, false, false) is Enemy enemy_2) // stuck
        {
            enemy_2.MoveDown();
            enemy.MoveUp();
        }
        
    }

    public void UpdateDestination(Enemy enemy, Player player, Room room)
    {
        if(startTile == null)
        {
            this.startTile = room.GetTileFromPos(enemy.position);
        }
        if(enemy.destinationTile == null) { 
            do {
                Random random = new Random();
                int row = (int) random.NextInt64(1, (int) room.gridDimensions.X - 2);
                int col = (int) random.NextInt64(1, (int) room.gridDimensions.Y - 2);
                enemy.destinationTile = room.Tiles[row, col];
            } while(enemy.destinationTile == startTile);
        }
        else if(room.GetTileFromPos(enemy.position) == enemy.destinationTile)
        {
            Tile temp = this.startTile;
            this.startTile = room.GetTileFromPos(enemy.position);
            enemy.destinationTile = temp;
        }
    }   
}
