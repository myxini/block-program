namespace Myxini.Detection.Raw
{
	interface IBlock
	{
		Image.IImage BoundingImage { get; protected set; }
	}
}
