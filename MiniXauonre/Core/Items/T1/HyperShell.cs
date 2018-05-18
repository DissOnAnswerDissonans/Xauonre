using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class HyperShell : Item
    {
        public HyperShell()
        {
            Name = "Hyper Shell";
            Cost = 750;

            HP = 470;
            A = 15;
            R = 15;
            HR = 10;


            //515
            Parts = new List<Item> {
                new Carapace(),
                new Leaven(),
                new RestoreRing(),
            };
        }
    }
}
