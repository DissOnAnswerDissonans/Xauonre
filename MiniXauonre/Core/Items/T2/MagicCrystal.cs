using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class MagicCrystal : Item
    {
        public MagicCrystal()
        {
            Name = "Magic Crystal";
            Tier = 2;
            Cost = 800;

            E = 400;
            AP = 100;
            ER = 30;

            //420

            Parts = new List<Item>
            {
                new Accumulator(),
                new MagicStone(),
                new MagicStone(),
            };

        }

    }

}
