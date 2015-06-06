using Myxini.Detection.Raw;

namespace Myxini.Detection.Image
{
	interface IImage
	{
		E GetElement<E>(int x, int y, int channel);

		IImage RegionOfImage(int x, int y, int width, int height);
		IImage RegionOfImage(Rectangle region);


		int Channel { get; protected set; }
		Rectangle BoundingBox { get; protected set; }
		int Width
		{
			get { return this.BoundingBox.Width; }
		}
		int Height
		{
			get { return this.BoundingBox.Height; }
		}

		bool IsRegionOfImage { get; protected set; }
	}
}