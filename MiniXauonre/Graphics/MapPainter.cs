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
using MiniXauonre.Controller;
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
        public List<Point> Av;
        private Bitmap mapImage;
        public Size DrawSize { get; set; }
        private Size MapSize { get; set; }
        private SizeF TileSize { get; set; }
        private TileLoader Loader { get; set; }
        
        private ScreenForm Screen { get; set; }

        private int Scaler { get; set; }
        private int ScaleFactor { get; set; }
        private Size ModifiedTS { get; set; }

        public MapPainter(Map map, ScreenForm s)
        {
            Screen = s;
            Map = map;
            DrawSize = Screen.Size;
            Loader = new TileLoader();
            Av = new List<Point>();
            SwitchMap(map);      
        }

        public void Paint(System.Drawing.Graphics g, int s)
        {
            Scaler = s;
            ScaleFactor = (int)Math.Pow(2, s);
            ModifiedTS = new Size((int)(TileSize.Width / ScaleFactor), (int)(TileSize.Height / ScaleFactor));
            DrawMap(g);
            DrawUnits(g);
            DrawAvs(g);           
        }

        public void ResizeMap(Size s) => DrawSize = s;

        public void SwitchMap(Map map)
        {
            MapSize = new Size(Map.Length, Map.Width);
            TileSize = new SizeF(128, 128);        
        }

        private void DrawMap(System.Drawing.Graphics g)
        {
            var test = Graphics.resources.Res.TileR3;

            var bounds = g.VisibleClipBounds;
            var tileBounds = new Rectangle(
                (int)bounds.Left / ModifiedTS.Width,
                (int)bounds.Top / ModifiedTS.Height,
                (int)bounds.Width / ModifiedTS.Width,
                (int)bounds.Height / ModifiedTS.Height
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
                    g.DrawImageUnscaled(tileImage, new System.Drawing.Point(x * ModifiedTS.Width, y * ModifiedTS.Height));           
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
                var borders = new RectangleF(coords.X * ModifiedTS.Width, 
                    (coords.Y - unitShift) * ModifiedTS.Height, ModifiedTS.Width, ModifiedTS.Height);
                
                g.DrawImage(heroImage, borders);
                DrawUnitInfo(g, unit, borders);
            }
        }

        readonly Font kok = new Font(FontFamily.GenericSansSerif, 16);

        private void DrawUnitInfo(System.Drawing.Graphics g, Hero unit, RectangleF borders)
        {
            var b = new RectangleF(borders.Location, borders.Size);
            b.Y += ModifiedTS.Height;
            b.Height -= ModifiedTS.Height * 0.7f;
            var color = Colors.PlayerDarkColors[unit.P.Game.Players.IndexOf(unit.P) % Colors.count];
            g.FillRectangle(new SolidBrush(Color.Black), b.X, b.Y, b.Width, b.Height);
            g.FillRectangle(new SolidBrush(color), b.X, b.Y, b.Width * (float)(unit.GetHp() / unit.GetMaxHp()), b.Height);
            g.DrawRectangle(new Pen(Color.Black), b.X, b.Y, b.Width, b.Height);
            if (Scaler == 0)
            {
                g.DrawRectangle(new Pen(Color.Black, 3.0f), b.X, b.Y, b.Width, b.Height);
                b.Y += 6;
                g.DrawString(((int) unit.GetHp()).ToString(CultureInfo.CurrentCulture) + "/"
                 + ((int) unit.GetMaxHp()).ToString(CultureInfo.CurrentCulture),
                    kok, new SolidBrush(Color.White), b);
            }
            else if (Scaler == 1)
            {
                b.Y -= 3;
                b.Height += 8;
                g.DrawString(((int) unit.GetHp()).ToString(CultureInfo.CurrentCulture),
                    kok, new SolidBrush(Color.White), b);
            }
        }

        private void DrawAvs(System.Drawing.Graphics g)
        {
            var tileImage = resources.Res.chooseable_tile;
            foreach (var p in Av)
            {
                g.DrawImage(tileImage, new RectangleF
                    (p.X * ModifiedTS.Width, p.Y * ModifiedTS.Height, ModifiedTS.Width, ModifiedTS.Height));
            }
        }

        public void OnMouseClick(System.Drawing.Point truncate, MouseButtons button)
        { 
            
            Screen.ClickedOnMap(new Point(truncate.X / ModifiedTS.Width, truncate.Y / ModifiedTS.Height), button);
        }
    }
}