using RogueLike.Classes;

public interface EnemyAI
{
    void Attack(Player player);

    void Move(Enemy enemy, Room room, Tile destinationTile);
}
