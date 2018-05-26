using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class KillerBoots : Item
    {
        public KillerBoots()
        {
            Name = "Killer Boots";
            Tier = 2;
            Cost = 700;
            MS = 1;
            AS = 1;
            AD = 10;
            HP = 100;

            //670
            Parts = new List<Item>
            {
                new Razor(),
                new Booster(),
                new Boots(),
            };
        }
    }
}
