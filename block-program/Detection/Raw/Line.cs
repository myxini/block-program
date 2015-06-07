using Myxini.Recognition.Image;

namespace Myxini.Recognition.Raw
{
	public class Line : ILine
	{
		public IImage BoundingImage { get; protected set; }
		public Point BeginPoint { get; protected set; }
		public Point EndPoint { get; protected set; }
	}
}
