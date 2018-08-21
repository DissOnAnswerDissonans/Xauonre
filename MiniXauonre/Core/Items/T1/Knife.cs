using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class Knife : Item
    {
        public Knife()
        {
            Name = "Knife";
            Tier = 1;
            Cost = 280;
            Parts = new List<Item>
            {
                new Razor(),
                new Razor(),
                new Razor(),
            };
            AD = 22;
            HP = 100;
        }
    }
}
