using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class Accumulator : Item
    {
        public Accumulator()
        {
            Name = "Accumulator";

            Cost = 260;
            E = 300;
            AP = 20;
            ER = 10;

            //170
            Parts = new List<Item>
            {
                new Battery(),
                new SpellBook(),
                new SpellBook(),
           };
        }
    }
}