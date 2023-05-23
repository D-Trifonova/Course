using System;
using System.Drawing;

namespace Draw
{
    /// <summary>
    /// Класът правоъгълник е основен примитив, който е наследник на базовия Shape.
    /// </summary>
    public class CurveShape : Shape
    {
        #region Constructor

        public CurveShape(RectangleF rect) : base(rect)
        {
        }

        public CurveShape(RectangleShape rectangle) : base(rectangle)
        {
        }

        #endregion

        /// <summary>
        /// Проверка за принадлежност на точка point към правоъгълника.
        /// В случая на правоъгълник този метод може да не бъде пренаписван, защото
        /// Реализацията съвпада с тази на абстрактния клас Shape, който проверява
        /// дали точката е в обхващащия правоъгълник на елемента (а той съвпада с
        /// елемента в този случай).
        /// </summary>
        public override bool Contains(PointF point)
        {
            if (base.Contains(point))
                // Проверка дали е в обекта само, ако точката е в обхващащия правоъгълник.
                // В случая на правоъгълник - директно връщаме true
                return true;
            else
                // Ако не е в обхващащия правоъгълник, то не може да е в обекта и => false
                return false;
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

            //PointF[] points = new PointF[4]; // Trapec OLD
            //points[0] = new PointF(Rectangle.X, Rectangle.Y + Rectangle.Height / 2);
            //points[1] = new PointF(Rectangle.X + Rectangle.Width / 2 - 80, Rectangle.Y + Rectangle.Height / 2 - 100);
            //points[2] = new PointF(Rectangle.X + Rectangle.Width / 2 + 80, Rectangle.Y + Rectangle.Height / 2 - 200);
            //points[3] = new PointF(Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height / 2); // First Rectangle.X + Rectangle.Width / 2 <- diamant

            PointF[] points = new PointF[4]; // NEW Trapecoid Isosceles
            points[0] = new PointF(Rectangle.X, Rectangle.Y + Rectangle.Height); // bottom-left
            points[1] = new PointF(Rectangle.X + Rectangle.Width / 2-50, Rectangle.Y + Rectangle.Height / 2 - 100); // top-left ; -80; -100 // Tupougulnik
            points[2] = new PointF(Rectangle.X + Rectangle.Width / 2+50, Rectangle.Y + Rectangle.Height / 2 - 100); // top-right; -80; -100
            points[3] = new PointF(Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height); // bottom-right


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