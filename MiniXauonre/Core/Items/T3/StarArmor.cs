using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class StarArmor : Item
    {
        public const double SpellResist = 0.30;
        public StarArmor()
        {
            Name = "Star Armor";
            Tier = 3;
            Cost = 2600;

            HR = 30;
            A = 30;
            R = 80;
            HP = 700;
            AD = 50;
            AP = 50;

            Explanation = (h) => "Reduces incoming Spell damage by " + SpellResist * 100 + "%.";

            Effect = new Perk
            {
                Name = this.Name,
                Explanation = this.Explanation,
                GetDamage = (f) => (d) =>
                {
                    d.DamageValue.Magic *= (1 - SpellResist);
                    return f(d);
                },
            };

            //1780
            Parts = new List<Item>
            {
                new SaintMantle(),
                new Resister(), 
                new MagicWand(),
            };
        }
    }
}
