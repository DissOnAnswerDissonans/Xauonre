using MiniXauonre.Controller;
using MiniXauonre.Core.Heroes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core
{
    class FuncData
    {
        public Shop ShopValue { get; set; }
        public Hero HeroValue { get; set; }
        public double DoubleValue { get; set; }
        public bool BoolValue { get; set; }
        public Damage DamageValue { get; set; }
        public Map MapValue { get; set; }
        public Player PlayerValue { get; set; }

        public FuncData(Hero hV, double dV = 0, bool bV = false, Shop sV = null,  Damage dmgV = null, Map mapvalue = null, Player playerValue = null) {
            HeroValue = hV;
            DoubleValue = dV;
            BoolValue = bV;
            DamageValue = dmgV;
            MapValue = mapvalue;
            PlayerValue = playerValue;
            ShopValue = sV;
        }

    }

}
