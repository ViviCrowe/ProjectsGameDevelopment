using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLike.Classes.Abilities
{
    public class PassivAbility
    {
        private float attackSpeed;
        private int healthPoits;
        private int attackDamage;
        private float movementSpeed;

        private Player player;
        public PassivAbility(Player player)
        {
            this.player = player;
        }

        public void AddToPlayerStats()
        {
            //TODO
        }

        public void RemoveFromPlayerStats()
        {
            //TODO
        }
    }
}
