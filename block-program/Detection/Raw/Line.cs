using Myxini.Detection.Image;

namespace Myxini.Detection.Raw
{
	public class Line : ILine
	{
		public IImage BoundingImage { get; protected set; }
		public Point BeginPoint { get; protected set; }
		public Point EndPoint { get; protected set; }
	}
}
