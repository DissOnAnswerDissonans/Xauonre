using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class InfinityEdge : Item
    {
        public InfinityEdge()
        {
            Name = "Infinity Edge";
            Tier = 3;
            Cost = 2100;

            AD = 200;
            HP = 400;
            A = 10;

            //1510
            Parts = new List<Item>
            {
                new Knife(),
                new Blade(),
                new Blade(),
                new Leach(),
            };
        }
    }
}
