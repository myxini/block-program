using Myxini.Detection.Image;

namespace Myxini.Detection
{
	interface IDetector
	{
		bool Detection(IImage image, out Raw.BlockArray blocks, out Raw.LineArray lines);
	}
}
