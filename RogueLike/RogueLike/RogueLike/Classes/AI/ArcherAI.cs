using RogueLike.Classes.Weapons;

namespace RogueLike.Classes.AI
{
    public class ArcherAI : EnemyAI
    {
        public void Move(Enemy enemy, Room room, Tile destinationTile)
        {
            int x = (int) destinationTile.Position.X;
            int y = (int) destinationTile.Position.Y;

            if (
                enemy.CheckForCollision(room, 0, 0, true, false) is
                Player player &&
                room
                    .CalculateHeuristic(room.GetTileFromPos(enemy.Position),
                    room.GetTileFromPos(player.Position)) >
                300
            )
            {
                if (enemy.EquippedWeapon is Bow bow)
                {
                    enemy.PlayerDirection = player.Position - enemy.Position;
                    bow.FireArrow (enemy);
                }
            }
            else if (
                x < room.GetTileFromPos(enemy.Position).Position.X &&
                enemy.CheckForCollision(room, 1, 0, false, false) == null &&
                y < room.GetTileFromPos(enemy.Position).Position.Y &&
                enemy.CheckForCollision(room, 0, 1, false, false) == null
            )
            {
                enemy.MoveDownRight();
            }
            else if (
                x < room.GetTileFromPos(enemy.Position).Position.X &&
                enemy.CheckForCollision(room, 1, 0, false, false) == null &&
                y > room.GetTileFromPos(enemy.Position).Position.Y &&
                enemy.CheckForCollision(room, 0, -1, false, false) == null
            )
            {
                enemy.MoveUpRight();
            }
            else if (
                x > room.GetTileFromPos(enemy.Position).Position.X &&
                enemy.CheckForCollision(room, -1, 0, false, false) == null &&
                y < room.GetTileFromPos(enemy.Position).Position.Y &&
                enemy.CheckForCollision(room, 0, 1, false, false) == null
            )
            {
                enemy.MoveDownLeft();
            }
            else if (
                x > room.GetTileFromPos(enemy.Position).Position.X &&
                enemy.CheckForCollision(room, -1, 0, false, false) == null &&
                y > room.GetTileFromPos(enemy.Position).Position.Y &&
                enemy.CheckForCollision(room, 0, -1, false, false) == null
            )
            {
                enemy.MoveUpLeft();
            }
            else if (
                x < room.GetTileFromPos(enemy.Position).Position.X &&
                enemy.CheckForCollision(room, 1, 0, false, false) == null
            )
            {
                enemy.MoveRight();
            }
            else if (
                x > room.GetTileFromPos(enemy.Position).Position.X &&
                enemy.CheckForCollision(room, -1, 0, false, false) == null
            )
            {
                enemy.MoveLeft();
            }
            else if (
                y < room.GetTileFromPos(enemy.Position).Position.Y &&
                enemy.CheckForCollision(room, 0, 1, false, false) == null
            )
            {
                enemy.MoveDown();
            }
            else if (
                y > room.GetTileFromPos(enemy.Position).Position.Y &&
                enemy.CheckForCollision(room, 0, -1, false, false) == null
            )
            {
                enemy.MoveUp();
            }
            else if (
                enemy.CheckForCollision(room, 0, 0, false, false) is
                Enemy enemy_2 // stuck
            )
            {
                enemy_2.MoveDownRight();
                enemy.MoveUpLeft();
            }
        }

        public void UpdateDestination(Enemy enemy, Player player, Room room)
        {
            enemy.DestinationTile = room.GetTileFromPos(player.Position);
        }
    }
}
