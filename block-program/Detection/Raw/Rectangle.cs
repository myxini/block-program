namespace Myxini.Recognition.Raw
{
	public struct Rectangle
	{
		public Rectangle(int x, int y, int width, int height)
			: this(new Point(x, y), new Size(width, height))
		{
		}

		public Rectangle(Point position, Size bounding_size)
		{
			this.Position = position;
			this.BoundingSize = bounding_size;
		}

		public int X { get { return this.Position.X; } set { this.Position.X = value; } }
		public int Y { get { return this.Position.Y; } set { this.Position.Y = value; } }

		public int Width { get { return this.BoundingSize.Width; } set { this.BoundingSize.Width = value; } }
		public int Height { get { return this.BoundingSize.Height; } set { this.BoundingSize.Height = value; } }

		public Point Position;
		public Size BoundingSize;
	}
}
