using System;
using System.Drawing;

namespace Draw
{
    /// <summary>
    /// Класът правоъгълник е основен примитив, който е наследник на базовия Shape.
    /// </summary>
    public class RectangleShape : Shape
    {
        #region Constructor

        public RectangleShape(RectangleF rect) : base(rect)
        {
        }

        public RectangleShape(RectangleShape rectangle) : base(rectangle)
        {
        }

        #endregion

        // Here

        /// <summary>
        /// Проверка за принадлежност на точка point към правоъгълника.
        /// В случая на правоъгълник този метод може да не бъде пренаписван, защото
        /// Реализацията съвпада с тази на абстрактния клас Shape, който проверява
        /// дали точката е в обхващащия правоъгълник на елемента (а той съвпада с
        /// елемента в този случай).
        /// </summary>
        public override bool Contains(PointF point)
        {
            // PointF[] pointsArray = { point };
            // Umnojavame Vektor po Matrix
            // Invert TransfMatrix
            // TransfMatrix.TransformPoints(pointsArray); // Priema masiv ot to4ki
            // Invert TransfMatrix pak
            // pointsArray[0];
            // Transformiranata to4ka 6te b1de dost1pena 4rez pointsArray
            if (base.Contains(point)) // Transform s to4ka pointsArray
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
        public override void DrawSelf(Graphics grfx) // Graphics instanciq na grafi4ni obekti
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
            //grfx.ResetTransform();

            grfx.Transform = TransformationMatrix; // TODO opravi 2 b1ga - ne se markira, obratno na dvijenieto

            FillColor = Color.FromArgb(Opacity, FillColor);

            grfx.FillRectangle(
                new SolidBrush(FillColor),
                Rectangle.X,
                Rectangle.Y,
                Rectangle.Width,
                Rectangle.Height
                );

            grfx.DrawRectangle(
                new Pen( // Was Pens.Black
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