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
            Explanation = () => "";
            Cost = 60;
            E = 50;
            ER = 0.5;
            AP = 10;
        }
    }
}
