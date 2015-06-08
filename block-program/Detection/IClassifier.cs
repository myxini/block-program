namespace Myxini.Recognition
{
	interface IClassifier
	{
		IBlock clustering(Raw.IBlock block);
	}
}
