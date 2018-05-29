using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class GiantArmor : Item
    {
        public const double CDRed = 0.5;
        public GiantArmor()
        {
            Name = "Giant Armor";
            Tier = 3;
            Cost = 1850;

            HP = 1500;
            A = 40;
            R = 40;
            HR = 50;

            Explanation = (h) => "Each time you get damage - Reduce CD of your skills by " + CDRed;
            Effect = new Perk
            {
                Name = this.Name,
                Explanation = this.Explanation,
                GetDamage = (f) => (d) =>
                {
                    foreach (var skill in d.HeroValue.Skills) skill.Tick(CDRed);
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
