using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class MagicWand : Item
    {
        public MagicWand()
        {
            Name = "Magic Wand";
            Cost = 800;

            Parts = new List<Item>
            {
                new MagicRelic(),
                new XPeke(),
            };

            AD = 40;
            AP = 50;
            A = 5;
            HP = 150;

            //Conterspell
            /*
            Effect = new Perk
            {
                GetDamage = (f) => (d) =>
                {
                    d.DamageValue.Magic *= 0.98;
                    return d;
                },
            };
            */

        }
    }
}
