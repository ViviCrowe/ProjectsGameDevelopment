namespace RogueLike.Classes.AI
{
    public interface EnemyAI
    {
        void Move(Enemy enemy, Room room, Tile destinationTile);

        void UpdateDestination(Enemy enemy, Player player, Room room);
    }
}