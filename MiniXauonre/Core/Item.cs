using MiniXauonre.Core.Heroes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core
{
    class Item
    {
        public Perk Effect { get; protected set; }
        public string Name { get; protected set; }
        public Func<string> Explanation { get; protected set; }
        public double Cost { get; protected set; }
        public List<Item> Parts { get; protected set; }
        public Action<Hero> AddStats { get; protected set; }
        public Action<Hero> RemoveStats { get; protected set; }

        public Item()
        {
            Name = "None";
            Explanation = () => "None";
            Effect = new Perk() { };
            Cost = 0;
            Parts = new List<Item>();
            AddStats = (h) => { };
            RemoveStats = (h) => { };
        }

        public void Bought(Hero h)
        {
            h.AddMoney(-GetFinalCost(h));
            foreach(var part in Parts)
            {
                if (h.Items.Contains(part))
                    part.Remove(h);
            }
            Equip(h);
        }

        public double GetFinalCost(Hero h)
        {
            var resultCost = Cost;
            var gottenParts = h.Items.Where(i => Parts.Contains(i));
            var tempParts = new List<Item>(Parts);
            foreach(var i in gottenParts)
            {
                if (tempParts.Contains(i))
                {
                    tempParts.Remove(i);
                    resultCost -= i.Cost;
                }
            }
            return resultCost;
        }

        protected void Equip(Hero h)
        {
            if (!h.Perks.Contains(Effect))
                h.Perks.Add(Effect);
            h.Items.Add(this);
            AddStats(h);
        }

        protected void Remove(Hero h)
        {
            if(h.Items.Contains(this))
            {
                h.Items.Remove(this);
                RemoveStats(h);
                if (!h.Items.Contains(this))
                    h.Perks.Remove(Effect);
            }
        }


        public override bool Equals(object obj) => obj.GetType() == typeof(Item) && Name == (obj as Item).Name;

        public override int GetHashCode()
        {
            return 539060726 + EqualityComparer<string>.Default.GetHashCode(Name);
        }
    }
}
