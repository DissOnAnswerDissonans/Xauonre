using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class Resister : Item
    {
        public Resister()
        {
            Name = "Resister";
            Cost = 300;
            Tier = 1;
            R = 20;
            HP = 100;

            Parts = new List<Item>
            {
                new Amulet(),
            };
        }
    }
}
