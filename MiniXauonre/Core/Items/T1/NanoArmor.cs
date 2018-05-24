using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class NanoArmor : Item
    {
        public NanoArmor()
        {
            Name = "Nano Armor";

            Cost = 620;

            E = 150;
            HP = 550;
            HR = 17;
            R = 8;

            Effect = new Perk
            {
                GetDamage = (f) => (d) =>
                {
                    var gotten = d.DamageValue.Sum() * 0.1;
                    d.HeroValue.AddEnergy(gotten);
                    return f(d);
                },
            };


            
            Parts = new List<Item>
            {
                new Battery(),
                new Amulet(),
                new Bulker(),
            };
        }
    }
}
