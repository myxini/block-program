﻿using System;


namespace Myxini.Recognition.Image
{
	using Rectangle = Raw.Rectangle;
	using Size = Raw.Size;

	public interface IImage
	{　
		/// <summary>
		/// 画像の要素を取得します
		/// </summary>
		/// <param name="x">画像のX座標</param>
		/// <param name="y">画像のY座標</param>
		/// <param name="channel">画像のチャネル</param>
		/// <returns>画像の要素</returns>
		int GetElement(int x, int y, int channel);

		/// <summary>
		/// コンストラクタを使用せずにオブジェクトを生成します
		/// </summary>
		/// <param name="convertor">初期化関数</param>
		/// <returns>生成した画像オブジェクト</returns>
		IImage Create(Func<IImage, int, int, int, int> convertor);

		/// <summary>
		/// 画像の一部を取得します
		/// </summary>
		/// <param name="x">部分画像の領域のX座標</param>
		/// <param name="y">部分画像の領域のY座標</param>
		/// <param name="width">部分画像の領域の幅</param>
		/// <param name="height">部分画像の領域の高さ</param>
		/// <returns>部分画像</returns>
		IImage RegionOfImage(int x, int y, int width, int height);
		
		/// <summary>
		/// 画像の一部を取得します
		/// </summary>
		/// <param name="region">部分画像の領域</param>
		/// <returns>部分画像</returns>
		IImage RegionOfImage(Rectangle region);

		/// <summary>
		/// 画像のディープコピーを返します
		/// </summary>
		/// <returns>ディープコピーした画像</returns>
		IImage Clone();

		/// <summary>
		/// 画像のチャンネル数
		/// </summary>
		int Channel { get; }
		/// <summary>
		/// 画像の領域
		/// この画像が部分画像の時，X,Yには元画像とのオフセットが指定されます．
		/// </summary>
		Rectangle BoundingBox { get; }
		/// <summary>
		/// 画像の幅
		/// </summary>
		int Width { get; }
		/// <summary>
		/// 画像の高さ
		/// </summary>
		int Height { get; }

		/// <summary>
		/// この画像が部分画像かを返します
		/// </summary>
		bool IsRegionOfImage { get; }
	}
}