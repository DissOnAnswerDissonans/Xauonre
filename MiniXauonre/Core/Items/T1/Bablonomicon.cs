﻿using MiniXauonre.Core.Heroes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class Bablonomicon : Item
    {
        public const double ExpAPScale = 0.4;

        public Bablonomicon()
        {
            Name = "Bablonomicon";
            Tier = 1;
            Explanation = (h) => "";
            Cost = 250;
            AP = 50;



            Explanation = (h) => "At the end of turn gives " + ExpAPScale * 100 + "% AP("+ h.GetAbilityPower()*ExpAPScale+") as EXP";

            Effect = new Perk
            {
                Name = this.Name,
                Explanation = this.Explanation,
                EndTurn = (a) => (d) =>
                {
                    var at = new Damage(d.HeroValue, d.HeroValue.P, pure: d.HeroValue.GetAbilityPower() * ExpAPScale);
                    var target = new Hero();
                    target.GetDamage(at);
                    return a(d);
                },
            };


            Parts = new List<Item>
            {
                new MagicStone(),
            };
        }
    }
}
