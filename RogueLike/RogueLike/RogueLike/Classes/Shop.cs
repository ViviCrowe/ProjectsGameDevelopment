using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueLike.Classes
{
    //Generic Type I stands for Item
    public class Shop<I>
    {
        private I[] inventory;

        public Shop()
        {
            //TODO
        }
        public I GiveItem(int index)
        {
            //TODO
            return inventory[0];
        }

        public bool isOpen()
        {
            //TODO
            return false;
        }
    }
}
