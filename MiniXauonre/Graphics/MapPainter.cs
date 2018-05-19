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
using MiniXauonre.Graphics.resources.Tiles;

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
        private TileLoader Loader { get; set; }

        public MapPainter(Map map, Size size)
        {
            Map = map;
            DrawSize = size;
            Loader = new TileLoader();
            Av = new List<Tuple<Point, MapPointMode>>();
            SwitchMap(map);      
        }

        public void Paint(System.Drawing.Graphics g, int s)
        {
            DrawMap(g, s);
            DrawUnits(g, s);
            //DrawAvs(g);           
        }

        public void FastPaint(System.Drawing.Graphics g, int s)
        {
            var ScaleFactor = (int)Math.Pow(2, s);
            g.DrawRectangle(
                new Pen(Color.Green, 3.0f),
                new Rectangle(0, 0, (int)(TileSize.Width * Map.Length / ScaleFactor), (int)(TileSize.Height * Map.Width / ScaleFactor))
                );
            for (int i = 1; i < Map.Length; ++i)
                g.DrawLine(new Pen(Color.Green),
                    new PointF((TileSize.Width * i / ScaleFactor), 0),
                    new PointF((TileSize.Width * i / ScaleFactor), (TileSize.Height * Map.Width / ScaleFactor)));
            for (int i = 1; i < Map.Width; ++i)
                g.DrawLine(new Pen(Color.Green),
                    new PointF(0, (TileSize.Height * i / ScaleFactor)),
                    new PointF((TileSize.Width * Map.Length / ScaleFactor), (TileSize.Height * i / ScaleFactor)));
            DrawUnits(g, s);
        }

        public void ResizeMap(Size s) => DrawSize = s;

        public void SwitchMap(Map map)
        {
            MapSize = new Size(Map.Length, Map.Width);
            TileSize = new SizeF(128, 128);        
        }

        private void DrawMap(System.Drawing.Graphics g, int Scaler)
        {
            var ScaleFactor = (int)Math.Pow(2, Scaler);
            var modifiedTS = new Size((int)(TileSize.Width / ScaleFactor), (int)(TileSize.Height / ScaleFactor));

            var test = Graphics.resources.Res.TileR3;

            var bounds = g.VisibleClipBounds;
            var tileBounds = new Rectangle(
                (int)bounds.Left / modifiedTS.Width,
                (int)bounds.Top / modifiedTS.Height,
                (int)bounds.Width / modifiedTS.Width,
                (int)bounds.Height / modifiedTS.Height
            );

            for (int x = Math.Max(0, tileBounds.Left); x < Math.Min(tileBounds.Right + 2, Map.Length); x++)
            {
                for (int y = Math.Max(0, tileBounds.Top); y < Math.Min(tileBounds.Bottom + 2, Map.Width); y++)
                {
                    Bitmap tileImage;
                    switch (Map.MapTiles[x, y].Type)
                    {
                        case TileType.Empty:
                            tileImage = Loader.BasicTile[Scaler]; break;
                        case TileType.Solid:
                            tileImage = Loader.SolidTile[Scaler]; break;
                        default:
                            tileImage = Loader.NullTile[0]; break;
                    }
                    g.DrawImageUnscaled(tileImage, new System.Drawing.Point(x * modifiedTS.Width, y * modifiedTS.Height));

                    // g.DrawImage(tileImage, new RectangleF
                    //    (x * modifiedTS.Width, y * modifiedTS.Height, modifiedTS.Width, modifiedTS.Height));
                
                }
            }
        }

        private void DrawUnits(System.Drawing.Graphics g, int Scaler)
        {
            var ScaleFactor = (int)Math.Pow(2, Scaler);
            var modifiedTS = new SizeF(TileSize.Width / ScaleFactor, TileSize.Height / ScaleFactor);
            const float unitShift = 0.3f;
            foreach (var unit in Map.UnitPositions.Keys)
            {
                var coords = Map.UnitPositions[unit];
                var heroImage = unit.GetImage();
                var borders = new RectangleF(coords.X * modifiedTS.Width, 
                    (coords.Y - unitShift) * modifiedTS.Height, modifiedTS.Width, modifiedTS.Height);
                
                g.DrawImage(heroImage, borders);
                //DrawUnitInfo(g, unit, borders);
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