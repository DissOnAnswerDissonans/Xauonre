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
            Cost = 225;
            A = 10;
            R = 10;
            HP = 20;

            Parts = new List<Item>
            {
                new Steel(),
                new Amulet(),
            };
        }
    }
}
