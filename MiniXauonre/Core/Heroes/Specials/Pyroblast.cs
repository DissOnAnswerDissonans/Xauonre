using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xauonre.Core;

namespace MiniXauonre.Core.Heroes.Specials
{
    class Pyroblast : Unit
    {
        public Point Direction { get; set; }

        public Hero Creator { get; set; }
        public Func<bool> Boom { get; set; }
        public Action Tick { get; set; }
        public Pyroblast(Hero creator)
        {
            Creator = creator;
            Direction = new Point();
            Image = Graphics.resources.Res.Pyroblast;
            Boom = () => false;
            Tick = () => { };
        }
    }
}
