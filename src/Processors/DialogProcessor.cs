using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Draw
{
    /// <summary>
    /// Класът, който ще бъде използван при управляване на диалога.
    /// </summary>
    public class DialogProcessor : DisplayProcessor
    {
        #region Constructor

        public DialogProcessor()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Използвам елемент при селекция.
        /// </summary>
        protected Shape selection1;
        public Shape Selection1
        {
            get { return selection1; }
            set { selection1 = value; }
        }
        /// <summary>
        /// Избран елемент.
        /// </summary>
        private List<Shape> selection = new List<Shape>();
        public List<Shape> Selection
        {
            get { return selection; }
            set { selection = value; }
        }

        /// <summary>
        /// Дали в момента диалога е в състояние на "влачене" на избрания елемент.
        /// </summary>
        private bool isDragging;
        public bool IsDragging
        {
            get { return isDragging; }
            set { isDragging = value; }
        }

        /// <summary>
        /// Последна позиция на мишката при "влачене".
        /// Използва се за определяне на вектора на транслация.
        /// </summary>
        private PointF lastLocation;
        public PointF LastLocation
        {
            get { return lastLocation; }
            set { lastLocation = value; }
        }

        /// <summary>
		/// Големината на екрана на потребителя.
		/// Използва се за определяне на граници за генериране на Shape.
		/// </summary>
		private Size screenSize;
        public Size SceneSize
        {
            get { return screenSize; }
            set { screenSize = value; }

        }

        #endregion

        /// <summary>
        /// Добавя примитив - правоъгълник на произволно място върху клиентската област.
        /// </summary>
        public void AddRandomRectangle(int strokeWidth)
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);

            RectangleShape rect = new RectangleShape(new Rectangle(x, y, 100, 200));
            rect.FillColor = Color.White;
            rect.StrokeColor = Color.DarkRed;
            rect.StrokeWidth = strokeWidth;

            ShapeList.Add(rect);
        }

        public void AddRandomEllipse(int strokeWidth)
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);

            EllipseShape ellipse = new EllipseShape(new Rectangle(x, y, 100, 200));
            ellipse.FillColor = Color.White;
            ellipse.StrokeColor = Color.Green;
            ellipse.StrokeWidth = strokeWidth;

            ShapeList.Add(ellipse);
        }

        public void AddRandomStar(int strokeWidth)
        {
            //Random rnd = new Random(); // OLD
            //int x = rnd.Next(100, 1000);
            //int y = rnd.Next(100, 600);

            //StarShape star = new StarShape(new Rectangle(x, y, 100, 200));
            //star.FillColor = Color.Blue;

            //ShapeList.Add(star);

            Random rnd = new Random(); // NEW
            int x = rnd.Next(screenSize.Width - 200);
            int y = rnd.Next(screenSize.Height - 200);

            StarShape star = new StarShape(new Rectangle(x, y, 200, 200));
            star.FillColor = Color.White;
            star.StrokeColor = Color.Red;
            star.StrokeWidth = strokeWidth;

            ShapeList.Add(star);
        }

        public void AddRandomCurveShape(int strokeWidth)
        {
            Random rnd = new Random();
            int x = rnd.Next(screenSize.Width - 200);
            int y = rnd.Next(screenSize.Height - 200);

            CurveShape curve = new CurveShape(new Rectangle(x, y, 200, 200));
            curve.FillColor = Color.DeepPink;
            curve.StrokeColor = Color.Maroon;
            curve.StrokeWidth = strokeWidth;

            ShapeList.Add(curve);
        }

        /// <summary>
        /// Проверява дали дадена точка е в елемента.
        /// Обхожда в ред обратен на визуализацията с цел намиране на
        /// "най-горния" елемент т.е. този който виждаме под мишката.
        /// </summary>
        /// <param name="point">Указана точка</param>
        /// <returns>Елемента на изображението, на който принадлежи дадената точка.</returns>
        public Shape ContainsPoint(PointF point)
        {
            for (int i = ShapeList.Count - 1; i >= 0; i--)
            {
                if (ShapeList[i].Contains(point))
                {
                    //ShapeList[i].FillColor = Color.Pink;

                    return ShapeList[i];
                }
            }
            return null;

            //if (ShapeList[i] is FillableShape fillableShape) // FillColor
            //{
            //    foreach (Shape shape in ShapeList)
            //    {
            //        if (shape != fillableShape)
            //        {
            //            shape.IsSelected = false;
            //        }
            //    }
            //    fillableShape.IsSelected = true;
            //    fillableShape.FillColor = Color.Yellow;
            //}
        }

        /// <summary>
        /// Транслация на избраният елемент на вектор определен от <paramref name="p>p</paramref>
        /// </summary>
        /// <param name="p">Вектор на транслация.</param>
        public void TranslateTo(PointF p)
        {
            //if (selection != null)
            //{
            //    selection.Location = new PointF(selection.Location.X + p.X - lastLocation.X, selection.Location.Y + p.Y - lastLocation.Y);
            //    lastLocation = p;
            //}

            if (selection.Count > 0) // > pove4e ot 0 elementa; selection != null
            {
                foreach (Shape item in Selection)
                {
                    item.Location = new PointF(
                        item.Location.X + p.X - lastLocation.X,
                        item.Location.Y + p.Y - lastLocation.Y);
                }
                lastLocation = p;
            }
        }

        public void GroupPrimitives()
        {
            // Check if selection contains > 1 => pove4e ot 1 element
            // Calculate bounding box -> obhva6ta6t tri1g1lnik
            //  Define minX koordinata po goren primitiv = float.MaxValue, minY - i viso4inata na primitiva, maxX - nai-lqv primitiv = float.MinValue, maxY
            //  for every item in Selection za6toto SubShape e void
            //   if item.X < minX -> minX = item.X
            //   if item.Y < minY -> minY = item.Y
            //   if item.X + item.Width > maxX -> maxX = item.X + item.Width
            //   If item.Y + item.Height > maxY -> maxY = item.Y + item.Height

            // new GroupShape insatnce with the bbox
            // assign(prisvoqvane) Selection to the SubShapes preporty
            // add the instance to the ShapeList
            //if (Selection.Count > 1)
            //{
            //    float minX = float.MaxValue;
            //    float minY = float.MaxValue;
            //    float maxX = float.MinValue;
            //    float maxY = float.MinValue;

            //    foreach (var item in Selection)
            //    {
            //        if (item.Location.X < minX)
            //            minX = item.Location.X;
            //        if (item.Location.Y < minY)
            //            minY = item.Location.Y;
            //        if (item.Location.X + item.Width > maxX)
            //            maxX = item.Location.X + item.Width;
            //        if (item.Location.Y + item.Height > maxY)
            //            maxY = item.Location.Y + item.Height;
            //    }

            //    var groupShape = new GroupShape(new RectangleF(minX, minY, maxX - minX, maxY - minY));
            //    groupShape.SubShapes = Selection;
            //    ShapeList.Add(groupShape);
            //}            //if (Selection.Count > 1)
            //{
            //    float minX = float.MaxValue;
            //    float minY = float.MaxValue;
            //    float maxX = float.MinValue;
            //    float maxY = float.MinValue;

            //    foreach (var item in Selection)
            //    {
            //        if (item.Location.X < minX)
            //            minX = item.Location.X;
            //        if (item.Location.Y < minY)
            //            minY = item.Location.Y;
            //        if (item.Location.X + item.Width > maxX)
            //            maxX = item.Location.X + item.Width;
            //        if (item.Location.Y + item.Height > maxY)
            //            maxY = item.Location.Y + item.Height;
            //    }

            //    var groupShape = new GroupShape(new RectangleF(minX, minY, maxX - minX, maxY - minY));
            //    groupShape.SubShapes = Selection;
            //    ShapeList.Add(groupShape);
            //}

            if (Selection.Count < 2) return;

            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;

            foreach (var item in Selection)
            {
                if (item.Location.X < minX)
                    minX = item.Location.X;
                if (item.Location.Y < minY)
                    minY = item.Location.Y;
                if (item.Location.X + item.Width > maxX)
                    maxX = item.Location.X + item.Width;
                if (item.Location.Y + item.Height > maxY)
                    maxY = item.Location.Y + item.Height;
            }

            GroupShape group = new GroupShape(new RectangleF(minX, minY, maxX - minX, maxY - minY));

            group.SubShapes = Selection;
            Selection = new List<Shape>();

            ShapeList.Add(group);

            foreach (Shape item in group.SubShapes)
            {
                ShapeList.Remove(item);
            }

            ShapeList.Add(group);
        }
        public void UngroupPrimitives()
        {
            //if (Selection.Count != 1 || !(Selection[0] is GroupShape)) return; // NOPE
            //// Create a temporary list to hold the sub-shapes of the selected group shape.
            //List<Shape> subShapes = new List<Shape>();
            //// Loop through all shapes in the selected group shape and add them to the temporary list.
            //GroupShape group = (GroupShape)Selection[0];
            //foreach (Shape subShape in group.SubShapes)
            //{
            //    subShapes.Add(subShape);
            //}
            //// Remove the selected group shape from the ShapeList.
            //ShapeList.Remove(group);
            //// Add the sub-shapes to the ShapeList.
            //foreach (Shape subShape in subShapes)
            //{
            //    ShapeList.Add(subShape);
            //}
            //// Clear the selection and add the sub-shapes to the selection.
            //Selection.Clear();
            //foreach (Shape subShape in subShapes)
            //{
            //    Selection.Add(subShape);
            //}
            ////ShapeList.Add(group);

            //if (selection1 != null) // OLD
            //{
            //    Selection.Clear();
            //    Selection.Add(selection1);
            //    foreach (GroupShape gsh in Selection)
            //    {
            //        foreach (Shape s in gsh.SubShapes)
            //        {
            //            ShapeList.Add(s);
            //            Selection.Add(s);
            //            selection1 = s;
            //        }
            //        gsh.SubShapes.Clear();
            //        Selection.Remove(gsh);
            //        ShapeList.Remove(gsh);
            //        break;
            //    }
            //}

            if (selection.Count > 0)
            {
                foreach (Shape shape in selection)
                {
                    if (shape.GetType() == typeof(GroupShape))
                    {
                        foreach (Shape gsh in ((GroupShape)shape).SubShapes)
                        {
                            ShapeList.Add(gsh);
                        }

                        ((GroupShape)shape).SubShapes.Clear();
                        ShapeList.Remove(shape);

                    }
                }

                selection.Clear();
            }
        }
        internal void Delete()
        {
            foreach (Shape item in selection)
            {
                ShapeList.Remove(item);
            }

            selection.Clear();
        }
        public void SelectAll()
        {
            if (ShapeList.Count > 0)
            {
                Selection.Clear();
                Selection.AddRange(ShapeList);
                Selection1 = Selection.Last();
            }
        }

        public void SetStrokeWidth(float width)
        {
            if (Selection.Count > 0)
            {
                foreach (Shape item in Selection)
                {
                    item.StrokeWidth = width;
                }
            }
        }
        public void SetStrokeColor(Color color)
        {
            if (Selection.Count > 0)
            {
                foreach (Shape item in Selection)
                {
                    item.StrokeColor = color;
                }
            }
        }
        public void SetFillColor(Color c)
        {
            foreach (Shape item in selection)
            {
                if (Selection != null)
                    item.FillColor = c;
            }
        }
        public void SetOpacity(int opacity)
        {
            if (Selection.Count > 0)
            {
                foreach (Shape item in Selection)
                {
                    item.Opacity = opacity;
                }
            }
        }
        //public void SetOpacityBorder(int transparency) // TOVA
        //{
        //    if (Selection.Count > 0)
        //    {
        //        foreach (Shape item in Selection)
        //        {
        //            item.Transparency = transparency;
        //        }
        //    }
        //} // TYK
        public void SetRotation(float rotation)
        {
            if (Selection.Count > 0)
            {
                foreach (Shape item in Selection)
                {
                    item.Rotation = rotation;
                }
            }
        }
        public void RotatePrimitive(int angle) // NEW mnojestvena selekciq
        {
            if (Selection.Count > 0)
            {
                foreach (Shape item in Selection)
                {
                    // create new Matrix instance
                    // copy item.TransformationMatrix in new instance
                    // new instance RotateAt ...
                    // Assign new instance of item.TransformationMatrix
                    item.TransformationMatrix.RotateAt(angle, new PointF(
                                               item.Location.X + item.Width / 2,
                                               item.Location.Y + item.Height / 2
                                           )
                                        ); // scale po X i po Y; Shear izkrivqvane nastrani;
                    // gridienti za prelivane na cveta; S cvetove butoni da se napravi tazi prelivka
                }
            }
        }
        public void RotateAt(int grad)
        {
            if (selection.Count > 0)
            {
                foreach (Shape item in selection)
                {
                    item.Rotates(grad);
                }
            }
        }
        public void Resize(int p, int p2)
        {
            if (selection.Count > 0)
            {
                foreach (Shape item in selection)
                {
                    item.Resize(p, p2);
                }
            }
        }
        public void RotatePrimitive2(int scaleFactorX, int scaleFactorY) // NEW selection
        {
            foreach (Shape item in Selection)
            {
                item.TransformationMatrix.Scale(scaleFactorX, scaleFactorY);
            }
        }

        // TODO k1de e kursora; zapazva posledno izbraniq cvqt; modalen dialog za stoinosti

        public override void Draw(Graphics grfx)
        {
            base.Draw(grfx); // Vika draw metoda na bazoviq klas

            float[] dashValues = { 4, 2 };
            //Pen dashPen = new Pen(Color.Black, 3); // Black/Gold
            //dashPen.DashPattern = dashValues;
            Pen dashPen = new Pen(Color.Black, 3)
            {
                DashPattern = dashValues
            };

            if (Selection.Count > 0)
            {
                foreach (Shape item in Selection)
                {
                    // New selection
                    grfx.DrawRectangle(
                                        dashPen,
                                        item.Location.X - 3,
                                        item.Location.Y - 3,
                                        item.Width + 6,
                                        item.Height + 6
                                        );
                }

                //if (Selection != null) // Default Selection
                //{
                //    grfx.DrawRectangle(Pens.Black, selection.Location.X - 3, selection.Location.Y - 3, Selection.Width + 6, Selection.Height + 6);
                //}
            }
        }

        internal void Save(string filename)
        {
            using (FileStream filestream = new FileStream(filename, FileMode.Create))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(filestream, ShapeList);
            }
        }

        internal void Open(string filename)
        {
            using (FileStream filestream = new FileStream(filename, FileMode.Open))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                ShapeList = (List<Shape>)binaryFormatter.Deserialize(filestream);
            }
        }

        // TODO end
    }
}