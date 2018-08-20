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

            E = 100;
            HP = 300;
            HR = 34;
            R = 8;

            Explanation = (h) =>
                "Each time you get damage restore your energy by " + EnergyRestore * 100 + "% of damage.";

            Effect = new Perk
            {
                Name = this.Name,
                Explanation = this.Explanation,
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
