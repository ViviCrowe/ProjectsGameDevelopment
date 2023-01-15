using System;

namespace RogueLike.Classes.AI
{
    public class PatrolAI : EnemyAI
    {
        private Tile startTile;

        public void Move(Enemy enemy, Room room, Tile destinationTile)
        {
            int x = (int) destinationTile.Position.X;
            int y = (int) destinationTile.Position.Y;

            if (
                x < room.GetTileFromPos(enemy.Position).Position.X &&
                enemy.CheckForCollision(room, 3, 0, false, false) == null &&
                y < room.GetTileFromPos(enemy.Position).Position.Y &&
                enemy.CheckForCollision(room, 0, 3, false, false) == null
            )
            {
                enemy.MoveUpLeft();
            }
            else if (
                x < room.GetTileFromPos(enemy.Position).Position.X &&
                enemy.CheckForCollision(room, 3, 0, false, false) == null &&
                y > room.GetTileFromPos(enemy.Position).Position.Y &&
                enemy.CheckForCollision(room, 0, 3, false, false) == null
            )
            {
                enemy.MoveDownLeft();
            }
            else if (
                x > room.GetTileFromPos(enemy.Position).Position.X &&
                enemy.CheckForCollision(room, -3, 0, false, false) == null &&
                y < room.GetTileFromPos(enemy.Position).Position.Y &&
                enemy.CheckForCollision(room, 0, 3, false, false) == null
            )
            {
                enemy.MoveUpRight();
            }
            else if (
                x > room.GetTileFromPos(enemy.Position).Position.X &&
                enemy.CheckForCollision(room, -3, 0, false, false) == null &&
                y > room.GetTileFromPos(enemy.Position).Position.Y &&
                enemy.CheckForCollision(room, 0, 3, false, false) == null
            )
            {
                enemy.MoveDownRight();
            }
            else if (
                x > room.GetTileFromPos(enemy.Position).Position.X + 2 &&
                enemy.CheckForCollision(room, 3, 0, false, false) == null
            )
            {
                enemy.MoveRight();
            }
            else if (
                x < room.GetTileFromPos(enemy.Position).Position.X &&
                enemy.CheckForCollision(room, -3, 0, false, false) == null
            )
            {
                enemy.MoveLeft();
            }
            else if (
                y > room.GetTileFromPos(enemy.Position).Position.Y + 2 &&
                enemy.CheckForCollision(room, 0, 3, false, false) == null
            )
            {
                enemy.MoveDown();
            }
            else if (
                y < room.GetTileFromPos(enemy.Position).Position.Y &&
                enemy.CheckForCollision(room, 0, -3, false, false) == null
            )
            {
                enemy.MoveUp();
            }
            else if (
                enemy.CheckForCollision(room, 0, 0, false, false) is
                Enemy enemy_2 // stuck
            )
            {
                enemy_2.MoveDown();
                enemy.MoveUp();
            }
        }

        public void UpdateDestination(Enemy enemy, Player player, Room room)
        {
            if (startTile == null)
            {
                this.startTile = room.GetTileFromPos(enemy.Position);
            }
            if (enemy.DestinationTile == null)
            {
                do
                {
                    Random random = new Random();
                    int row = (int) random.NextInt64(1, (int) room.GridDimensions.X - 2);
                    int col = (int) random.NextInt64(1, (int) room.GridDimensions.Y - 2);
                    enemy.DestinationTile = room.Tiles[row, col];
                }
                while (enemy.DestinationTile == startTile);
            }
            else if (
                room.GetTileFromPos(enemy.Position) == enemy.DestinationTile
            )
            {
                Tile temp = this.startTile;
                this.startTile = room.GetTileFromPos(enemy.Position);
                enemy.DestinationTile = temp;
            }
        }
    }
}
