using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class Carapace : Item
    {
        public Carapace()
        {
            Name = "Carapace";
            Tier = 1;
            Cost = 260;
            A = 8;
            R = 8;
            HP = 8;
            //255
            Parts = new List<Item>
            {
                new RestoreRing(),
                new Steel(),
                new Amulet(),
            };
        }
    }
}
