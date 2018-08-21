﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class Bulker : Item
    {
        public Bulker()
        {
            Name = "Bulker";
            Cost = 270;
            Tier = 1;
            HP = 150;
            HR = 9;
            A = 5;

            Parts = new List<Item>
            {
                new FlameCoast(),
                new Steel(),
            };
        }
    }
}
