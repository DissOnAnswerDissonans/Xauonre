using MiniXauonre.Core.Heroes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core
{
    class Shop
    {
        public List<Item> Items { get; protected set; }

        public Shop()
        {
            Items = new List<Item>();
        }

        public bool Buy(Hero h, Item item)
        {
            var cost = item.GetFinalCost(h);
            if(!Items.Contains(item) || h.GetMoney() < cost)
                return false;
            item.Bought(h);
            return true;
        }
    }
}
