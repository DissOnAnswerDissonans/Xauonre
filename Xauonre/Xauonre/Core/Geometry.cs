using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xauonre.Core
{
    static class GeometryRules
    {
        public const int DiagonalFactor = 3;
        public const int NormalFactor = 2;
    }


    class Point
    {
        int X, Y;
        public Point() { }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public double GetDistanceTo(Point p) => Math.Sqrt((p.X - this.X) * (p.X - this.X) + (p.Y - this.Y) * (p.Y - this.Y));
        
        public int GetStepsTo(Point p)
        {
            var dX = Math.Abs(p.X - this.X);
            var dY = Math.Abs(p.Y - this.Y);
            var diag = Math.Min(dX, dY);
            return diag * GeometryRules.DiagonalFactor + (dX + dY - diag - diag) * GeometryRules.NormalFactor;
        }

    }
}
