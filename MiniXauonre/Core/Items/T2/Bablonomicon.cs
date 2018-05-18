using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class Bablonomicon : Item
    {
        public const double ExpAPScale = 10;

        public Bablonomicon()
        {
            Name = "Bablonomicon";
            Explanation = () => "";
            Cost = 250;
            AP = 50;

            Effect = new Perk
            {
                EndTurn = (a) => (d) =>
                {
                    d.PlayerValue.AllDamage += ExpAPScale * d.HeroValue.GetAbilityPower();
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
