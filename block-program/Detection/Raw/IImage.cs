namespace Myxini.Detection.Raw.Image
{
	interface IImage
	{

		int Width 
		{
			get { return this.BoundingBox.Width; }
		}
		int Channel { get; protected set; }
		Rectangle BoundingBox { get; protected set; }
	}
}