using MiniXauonre.Core.Heroes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xauonre.Core;

namespace MiniXauonre.Core
{
    class Map
    {
        public Tile[,] MapTiles { get; protected set; }

        public Dictionary<Hero, Point> UnitPositions { get; set; }

        public int Width { get; protected set; }
        public int Length { get; protected set; }

        public Map(int length = 100, int width = 50)
        {
            UnitPositions = new Dictionary<Hero, Point>();
            MapTiles = new Tile[length, width];
            Width = width;
            Length = length;
            for (var i = 0; i < Length; i++)
                for (var j = 0; j < Width; j++)
                    MapTiles[i, j] = new Tile();
        }

        public bool IsInBounds(Point p) => p.X >= 0 && p.Y >= 0 && p.X < Length && p.Y < Width;

        public List<Hero> GetIn(Point p) => UnitPositions.Where(u => u.Value.X == p.X && u.Value.Y == p.Y).Select(u => u.Key).ToList();

        public bool CellIsFree(Point p) => IsInBounds(p) && MapTiles[p.X, p.Y].Type == TileType.Empty && GetIn(p).Count() == 0;
    }
}
