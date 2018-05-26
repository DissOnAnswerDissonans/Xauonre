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

        public List<Item> GetUpgrades(Item item) => Items.Where(i => i.Parts.Contains(item))
            .OrderBy(i => Tuple.Create(i.Tier, i.Cost)).ToList();

        public List<Item> GetItemsWithStat(StatType stat) => Items.Where(i => i.Stats.ContainsKey(stat))
            .OrderBy(i => Tuple.Create(i.Tier, i.Cost)).ToList();

        public List<Item> GetItemsWithTier(int tier) => Items.Where(i => i.Tier == tier).OrderBy(i => i.Cost).ToList();
    }
}
