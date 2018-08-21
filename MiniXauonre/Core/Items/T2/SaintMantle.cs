
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class SaintMantle : Item
    {
        public SaintMantle()
        {
            Name = "Saint Mantle";
            Tier = 2;
            Cost = 880;

            HR = 24;
            A = 13;
            R = 35;
            HP = 250;
            //830
            Parts = new List<Item>
            {
                new Resister(),
                new Bulker(),
                new Carapace(),
            };
        }
    }
}
