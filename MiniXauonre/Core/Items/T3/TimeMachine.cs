﻿using System;
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
            Tier = 3;
            Cost = 2200;

            HP = 200;
            AP = 250;
            E = 200;
            ER = 45;
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
