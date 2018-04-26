using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xauonre.Core
{
    class Tile
    {
        public TileType type;
        public HashSet<int> idsOfEntities;

        public Tile(TileType type) {
            this.type = type;
        }

    }


    enum TileType
    {
        Wall,
        Empty
    }
}
