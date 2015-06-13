﻿
namespace Myxini.Recognition.Image
{
	using Rectangle = Raw.Rectangle;
	using Size = Raw.Size;

	public class ColorImage : IImage
	{
		public ColorImage(byte[] pixels, int width, int height)
		{
			this.BoundingBox = new Rectangle(0, 0, width, height);
			this.OriginalSize = this.BoundingBox.BoundingSize;
			this.Channel = 3;
			this.IsRegionOfImage = false;
			pixels.CopyTo(this.Pixels, 0);
		}

		public ColorImage(ColorImage image, Rectangle region)
		{
			this.Channel = image.Channel;
			this.Pixels = image.Pixels;
			this.OriginalSize = this.OriginalSize;
			this.IsRegionOfImage = true;

			Rectangle new_region = new Rectangle(
				image.BoundingBox.X + region.X,
				image.BoundingBox.Y + region.Y,
				region.Width,
				region.Height
				);
		}

		public int GetElement(int x, int y, int channel)
		{
			return this.Pixels[
				(this.OriginalSize.Width * this.BoundingBox.Y + this.BoundingBox.X +	/// 画像全体での部分画像の位置
				this.BoundingBox.Width * y + x) * this.Channel + channel];						/// 部分画像内での位置
			//return this.Pixels[(this.Width * y + x) * this.Channel + channel];
		}

		/// <summary>
		/// 画像の一部を取得します
		/// </summary>
		/// <param name="x">部分画像の領域のX座標</param>
		/// <param name="y">部分画像の領域のY座標</param>
		/// <param name="width">部分画像の領域の幅</param>
		/// <param name="height">部分画像の領域の高さ</param>
		/// <returns>部分画像</returns>
		public IImage RegionOfImage(int x, int y, int width, int height)
		{
			return new ColorImage(this, new Rectangle(x, y, width, height));
		}

		/// <summary>
		/// 画像の一部を取得します
		/// </summary>
		/// <param name="region">部分画像の領域</param>
		/// <returns>部分画像</returns>
		public IImage RegionOfImage(Rectangle region)
		{
			return new ColorImage(this, region);
		}

		/// <summary>
		/// 画像のディープコピーを返します
		/// </summary>
		/// <returns>ディープコピーした画像</returns>
		public IImage Clone()
		{
			return new ColorImage(this.Pixels, this.Width, this.Height);
		}

		/// <summary>
		/// 画像のチャンネル数
		/// </summary>
		public int Channel { get; private set; }
		/// <summary>
		/// 画像の領域
		/// この画像が部分画像の時，X,Yには元画像とのオフセットが指定されます．
		/// </summary>
		public Rectangle BoundingBox { get; private set; }

		/// <summary>
		/// 画像の幅
		/// </summary>
		public int Width
		{
			get
			{
				return this.BoundingBox.Width;
			}
		}
		/// <summary>
		/// 画像の高さ
		/// </summary>
		public int Height 
		{
			get
			{
				return this.BoundingBox.Height;
			}
		}

		/// <summary>
		/// この画像が部分画像かを返します
		/// </summary>
		public bool IsRegionOfImage { get; private set; }

		/// <summary>
		/// 画像の画素値が保存されています
		/// </summary>
		public byte[] Pixels { get; private set; }

		/// <summary>
		/// 部分画像の時に原画像の幅と高さを取得する
		/// </summary>
		private Size OriginalSize { get; set; }
	}
}
