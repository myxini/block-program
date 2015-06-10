namespace Myxini.Recognition
{
	interface IClassifier
	{
		IBlock clustering(Raw.IRawBlock block);
	}
}
