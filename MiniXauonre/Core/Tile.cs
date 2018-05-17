using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Core
{
    class Tile
    {
        public TileType Type { get; set; }
        public Tile() => Type = TileType.Empty;
    }
    enum TileType
    {
        Empty,
        Solid,
    }
}
