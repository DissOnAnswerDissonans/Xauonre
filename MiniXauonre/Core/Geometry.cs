using MiniXauonre.Core;
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

    internal class Point
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public Point() { }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Add(Point p)
        {
            X += p.X;
            Y += p.Y;
        }
        
        public double GetDistanceTo(Point p) => Math.Sqrt((p.X - this.X) * (p.X - this.X) + (p.Y - this.Y) * (p.Y - this.Y));

        public Tuple<int, int> GetCoords() => Tuple.Create(X, Y);

        public int GetStepsTo(Point p)
        {
            var dX = Math.Abs(p.X - this.X);
            var dY = Math.Abs(p.Y - this.Y);
            var diag = Math.Min(dX, dY);
            return diag * GeometryRules.DiagonalFactor + (dX + dY - diag - diag) * GeometryRules.NormalFactor;
        }

        static public Point StepToPoint(StepTypes type)
        {
            switch (type)
            {
                case StepTypes.Down:
                    return new Point(0, 1);
                case StepTypes.LeftDown:
                    return new Point(-1, 1);
                case StepTypes.Left:
                    return new Point(-1, 0);
                case StepTypes.LeftUp:
                    return new Point(-1, -1);
                case StepTypes.Up:
                    return new Point(0, -1);
                case StepTypes.RightUp:
                    return new Point(1, -1);
                case StepTypes.Right:
                    return new Point(1, 0);
                case StepTypes.RightDown:
                    return new Point(1, 1);
                default:
                    return new Point(0, 0);
            }
        }

        public StepTypes ToStep()
        {
            switch (X)
            {
                case 0:
                    switch (Y)
                    {
                        case -1:
                            return StepTypes.Up;
                        case 1:
                            return StepTypes.Down;
                        default:
                            throw new ArgumentException("Fu");
                    }
                case 1:
                    switch (Y)
                    {
                        case -1:
                            return StepTypes.RightUp;
                        case 1:
                            return StepTypes.RightDown;
                        case 0:
                            return StepTypes.Right;
                        default:
                            throw new ArgumentException("Fu");
                    }
                case -1:
                    switch (Y)
                    {
                        case -1:
                            return StepTypes.LeftUp;
                        case 1:
                            return StepTypes.LeftDown;
                        case 0:
                            return StepTypes.Left;
                        default:
                            throw new ArgumentException("Fu");
                    }
                default:
                    throw new ArgumentException("Fu");
            }
        }


        public static Dictionary<Point, int> PossibleSteps =
            new Dictionary<Point, int>{
                { new Point(0, 1), GeometryRules.NormalFactor },
                { new Point(0, -1), GeometryRules.NormalFactor },
                { new Point(1, 0), GeometryRules.NormalFactor },
                { new Point(-1, 0), GeometryRules.NormalFactor },
                { new Point(1, 1), GeometryRules.DiagonalFactor },
                { new Point(1, -1), GeometryRules.DiagonalFactor },
                { new Point(-1, 1), GeometryRules.DiagonalFactor },
                { new Point(-1, -1), GeometryRules.DiagonalFactor },
            };

        public static List<Point> DimentionsMultiplier =
            new List<Point>
            {
                new Point(1, 1),
                new Point(-1, 1),
                new Point(1, -1),
                new Point(-1, -1),
            };


        public static Point operator +(Point p1, Point p2) => new Point(p1.X + p2.X, p1.Y + p2.Y);

        public enum StepTypes
        {
            Right,
            Left,
            Up,
            Down,
            RightUp,
            RightDown,
            LeftUp,
            LeftDown,
        }






        public override bool Equals(object obj) => obj.GetType() == typeof(Point) && X == (obj as Point).X && Y == (obj as Point).Y;
    }
}
