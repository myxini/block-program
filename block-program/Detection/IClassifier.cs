using Myxini.Recognition.Image;
namespace Myxini.Recognition
{
	public interface IClassifier
	{
		IBlock Clustering(IImage raw_block, IPatternMatchingAlgorithm algorithm);
	}
}
