using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core.Perks
{
    class TestPerk2 : Perk
    {
        public TestPerk2()
        {
            GetMaxHp = (g) => () => g() + 10;
            SetMaxHp = (s) => (v) => s(v - 10);
            GetHp = (g) => () => g() + 10;
            SetHp = (s) => (v) => s(v - 10);
        }
    }
}
