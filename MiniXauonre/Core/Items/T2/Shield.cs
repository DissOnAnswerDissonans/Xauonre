using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class Shield : Item
    {
        public Shield()
        {
            Name = "Shield";
            Cost = 220;

            HP = 200;
            HR = 8;

            Effect = new Perk
            {
                GetDamage = (f) => (d) =>
                {
                    d.DamageValue.Phys *= 0.90;
                    return f(d);
                },
            };


            //170
            Parts = new List<Item>
            {
                new RestoreRing(),
                new FlameCoast(),
            };
        }
    }
}
