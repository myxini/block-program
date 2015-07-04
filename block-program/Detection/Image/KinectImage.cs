using System;

namespace Myxini.Recognition.Image
{
	using Rectangle = Raw.Rectangle;
	using Size = Raw.Size;

	class KinectImage : IImage
	{
		public KinectImage(ColorImage color, DepthImage depth)
		{
			this.Color = color;
			this.Depth = depth;
			this.BoundingBox = color.BoundingBox;
			this.Channel = 4;
			this.OriginalSize = this.BoundingBox.BoundingSize;
			this.IsRegionOfImage = false;
		}

		public KinectImage(short[] depth, byte[] color, int width, int height)
		{
			this.Depth = new DepthImage(depth, width, height);
			this.Color = new ColorImage(color, width, height);

			this.BoundingBox = new Rectangle(0, 0, width, height);
			this.OriginalSize = this.BoundingBox.BoundingSize;
			this.Channel = 4;
			this.IsRegionOfImage = false;
		}

		public KinectImage(KinectImage image, Rectangle region)
		{
			this.Channel = image.Channel;
			this.OriginalSize = this.OriginalSize;
			this.IsRegionOfImage = true;

			this.Color = new ColorImage(image.Color, region);
			this.Depth = new DepthImage(image.Depth, region);

			Rectangle new_region = new Rectangle(
				image.BoundingBox.X + region.X,
				image.BoundingBox.Y + region.Y,
				region.Width,
				region.Height
				);
		}

		public int GetElement(int x, int y, int channel)
		{
			if(channel == 0)
			{
				return this.Depth.GetElement(x, y, 0);
			}
			else
			{
				return this.Color.GetElement(x, y, channel - 1);
			}

			throw new ArgumentOutOfRangeException();
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
			return new KinectImage(this, new Rectangle(x, y, width, height));
		}

		/// <summary>
		/// 画像の一部を取得します
		/// </summary>
		/// <param name="region">部分画像の領域</param>
		/// <returns>部分画像</returns>
		public IImage RegionOfImage(Rectangle region)
		{
			return new KinectImage(this, region);
		}

		/// <summary>
		/// 画像のディープコピーを返します
		/// </summary>
		/// <returns>ディープコピーした画像</returns>
		public IImage Clone()
		{
			return new KinectImage(this.Color, this.Depth);
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
		/// 深度画像の画素値が保存されています
		/// </summary>
		public short[] DepthPixels 
		{
			get
			{
				return this.DepthPixels;
			}
		}

		/// <summary>
		/// カラー画像の画素値が保存されています
		/// </summary>
		public byte[] ColorPixels 
		{
			get
			{
				return this.Color.Pixels;
			}
		}

		/// <summary>
		/// 部分画像の時に原画像の幅と高さを取得する
		/// </summary>
		private Size OriginalSize { get; set; }

		private ColorImage Color;
		private DepthImage Depth;
	}
}
