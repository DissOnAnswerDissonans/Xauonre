using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class MagicRelic : Item
    {
        public MagicRelic()
        {
            Name = "Magic Relic";
            Explanation = () => "+" + AP + " AP\n+" + HP + " HP";
            Cost = 240;
            AP = 50;
            HP = 100;
            Parts = new List<Item>
            {
                new MagicStone(),
                new MagicStone(),
                new MagicStone(),
            };
        }
    }
}
