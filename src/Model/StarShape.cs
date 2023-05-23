using System;
using System.Collections.Generic; // !
using System.Drawing;
using System.Linq; // !

namespace Draw
{
    /// <summary>
    /// Класът правоъгълник е основен примитив, който е наследник на базовия Shape.
    /// </summary>
    public class StarShape : Shape
    {
        #region Constructor

        public StarShape(RectangleF rect) : base(rect)
        {
        }

        public StarShape(RectangleShape rectangle) : base(rectangle)
        {
        }

        #endregion

        /// <summary>
        /// Determine if a point is in a polygon.
        /// </summary>
        /// <param name="x">point x</param>
        /// <param name="y">point y</param>
        /// <param name="px">polygon x</param>
        /// <param name="py">polygon y</param>
        /// <returns>
        /// 1: point v polygon
        /// 0: point na polygon
        /// -1: point izv1n polygon
        /// </returns>
        static bool PointInPolygon(double x, double y, IEnumerable<double> px, IEnumerable<double> py)
        {
            var p = px.Zip(py, (x_, y_) => new { x = x_, y = y_ });
            return p.Zip(p.Skip(1).Concat(p), (a, b) =>
            {
                if (a.y == y && b.y == y)
                    return (a.x <= x && x <= b.x) || (b.x <= x && x <= a.x) ? 0 : 1;
                return a.y <= b.y ?
                    y <= a.y || b.y < y ? 1 : Math.Sign((a.x - x) * (b.y - y) - (a.y - y) * (b.x - x)) :
                    y <= b.y || a.y < y ? 1 : Math.Sign((b.x - x) * (a.y - y) - (b.y - y) * (a.x - x));
            }).Aggregate(-1, (r, v) => r * v) <= 1;
        }

        /// <summary>
        /// Проверка за принадлежност на точка point към правоъгълника.
        /// В случая на правоъгълник този метод може да не бъде пренаписван, защото
        /// Реализацията съвпада с тази на абстрактния клас Shape, който проверява
        /// дали точката е в обхващащия правоъгълник на елемента (а той съвпада с
        /// елемента в този случай).
        /// </summary>
        public override bool Contains(PointF point)
        {
            var x = point.X;
            var y = point.Y;
            double[] a = new double[] { Rectangle.X, Rectangle.X + Rectangle.Width / 2 - 20, Rectangle.X + Rectangle.Width / 2,
        Rectangle.X + Rectangle.Width / 2 + 20, Rectangle.X + Rectangle.Width, Rectangle.X + Rectangle.Width / 2 + 20,
        Rectangle.X + Rectangle.Width / 2, Rectangle.X + Rectangle.Width / 2 - 20 };
            double[] b = new double[] { Rectangle.Y + Rectangle.Height / 2, Rectangle.Y + Rectangle.Height / 2 - 20,
        Rectangle.Y, Rectangle.Y + Rectangle.Height / 2 - 20, Rectangle.Y + Rectangle.Height / 2,
        Rectangle.Y + Rectangle.Height / 2 + 20, Rectangle.Y + Rectangle.Height, Rectangle.Y + Rectangle.Height / 2 + 20 };
            // PointInPolygon(...) && base.Contains(point)
            if (PointInPolygon(x, y, a, b) && base.Contains(point)) // if PointInPolygon is true and base.Contains(point) is true, then return 'true', which is equivalent to "if PointInPolygon is false or base.Contains(point) is false, then return 'false'. CTRL+E+W
            {
                // Проверка дали е в обекта само, ако точката е в обхващащия правоъгълник.
                // В случая на правоъгълник - директно връщаме true
                return true;
            }
            else
            {
                // Ако не е в обхващащия правоъгълник, то не може да е в обекта и => false
                return false;
            }
        }

        /// <summary>
        /// Частта, визуализираща конкретния примитив.
        /// </summary>
        public override void DrawSelf(Graphics grfx)
        {
            base.DrawSelf(grfx);

            //grfx.TranslateTransform( // Rotation Rotate
            //    Rectangle.X + Rectangle.Width / 2,
            //    Rectangle.Y + Rectangle.Height / 2
            //    );
            //grfx.RotateTransform(Rotation);
            //grfx.TranslateTransform( // Back to normal
            //    -(Rectangle.X + Rectangle.Width / 2),
            //    -(Rectangle.Y + Rectangle.Height / 2)
            //    );

            grfx.Transform = TransformationMatrix;

            //PointF[] points = new PointF[8]; // Hardcorednat Location
            //points[0] = new PointF(0, 150); // Location.X/Y, Rectangle.X and Y Width and Height
            //points[1] = new PointF(120, 120);
            //points[2] = new PointF(150, 0);
            //points[3] = new PointF(180, 120);
            //points[4] = new PointF(300, 150);
            //points[5] = new PointF(180, 180);
            //points[6] = new PointF(150, 300);
            //points[7] = new PointF(120, 180);

            PointF[] points = new PointF[8];
            points[0] = new PointF(Rectangle.X, Rectangle.Y + Rectangle.Height / 2);
            points[1] = new PointF(Rectangle.X + Rectangle.Width / 2 - 20, Rectangle.Y + Rectangle.Height / 2 - 20);
            points[2] = new PointF(Rectangle.X + Rectangle.Width / 2, Rectangle.Y);
            points[3] = new PointF(Rectangle.X + Rectangle.Width / 2 + 20, Rectangle.Y + Rectangle.Height / 2 - 20);
            points[4] = new PointF(Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height / 2);
            points[5] = new PointF(Rectangle.X + Rectangle.Width / 2 + 20, Rectangle.Y + Rectangle.Height / 2 + 20);
            points[6] = new PointF(Rectangle.X + Rectangle.Width / 2, Rectangle.Y + Rectangle.Height);
            points[7] = new PointF(Rectangle.X + Rectangle.Width / 2 - 20, Rectangle.Y + Rectangle.Height / 2 + 20);

            FillColor = Color.FromArgb(Opacity, FillColor);

            grfx.FillPolygon(
                new SolidBrush(FillColor),
                points
                );

            grfx.DrawPolygon(
                new Pen(
                    StrokeColor,
                    StrokeWidth
                ),
                points
                );
        }
        // TODO end
    }
}