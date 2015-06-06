using Myxini.Detection.Raw;

namespace Myxini.Detection.Image
{
	public interface IImage
	{
		E GetElement<E>(int x, int y, int channel);

		IImage RegionOfImage(int x, int y, int width, int height);
		IImage RegionOfImage(Rectangle region);


		int Channel { get; }
		Rectangle BoundingBox { get; }
		int Width { get; }
		int Height { get; }

		bool IsRegionOfImage { get; }
	}
}