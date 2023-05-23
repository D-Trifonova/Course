using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Draw
{
    /// <summary>
    /// Класът правоъгълник е основен примитив, който е наследник на базовия Shape.
    /// </summary>
    public class GroupShape : Shape
    {
        public List<Shape> SubShapes = new List<Shape>();
        #region Constructor

        public GroupShape(RectangleF rect) : base(rect)
        {
        }

        public GroupShape(RectangleShape rectangle) : base(rectangle)
        {
        }

        #endregion

        // Here

        public override PointF Location
        {
            get => base.Location;
            // set => base.Location
            set
            {
                // for every item in SubShape
                // item Location = ... promenq se location-a na goren lqv 1g1l
                foreach (Shape item in SubShapes)
                {
                    item.Location = new PointF( // za rotaciq
                        item.Location.X - Location.X + value.X,
                        item.Location.Y - Location.Y + value.Y);
                }

                base.Location = new PointF( // goren lqv 1g1l
                    value.X, value.Y); // za translaciq

                foreach (Shape item in SubShapes)
                {
                    item.FillColor = FillColor;
                }
                // i za stroke collor napravi
                foreach (Shape item in SubShapes)
                {

                }
            }
        }

        public override Matrix TransformationMatrix // 5 i 6 argument otgovarqt za rotaciqta na primitiva. Dost1pvat se direktno elementite
        {
            get => base.TransformationMatrix; // flip da ima BREAKPOINT
            set
            {
                base.TransformationMatrix.Multiply(value);
                foreach (Shape item in SubShapes)
                    base.TransformationMatrix.Multiply(value);
            }
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
            // For every item in SubShape
            //  Call Contains(point)
            //  if true -> return true;
            //  return false;
            foreach (Shape item in SubShapes) // Group selection
            {
                if (item.Contains(point))
                {
                    return true;
                }
                return false;
            }
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
        public override void DrawSelf(Graphics grfx)
        {
            base.DrawSelf(grfx);

            foreach (Shape item in SubShapes)
            {
                item.DrawSelf(grfx);
            }
        }
        // TODO end
    }
}