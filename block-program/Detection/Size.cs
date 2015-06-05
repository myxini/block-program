using System;

namespace Myxini.Detection.Raw
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

		public int Width;
		public int Height;
	}
}
