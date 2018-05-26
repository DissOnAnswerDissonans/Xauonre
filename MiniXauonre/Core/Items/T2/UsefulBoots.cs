using MiniXauonre.Core.Heroes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class UsefulBoots : Item
    {
        public const double PhysDamageReduction = 0.1;
        public const double ExpDamageReductionScale = 2;
        public UsefulBoots()
        {
            Name = "Useful Boots";
            Tier = 2;
            Cost = 750;

            MS = 2;
            HP = 200;
            HR = 8;
            AP = 50;


            Effect = new Perk
            {
                GetDamage = (f) => (d) =>
                {
                    var k = d.DamageValue.Phys * (1 - PhysDamageReduction);
                    d.DamageValue.Phys *= k;
                    var at = new Damage(d.HeroValue, d.HeroValue.P, pure: k);
                    var target = new Hero();
                    target.GetDamage(at);
                    return f(d);
                },
            };


            //670
            Parts = new List<Item>
            {
                new Boots(),
                new Shield(),
                new Bablonomicon(),
            };
        }
    }
}
