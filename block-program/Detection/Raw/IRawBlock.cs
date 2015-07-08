using Myxini.Recognition.Image;

namespace Myxini.Recognition.Raw
{
	interface IRawBlock
	{
		IImage BoundingImage { get; }
	}
}
