using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class NanoArmor : Item
    {
        public const double EnergyRestore = 0.1;

        public NanoArmor()
        {
            Name = "Nano Armor";
            Tier = 2;
            Cost = 620;

            E = 150;
            HP = 550;
            HR = 17;
            R = 8;

            Explanation = (h) =>
                "Each time you get damage restore your energy by " + EnergyRestore * 100 + "% of damage.";

            Effect = new Perk
            {
                GetDamage = (f) => (d) =>
                {
                    var gotten = d.DamageValue.Sum() * EnergyRestore;
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
