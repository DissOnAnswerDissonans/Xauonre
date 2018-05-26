using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class Simplex : Item
    {
        public Simplex()
        {
            Name = "Simplex";
            Tier = 2;

            AP = 128;
            E = 50;
            HP = 300;
            ER = 20;

            Cost = 720;
            
            //540
            Parts = new List<Item>()
            {
                new MagicRelic(),
                new MagicRelic(),
                new SpellBook(),
            };
        }
    }
}