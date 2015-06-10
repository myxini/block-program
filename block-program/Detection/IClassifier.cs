namespace Myxini.Recognition
{
	interface IClassifier
	{
		IBlock Clustering(Raw.IRawBlock block);
	}
}
