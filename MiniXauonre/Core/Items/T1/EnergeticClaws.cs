using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class EnergeticClaws : Item
    {
        public double Bonus = 3;
        public EnergeticClaws()
        {
            Name = "Energetic Claws";
            Tier = 1;
            Cost = 270;
            E = 110;
            AD = 15;
            HP = 50;


            Explanation = (h) => "At the End of your Turn you get "+Bonus+" AD forever.";

            Effect = new Perk()
            {
                Name = this.Name,
                Explanation = this.Explanation,
                EndTurn = (f) => (d) =>
                {
                    d.HeroValue.AddAttackDamage(Bonus);
                    return d;
                }
            };

            //190
            Parts = new List<Item>()
            {
                new Battery(),
                new Razor(),
                new Razor(),
            };
        }
    }
}
