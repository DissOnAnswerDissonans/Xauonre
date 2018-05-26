using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class EntientYeast : Item
    {
        public EntientYeast()
        {
            Name = "Entient Yeast";
            Cost = 1960;//2450
            Tier = 3;
            HP = 612.5;
            E = 328.125;
            AD = 30.625;
            AP = 76.5625;
            A = 24.5;
            R = 24.5;
            HR = 30.625;
            ER = 49;

            Parts = new List<Item>
            {
                new AgedYeast(),
                new AgedYeast(),
            };
        }
    }
}
