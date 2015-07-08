namespace Myxini.Recognition
{
	interface IClassifier
	{
		IBlock Clustering(Image.IImage block);
	}
}
