using System;

namespace Myxini.Detection.Raw
{
	public struct Point
	{
		public Point(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}

		public override string ToString()
		{
			return (String.Format("{0},{1}", this.X, this.Y));
		}

		public int X;
		public int Y;
	}
}
