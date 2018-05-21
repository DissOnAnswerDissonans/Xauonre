using System.Collections.Generic;
using System.Drawing;

namespace MiniXauonre.Graphics
{
    public struct Colors
    {
        public const int count = 4;
        
        public static readonly List<Color> PlayerDarkColors = new List<Color>
        {
            Color.Maroon,
            Color.Indigo,
            Color.DarkGreen,
            Color.DarkBlue,
        };
        
        public static readonly List<Color> PlayerLightColors = new List<Color>
        {
            Color.OrangeRed,
            Color.Fuchsia,
            Color.LimeGreen,
            Color.DodgerBlue,
        };
    }
}