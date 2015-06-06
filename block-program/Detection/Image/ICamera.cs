namespace Myxini.Detection.Image
{
	interface ICamera
	{
		/// <summary>
		/// 帰ら画像を取得します
		/// </summary>
		/// <returns>取得したカメラ画像</returns>
		IImage Capture();
	}
}
