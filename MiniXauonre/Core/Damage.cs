using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core
{
    public class Damage
    {
        public double Phys { get; set; }
        public double Magic { get; set; }
        public double Pure { get; set; }
        public Damage(double phys = 0, double magic = 0, double pure = 0)
        {
            Phys = phys;
            Magic = magic;
            Pure = pure;
        }

        public double Sum() => Pure + Phys + Magic;


        public override string ToString() => "Damage: Phys-" + Phys + ", Magic-" + Magic + ", Pure-" + Pure + ";";
    }
}
