using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core
{
    static class MapLoader
    {
        public static Map FromText(string text)
        {
            var lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            var height = lines.Count();
            var width = lines[0].Length;
            var map = new Map(width, height);
            for (var i = 0; i < height; i++){
                for(var j = 0; j < width; j++)
                {
                    if (lines[i][j] == '.')
                        map.MapTiles[j, i].Type = TileType.Empty;
                    if (lines[i][j] == '0')
                        map.MapTiles[j, i].Type = TileType.Solid;
                } 
            }
            return map;
        }
    }
}
