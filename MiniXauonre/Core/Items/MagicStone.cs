using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class MagicStone : Item
    {
        public const double AP = 20;
        public MagicStone()
        {
            Name = "Magic Stone";
            Explanation = () => "+"+AP+" AP\n";
            Cost = 40;
            AddStats = (h) => h.AddAbilityPower(AP);
            RemoveStats = (h) => h.AddAbilityPower(-AP);
        }
    }
}
