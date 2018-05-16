using MiniXauonre.Controller;
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

        public double DoubleValue { get; set; }
        public bool BoolValue { get; set; }
        public Damage DamageValue { get; set; }
        public Map MapValue { get; set; }
        public Player PlayerValue { get; set; }

        public FuncData(double dV = 0, bool bV = false, Shop sV = null,  Damage dmgV = null, Map mapvalue = null, Player playerValue = null) {
            DoubleValue = dV;
            BoolValue = bV;
            DamageValue = dmgV;
            MapValue = mapvalue;
            PlayerValue = playerValue;
            ShopValue = sV;
        }

    }

}
