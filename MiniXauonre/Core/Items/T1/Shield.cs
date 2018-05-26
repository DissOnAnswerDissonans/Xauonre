using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class Shield : Item
    {

        public const double PhysDamageReduction = 0.1;
        public Shield()
        {
            Name = "Shield";
            Cost = 220;
            Tier = 1;
            HP = 200;
            HR = 8;


            Explanation = () => "Reduce incoming phys damage by " + PhysDamageReduction + "%."; 
            Effect = new Perk
            {
                GetDamage = (f) => (d) =>
                {
                    d.DamageValue.Phys *= (1 - PhysDamageReduction);
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
