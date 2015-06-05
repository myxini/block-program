namespace Myxini.Detection.Raw
{
	public interface ILine
	{
		Image.IImage BoundingImage { get; protected set; }
		Point EndPoint { get; protected set; }
	}
}
