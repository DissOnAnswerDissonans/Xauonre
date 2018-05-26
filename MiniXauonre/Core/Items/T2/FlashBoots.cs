using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class FlashBoots : Item
    {
        public FlashBoots()
        {
            Name = "Flash Boots";
            Tier = 2;
            Cost = 700; //800
            MS = 3;
            HR = 20;

            //280
            Parts = new List<Item>
            {
                new Boots(),
                new RestoreRing(),
            };
        }
    }
}
