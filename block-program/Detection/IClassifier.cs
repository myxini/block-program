namespace Myxini.Detection
{
	interface IClassifier
	{
		IBlock clustering(Raw.IBlock block);
	}
}
