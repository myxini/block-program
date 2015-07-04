using Myxini.Recognition.Image;
using Myxini.Recognition.Raw;

namespace Myxini.Recognition
{
	public interface IBoard
	{
		/// <summary>
		/// カメラ画像を元にキャリブレーションを行う
		/// </summary>
		/// <param name="camera">キャリブレーションするカメラを指定します</param>
		void Calibration(ICamera camera);
		
		/// <summary>
		/// ブロックのサイズ[mm]から画像中のブロックのサイズ[pixel]に変換する
		/// </summary>
		/// <param name="size">ブロックのサイズ[mm]</param>
		/// <returns>ブロックのサイズ[pixel]</returns>
		Size GetBlockSize(Size size);

		/// <summary>
		/// キャリブレーションした時の画像から背景を取り除いた画像を返します
		/// </summary>
		/// <param name="image">入力画像</param>
		/// <returns>背景を削除した画像</returns>
		IImage GetBackgroundDeleteImage(IImage image);
	}
}
