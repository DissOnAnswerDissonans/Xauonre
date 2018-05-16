using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class MagicStone : Item
    {
        public MagicStone()
        {
            Name = "Magic Stone";
            Explanation = () => "+"+AP+" AP\n";
            Cost = 80;
            AP = 20;
        }
    }
}
