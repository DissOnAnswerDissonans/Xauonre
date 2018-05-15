using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiniXauonre.Core;
using Point = Xauonre.Core.Point;

namespace MiniXauonre.Graphics
{
    
    enum MapPointMode
    {
        Empty,
        Chooseable,
        Availible,
        Unavailible
    }

    class MapPainter
    {
        private Map Map { get; set; }
        private List<Tuple<Point, MapPointMode>> Av;
        private Bitmap mapImage;
        private Size DrawSize { get; set; }
        private Size MapSize { get; set; }
        private SizeF TileSize { get; set; }

        public MapPainter(Map map, Size size)
        {
            Map = map;
            DrawSize = size;
            Av = new List<Tuple<Point, MapPointMode>>();
            SwitchMap(map);
            DrawMap();
        }

        public void ResizeMap(Size s) => DrawSize = s;

        public void SwitchMap(Map map)
        {
            MapSize = new Size(Map.Width, Map.Length);
            TileSize = new SizeF((float)DrawSize.Width / MapSize.Width, (float)DrawSize.Height / MapSize.Height);        
        }

        private void DrawMap()
        {
            mapImage = new Bitmap(DrawSize.Width, DrawSize.Height);
            var g = System.Drawing.Graphics.FromImage(mapImage);
            for (int x = 0; x < MapSize.Width; x++)
            {
                for (int y = 0; y < MapSize.Height; y++)
                {
                    Bitmap tileImage;
                    switch (Map.MapTiles[x, y].Type)
                    {
                        case TileType.Empty:
                            tileImage = Graphics.resources.Res.Tile; break;                      
                        case TileType.Solid:
                            tileImage = Graphics.resources.Res.TileSolid; break;   
                        default:
                            tileImage = Graphics.resources.Res.noyhing; break;
                    }
                    g.DrawImage(tileImage, new RectangleF
                        (x * TileSize.Width, y * TileSize.Height, TileSize.Width, TileSize.Height));
                }
            }
        }

        private void DrawAvs()
        {
            var g = System.Drawing.Graphics.FromImage(mapImage);
            foreach (var p in Av)
            {
                Bitmap tileImage;
                switch (p.Item2)
                {
                    case MapPointMode.Chooseable:
                        tileImage = Graphics.resources.Res.chooseable_tile; break;
                    case MapPointMode.Availible:
                        tileImage = Graphics.resources.Res.available_tile; break;
                    case MapPointMode.Unavailible:
                        tileImage = Graphics.resources.Res.unavailable_tile; break;
                    default:
                        tileImage = Graphics.resources.Res.noyhing; break;
                }
                g.DrawImage(tileImage, new RectangleF
                    (p.Item1.X * TileSize.Width, p.Item1.Y * TileSize.Height, TileSize.Width, TileSize.Height));
            }
        }
    }
}