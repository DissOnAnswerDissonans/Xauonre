using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core
{
    public class FuncData
    {
        public double DoubleValue { get; private set; }
        public bool BoolValue { get; private set; }
        public Damage DamageValue { get; private set; }

        public FuncData(double dV = 0, bool bV = false, Damage dmgV = null) {
            DoubleValue = dV;
            BoolValue = bV;
            DamageValue = dmgV;
        }

    }

}
