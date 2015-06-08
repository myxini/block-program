using Myxini.Recognition.Image;

namespace Myxini.Recognition.Raw
{
	public interface ILine
	{
		IImage BoundingImage { get; }
		Point EndPoint { get; }
	}
}
