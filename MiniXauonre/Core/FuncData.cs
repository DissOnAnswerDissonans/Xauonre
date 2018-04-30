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
        public double DoubleValue { get; private set; }
        public bool BoolValue { get; private set; }
        public Damage DamageValue { get; private set; }
        public Map MapValue { get; private set; }
        public Player PlayerValue { get; private set; }

        public FuncData(double dV = 0, bool bV = false, Damage dmgV = null, Map mapvalue = null, Player playerValue = null) {
            DoubleValue = dV;
            BoolValue = bV;
            DamageValue = dmgV;
            MapValue = mapvalue;
            PlayerValue = playerValue;
        }

    }

}
