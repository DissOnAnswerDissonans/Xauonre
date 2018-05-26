using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class KingSword : Item
    {
        public KingSword()
        {
            Name = "King Sword";

            Cost = 600;

            HP = 150;
            AD = 55;

            //520
            Parts = new List<Item>
            {
                new Blade(),
                new Knife(),
            };
        }
    }
}
