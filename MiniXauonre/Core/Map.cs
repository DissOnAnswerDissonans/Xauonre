using MiniXauonre.Controller;
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
        public List<Effect> Effects { get; set; }
        public Tile[,] MapTiles { get; protected set; }

        public Dictionary<Unit, Point> UnitPositions { get; set; }

        public int Width { get; protected set; }
        public int Length { get; protected set; }

        public Map(int length = 30, int width = 15)
        {
            Effects = new List<Effect>();
            UnitPositions = new Dictionary<Unit, Point>();
            MapTiles = new Tile[length, width];
            Width = width;
            Length = length;
            for (var i = 0; i < Length; i++)
                for (var j = 0; j < Width; j++)
                    MapTiles[i, j] = new Tile();
        }

        public bool IsInBounds(Point p) => p.X >= 0 && p.Y >= 0 && p.X < Length && p.Y < Width;

        public List<Unit> GetIn(Point p) => UnitPositions.Where(u => u.Value.X == p.X && u.Value.Y == p.Y).Select(u => u.Key).ToList();

        public bool CellIsFree(Point p) => IsInBounds(p) && MapTiles[p.X, p.Y].Type == TileType.Empty && GetIn(p).Count() == 0;

        public List<KeyValuePair<Hero, Point>> GetHeroPositions() => GetHeroes().Select(h => new KeyValuePair<Hero, Point>(h, UnitPositions[h])).ToList();

        public List<Hero> GetHeroes() => UnitPositions.Keys.Select(u => u as Hero).Where(u => u != null).ToList();

        public void TickTalents(Player player)
        {
            foreach(var effect in Effects.Where(e => e.Creator == player.CurrentHero).ToList())
            {
                effect.Tick(player.CurrentHero);
                if (effect.Timer < 0)
                    Effects.Remove(effect);
            }
        }
    }
}
