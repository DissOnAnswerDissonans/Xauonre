using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Configuration;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiniXauonre.Core;
using MiniXauonre.Core.Heroes;
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
        public Size DrawSize { get; set; }
        private Size MapSize { get; set; }
        private SizeF TileSize { get; set; }

        public MapPainter(Map map, Size size)
        {
            Map = map;
            DrawSize = size;
            Av = new List<Tuple<Point, MapPointMode>>();
            SwitchMap(map);      
        }

        public void Paint(System.Drawing.Graphics g)
        {
            DrawMap(g);
            DrawUnits(g);
            //DrawAvs(g);           
        }

        public void ResizeMap(Size s) => DrawSize = s;

        public void SwitchMap(Map map)
        {
            MapSize = new Size(Map.Length, Map.Width);
            TileSize = new SizeF(128, 128);        
        }

        private void DrawMap(System.Drawing.Graphics g)
        {

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
        
        private void DrawUnits(System.Drawing.Graphics g)
        {
            const float unitShift = 0.3f;
            foreach (var unit in Map.UnitPositions.Keys)
            {
                var coords = Map.UnitPositions[unit];
                var heroImage = unit.GetImage();
                var borders = new RectangleF(coords.X * TileSize.Width, 
                    (coords.Y - unitShift) * TileSize.Height, TileSize.Width, TileSize.Height);
                
                g.DrawImage(heroImage, borders);
                DrawUnitInfo(g, unit, borders);
            }
        }

        readonly Font kok = new Font(FontFamily.GenericSansSerif, 16);

        private void DrawUnitInfo(System.Drawing.Graphics g, Hero unit, RectangleF borders)
        {
            var b = new RectangleF(borders.Location, borders.Size);
            b.Y += TileSize.Height;
            g.DrawString((((int)unit.GetHp()).ToString(CultureInfo.CurrentCulture) + "/"
                          + ((int)unit.GetMaxHp()).ToString(CultureInfo.CurrentCulture)),
                kok, new SolidBrush(Color.Black), b);
        }

        private void DrawAvs(System.Drawing.Graphics g)
        {
            foreach (var p in Av)
            {
                Bitmap tileImage;
                switch (p.Item2)
                {
                    case MapPointMode.Chooseable:
                        tileImage = resources.Res.chooseable_tile; break;
                    case MapPointMode.Availible:
                        tileImage = resources.Res.available_tile; break;
                    case MapPointMode.Unavailible:
                        tileImage = resources.Res.unavailable_tile; break;
                    default:
                        tileImage = resources.Res.noyhing; break;
                }
                g.DrawImage(tileImage, new RectangleF
                    (p.Item1.X * TileSize.Width, p.Item1.Y * TileSize.Height, TileSize.Width, TileSize.Height));
            }
        }
    }
}