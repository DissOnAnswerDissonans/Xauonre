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
        };


        public BasicShop() => Items = set;
    }
}
