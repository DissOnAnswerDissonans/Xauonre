using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Items
{
    class SpellBook : Item
    {

        public SpellBook()
        {
            Name = "Spell Book";
            Tier = 0;
            Cost = 60;
            E = 50;
            ER = 2;
            AP = 10;
        }
    }
}
