using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniXauonre.Graphics.resources.Tiles
{
    class TileLoader
    {
        private const int QualityLevels = 4;

        public Bitmap[] NullTile { get; private set; }
        public Bitmap[] BasicTile { get; private set; }
        public Bitmap[] SolidTile { get; private set; }
        public TileLoader()
        {
            BasicTile = new Bitmap[QualityLevels];
            BasicTile[0] = resources.Res.Tile;
            BasicTile[1] = resources.Res.TileR1;
            BasicTile[2] = resources.Res.TileR2;
            BasicTile[3] = resources.Res.TileR3;

            SolidTile = new Bitmap[QualityLevels];
            SolidTile[0] = resources.Res.TileSolid;
            SolidTile[1] = resources.Res.TileSolidR1;
            SolidTile[2] = resources.Res.TileSolidR2;
            SolidTile[3] = resources.Res.TileSolidR3;

            NullTile = new Bitmap[1];
            NullTile[0] = resources.Res.noyhing;
        }
    }
}
