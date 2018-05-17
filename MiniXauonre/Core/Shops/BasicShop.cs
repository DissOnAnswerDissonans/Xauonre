using MiniXauonre.Core.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Shops
{
    class BasicShop : Shop
    {
        public static List<Item> set = new List<Item>
        {
            new MagicStone(),
            new MagicRelic(),
            new Bablonomicon(),
            new SpellBook(),
            new Bulker(),
            new FlameCoast(),
            new Knife(),
            new MagicBoots(),
            new MagicWand(),
            new Razor(),
            new Steel(),
            new XPeke(),
        };


        public BasicShop() => Items = set;
    }
}
