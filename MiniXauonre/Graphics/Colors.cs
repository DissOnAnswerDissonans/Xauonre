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
            Color.DarkGreen,
            Color.Indigo,
        };
        
        public static readonly List<Color> PlayerLightColors = new List<Color>
        {
            Color.OrangeRed,
            Color.DodgerBlue,
            Color.LimeGreen,
            Color.Fuchsia,
        };
        
        public static readonly List<Color> ItemTierColors = new List<Color>
        {
            Color.White,
            Color.LimeGreen,
            Color.DeepSkyBlue,
            Color.DarkOrange,
        };
    }
}