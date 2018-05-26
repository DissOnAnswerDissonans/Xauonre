using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class Minigun : Item
    {
        public Minigun()
        {
            Name = "Minigun";
            Cost = 2100;

            
            HP = 500;
            AD = 150;
            A = 30;
            AS = 1;

            //1240
            Parts = new List<Item>
            {
                new Booster(),
                new KingSword(),
                new Blade(),
            };
        }
    }
}
