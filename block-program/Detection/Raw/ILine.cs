using Myxini.Detection.Image;

namespace Myxini.Detection.Raw
{
	public interface ILine
	{
		IImage BoundingImage { get; }
		Point EndPoint { get; }
	}
}
