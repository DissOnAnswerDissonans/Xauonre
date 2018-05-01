using MiniXauonre.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core
{
    class Damage
    {
        public Player Pl { get; set; }
        public double Phys { get; set; }
        public double Magic { get; set; }
        public double Pure { get; set; }
        public Damage(Player p, double phys = 0, double magic = 0, double pure = 0)
        {
            Pl = p;
            Phys = phys;
            Magic = magic;
            Pure = pure;
        }

        public double Sum() => Pure + Phys + Magic;

        public void NotifyPlayer() => Pl.NotifyAboutDamage(this);

        public override string ToString() => "Damage: Phys-" + Phys + ", Magic-" + Magic + ", Pure-" + Pure + ";";
    }
}
