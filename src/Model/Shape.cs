using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Draw
{
	/// <summary>
	/// Базовия клас на примитивите, който съдържа общите характеристики на примитивите.
	/// </summary>
	public abstract class Shape
	{
		#region Constructors
		
		public Shape()
		{
		}
		
		public Shape(RectangleF rect)
		{
			rectangle = rect;
		}
		
		public Shape(Shape shape)
		{
			this.Height = shape.Height;
			this.Width = shape.Width;
			this.Location = shape.Location;
			this.rectangle = shape.rectangle;
			
			this.FillColor =  shape.FillColor;
		}
        #endregion

        #region Properties

        public virtual float ScaleFactor { get; set; }

        /// <summary>
        /// Обхващащ правоъгълник на елемента.
        /// </summary>
        private RectangleF rectangle;		
		public virtual RectangleF Rectangle {
			get { return rectangle; }
			set { rectangle = value; }
		}
		
		/// <summary>
		/// Широчина на елемента.
		/// </summary>
		public virtual float Width {
			get { return Rectangle.Width; }
			set { rectangle.Width = value; }
		}
		
		/// <summary>
		/// Височина на елемента.
		/// </summary>
		public virtual float Height {
			get { return Rectangle.Height; }
			set { rectangle.Height = value; }
		}
		
		/// <summary>
		/// Горен ляв ъгъл на елемента.
		/// </summary>
		public virtual PointF Location
        { // naslednicite na shape class property se overrid-va. getter i setter
            get { return Rectangle.Location; }
			set { rectangle.Location = value; }
		}

        public virtual PointF Center
        {
            get { return new PointF(
				this.Location.X + this.Width / 2, 
				this.Location.Y + this.Height / 2
				); 
			}
        }

        /// <summary>
        /// Цвят на елемента.
        /// </summary>
        private Color fillColor;		
		public virtual Color FillColor {
			get { return fillColor; }
			set { fillColor = value; }
		}

		private Color strokeColor = Color.Aqua;
		public virtual Color StrokeColor
		{
			get { return strokeColor; }
			set { strokeColor = value; }
		}

        private float rotation = 0;
        public virtual float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        private float strokeWidth = 1; // Default = 1, moje 5 i t.n.
        public virtual float StrokeWidth
        {
            get { return strokeWidth; }
            set { strokeWidth = value; }
        }

		private int transparency = 255; // TOVA DEL
		public virtual int Transparency
		{
			get { return transparency; }
			set { transparency = value; }
		} // TYK

		private int opacity = 255;
        public virtual int Opacity
        {
            get { return opacity; }
            set { opacity = value; }
        }

		[NonSerialized]
		private Matrix transformationMatrix = new Matrix(); // Matrix ne se Serializira. trqbva da se otamani :D
        public virtual Matrix TransformationMatrix
		{
			get { return transformationMatrix; }
			set { transformationMatrix = value; }
		}

        #endregion

        public virtual void Translate(PointF vector)
        {
            int backRotation = (int)rotation;
            Rotates(-backRotation);
            rotation = backRotation;

            Location = new PointF(Location.X + vector.X, Location.Y + vector.Y);

            Rotates(backRotation);
            rotation = backRotation;
        }

        public virtual void Rotates(int radian)
        {
            TransformationMatrix.RotateAt(radian, Center);
            rotation += radian;
        }
        public virtual void Resize(float atX, float atY)
        {
            int backRotation = (int)rotation;
            Rotates(-backRotation);
            rotation = backRotation;

            Width += atX;
			Height += atY;

            Rotates(backRotation);
            rotation = backRotation;
        }

        /// <summary>
        /// Проверка дали точка point принадлежи на елемента.
        /// </summary>
        /// <param name="point">Точка</param>
        /// <returns>Връща true, ако точката принадлежи на елемента и
        /// false, ако не пренадлежи</returns>
        public virtual bool Contains(PointF point)
		{
			return Rectangle.Contains(point.X, point.Y);
		}
		
		/// <summary>
		/// Визуализира елемента.
		/// </summary>
		/// <param name="grfx">Къде да бъде визуализиран елемента.</param>
		public virtual void DrawSelf(Graphics grfx)
		{
            //shape.Rectangle.Inflate(shape.BorderWidth, shape.BorderWidth); // ???
            //rectangle.Inflate(StrokeWidth, StrokeWidth); // work
        }
        // TODO end
    }
}