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

		public int X { get { return this.Position.X; } }
		public int Y { get { return this.Position.Y; } }

		public int Width { get { return this.BoundingSize.Width; } }
		public int Height { get { return this.BoundingSize.Height; } }

		public Point Position;
		public Size BoundingSize;
	}
}
