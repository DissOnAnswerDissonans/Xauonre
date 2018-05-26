using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class GiantArmor : Item
    {
        public GiantArmor()
        {
            Name = "Giant Armor";

            Cost = 1850;

            HP = 1500;
            A = 40;
            R = 40;
            HR = 50;


            Effect = new Perk
            {
                GetDamage = (f) => (d) =>
                {
                    foreach (var skill in d.HeroValue.Skills) skill.Tick(0.5);
                    return f(d);
                },
            };



            Parts = new List<Item>
            {
                new HyperShell(),
                new HyperShell(),
                new Leaven(),
                new FlameCoast(),
            };
        }
    }
}
