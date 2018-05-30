using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core
{
    class Unit
    {
        protected Image Image { get; set; }
        public Unit()
        {
            Image = Graphics.resources.Res.AbilityPower;
        }

        public Image GetImage() => Image;
    }
}
