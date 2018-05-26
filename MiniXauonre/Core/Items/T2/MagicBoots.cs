using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class MagicBoots : Item
    {
        public MagicBoots()
        {
            Name = "Magic Boots";
            Tier = 2;
            Explanation = (h) => "";
            Cost = 680;
            AP = 100;
            HP = 200;
            MS = 1;

            //520
            Parts = new List<Item>
            {
                new Boots(),
                new MagicRelic(),
                new MagicStone(),
            };
        }
    }
}
