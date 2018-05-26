using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class XPeke : Item
    {
        public XPeke()
        {
            Name = "X Peke";
            Tier = 1;
            Cost = 230;
            Parts = new List<Item>
            {
                new Razor(),
                new Steel(),
            };
            AD = 15;
            A = 5;
            HP = 50;
        }
    }
}
