using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class AgedYeast : Item
    {
        public AgedYeast()
        {
            Name = "Aged Yeast";
            Cost = 700;//875
            Tier = 2;
            HP = 218.75;
            E = 117.1875;
            AD = 10.9375;
            AP = 27.34375;
            A = 8.75;
            R = 8.75;
            HR = 10.9375;
            ER = 17.5;

            Parts = new List<Item>
            {
                new OldYeast(),
                new OldYeast(),
            };
        }
    }
}
