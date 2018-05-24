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

            Cost = 800;



            E = 600;
            AP = 100;
            ER = 20;



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