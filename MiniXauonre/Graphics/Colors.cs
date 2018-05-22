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
            Color.DarkBlue,
            Color.Indigo,
            Color.DarkGreen,
        };
        
        public static readonly List<Color> PlayerLightColors = new List<Color>
        {
            Color.OrangeRed,
            Color.DodgerBlue,
            Color.Fuchsia,
            Color.LimeGreen,
        };
    }
}