using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class OldYeast : Item
    {
        public OldYeast()
        {
            Name = "Old Yeast";
            Cost = 218;//312,5
            Tier = 1;
            HP = 78.125;
            E = 117.1875;
            AD = 3.90625;
            AP = 9.765625;
            A = 3.125;
            R = 3.125;
            HR = 3.90625;
            ER = 6.25;

            Parts = new List<Item>
            {
                new Yeast(),
                new Yeast(),
                new Yeast(),
            };
        }
    }
}
