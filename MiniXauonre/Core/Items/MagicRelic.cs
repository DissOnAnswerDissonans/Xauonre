using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class MagicRelic : Item
    {
        public const double AP = 50;
        public const double HP = 100;

        public MagicRelic()
        {
            Name = "Magic Relic";
            Explanation = () => "+" + AP + " AP\n+" + HP + " HP";
            Cost = 105;
            Parts = new List<Item>
            {
                new MagicRelic(),
                new MagicRelic(),
            };
            AddStats = (h) =>
            {
                h.AddMaxHp(HP);
                h.AddAbilityPower(AP);
            };
            RemoveStats = (h) =>
            {
                h.AddMaxHp(-HP);
                h.AddAbilityPower(-AP);
            };
        }
    }
}
