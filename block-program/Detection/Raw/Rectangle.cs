using System;

namespace Myxini.Recognition.Raw
{
	public struct Rectangle
	{
		public Rectangle(int x, int y, int width, int height)
			: this(new Point(x, y), new Size(width, height))
		{
		}

		public Rectangle(Point left_top, Point right_bottom)
			: this(left_top, new Size(Math.Abs(right_bottom.X - left_top.X), Math.Abs(right_bottom.Y - left_top.Y)))
		{
		}

		public Rectangle(Point position, Size bounding_size)
		{
			this.Position = position;
			this.BoundingSize = bounding_size;
		}

		public static bool operator ==(Rectangle a, Rectangle b)
		{
			return a.Position == b.Position && a.BoundingSize == b.BoundingSize;
		}

		public static bool operator !=(Rectangle a, Rectangle b)
		{
			return a.Position != b.Position || a.BoundingSize != b.BoundingSize;
		}

		public int X { get { return this.Position.X; } set { this.Position.X = value; } }
		public int Y { get { return this.Position.Y; } set { this.Position.Y = value; } }

		public int Width { get { return this.BoundingSize.Width; } set { this.BoundingSize.Width = value; } }
		public int Height { get { return this.BoundingSize.Height; } set { this.BoundingSize.Height = value; } }

		public Point Position;
		public Size BoundingSize;
	}
}
