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
            HP = 250;
            HR = 8;
            AP = 50;

            Explanation = (h) => "Reduce incoming phys damage by " + PhysDamageReduction * 100 + "% and gives " 
            + ExpDamageReductionScale*100+ "% of it as EXP";
            Effect = new Perk
            {
                Name = this.Name,
                Explanation = this.Explanation,
                GetDamage = (f) => (d) =>
                {
                    var k = d.DamageValue.Phys * PhysDamageReduction;
                    d.DamageValue.Phys -= k;
                    var at = new Damage(d.HeroValue, d.HeroValue.P, pure: k*ExpDamageReductionScale);
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
