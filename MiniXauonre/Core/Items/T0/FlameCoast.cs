using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class FlameCoast : Item
    {
        public FlameCoast()
        {
            Name = "Flame Coast";
            Tier = 0;
            Cost = 90;
            HP = 200;


            //Conterspell

            Explanation = (h) => "";


            /*
            Effect = new Perk
            {

                GetDamage = (f) => (d) =>
                {
                    
                    var conter = new Damage(d.DamageValue.Creator, d.HeroValue.P, pure: Math.Min(d.DamageValue.Magic, 10));
                    d.DamageValue.Creator.GetDamage(conter);
                    return f(d);
                },
            };
            */
        }
    }
}
