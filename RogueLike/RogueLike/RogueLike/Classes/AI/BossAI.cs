using RogueLike.Classes.Weapons;

namespace RogueLike.Classes.AI
{
    public class BossAI : EnemyAI
    {
        private ArcherAI archerAI = new ArcherAI();
        private MeleeAI meleeAI = new MeleeAI();
        private Player player;

        public void Move(Enemy enemy, Room room, Tile destinationTile)
        {
            Boss boss = (Boss) enemy;

            // swap weapon        
            if((room.CalculateHeuristic(room.GetTileFromPos(boss.Position), boss.DestinationTile) > 300
                && boss.EquippedWeapon is not Bow)
                || room.CalculateHeuristic(room.GetTileFromPos(boss.Position), boss.DestinationTile) < 300
                && boss.EquippedWeapon is Bow)
            {
                Weapon temp = boss.EquippedWeapon;
                boss.EquippedWeapon = boss.SecondaryWeapon;
                boss.SecondaryWeapon = temp;
            }

            // swap behaviour
            if(this.player is null && boss.CheckForCollision(room, 0, 0, true, false) is Player player)
            {
                this.player = player;
            }
            if(this.player is not null)
            {
                if(room.CalculateHeuristic(room.GetTileFromPos(boss.Position), 
                    room.GetTileFromPos(this.player.Position)) < 320)
                {
                    meleeAI.Move(boss, room, destinationTile);
                }
                else if(room.CalculateHeuristic(room.GetTileFromPos(boss.Position), 
                    room.GetTileFromPos(this.player.Position)) > 320)
                {
                    archerAI.Move(boss, room, destinationTile);
                }
            }

            // spawn enemies
            if(boss.MinionCountdown == 0) boss.SummonMinions(room);
            else boss.MinionCountdown--;
        }

        public void UpdateDestination(Enemy enemy, Player player, Room room)
        {
            Boss boss = (Boss) enemy;
            boss.DestinationTile = room.GetTileFromPos(player.Position);
        }
    }
}
