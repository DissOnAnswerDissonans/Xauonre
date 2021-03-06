﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class MagicWand : Item
    {

        public const double SpellResist = 0.10;
        public MagicWand()
        {
            Name = "Magic Wand";
            Cost = 800;
            Tier = 2;
            Parts = new List<Item>
            {
                new MagicRelic(),
                new XPeke(),
            };


            Explanation = (h) => "Reduces incoming Spell damage by " + SpellResist * 100 + "%.";

            AD = 50;
            AP = 50;
            A = 15;
            HP = 150;

            
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
            

        }
    }
}
