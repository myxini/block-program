namespace Myxini.Recognition.Image
{
	public interface ICamera
	{
		/// <summary>
		/// カメラ画像を取得します
		/// </summary>
		/// <returns>取得したカメラ画像</returns>
		IImage Capture();
	}
}
