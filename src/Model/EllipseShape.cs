using System;
using System.Drawing;

namespace Draw
{
    /// <summary>
    /// Класът правоъгълник е основен примитив, който е наследник на базовия Shape.
    /// </summary>
    [Serializable] // Annotation
    public class EllipseShape : Shape
    {
        #region Constructor

        public EllipseShape(RectangleF rect) : base(rect)
        {
        }

        public EllipseShape(RectangleShape rectangle) : base(rectangle)
        {
        }

        #endregion

        // Function to check the point if it's inside or not
        static bool Checkpoint(double h, double k, double x, double y, double a, double b)
        {
            // Checking the equation of ellipse with the given point
            bool p = ((double)Math.Pow((x - h), 2)
                        / (double)Math.Pow(a, 2))
                    + ((double)Math.Pow((y - k), 2)
                        / (double)Math.Pow(b, 2)) <= 1;

            return p;
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
            // Was -> h = 0, k = 0, x = 2, y = 1, a = 4, b = 5
            //double h = Rectangle.X, k = Rectangle.Y, x = Location.X + Width / 2, y = Location.Y + Height / 2,
            //    a = Rectangle.Height / 2, b = Rectangle.Width / 2;

            //if (Checkpoint(h, k, x, y, a, b) > 1)
            //    Console.WriteLine("Outside");

            //else if (Checkpoint(h, k, x, y, a, b) == 1)
            //    Console.WriteLine("On the ellipse");

            //else // <= 1
            //    Console.WriteLine("Inside");

            var x = point.X;
            var y = point.Y;
            var k = Location.X + Width / 2;
            var h = Location.Y + Height / 2;
            var a = Width / 2;
            var b = Height / 2;

            if (Checkpoint(x, y, k, h, a, b) && base.Contains(point)) // Checkpoint(...) && base.Contains(point)
            {
                // Проверка дали е в обекта само, ако точката е в обхващащия правоъгълник.
                // В случая на правоъгълник - директно връщаме true
                return true;
            }
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

            FillColor = Color.FromArgb(Opacity, FillColor);

            grfx.FillEllipse(
                new SolidBrush(FillColor),
                Rectangle.X,
                Rectangle.Y,
                Rectangle.Width,
                Rectangle.Height
                );

            grfx.DrawEllipse( // was Pens.Black
                new Pen(
                    StrokeColor,
                    StrokeWidth
                ),
                Rectangle.X,
                Rectangle.Y,
                Rectangle.Width,
                Rectangle.Height
                );
        }
        // TODO end
    }
}