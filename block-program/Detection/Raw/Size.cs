using System;

namespace Myxini.Recognition.Raw
{
	public struct Size
	{
		public Size(int width, int height)
		{
			this.Width = width;
			this.Height = height;
		}

		public override string ToString()
		{
			return (String.Format("{0},{1}", this.Width, this.Height));
		}

		public int Area
		{
			get { return this.Width * this.Height; }
		}

		public static bool operator ==(Size a, Size b)
		{
			return a.Width == b.Width && a.Height == b.Height;
		}

		public static bool operator !=(Size a, Size b)
		{
			return a.Width != b.Width || a.Height != b.Height;
		}


		public int Width;
		public int Height;
	}
}
