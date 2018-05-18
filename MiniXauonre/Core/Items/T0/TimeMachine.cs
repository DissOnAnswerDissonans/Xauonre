using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class TimeMachine : Item
    {
        public TimeMachine()
        {
            Name = "Time Machine";

            Cost = 2000;

            HP = 200;
            AP = 225;
            E = 100;
            ER = 15;
            MS = 2;
            CDR = 2;

            //1300
            Parts = new List<Item>
            {
                new Accelerator(),
                new Accelerator(),
                new MagicBoots(),
            };
        }
    }
}
