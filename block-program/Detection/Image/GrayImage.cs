using System;

namespace Myxini.Recognition.Image
{
	using Rectangle = Raw.Rectangle;
	using Size = Raw.Size;

	public class GrayImage : 
		IImage
	{
		public enum ImageType
		{
			RGB, BGR, ARGB, RGBA
		}

		public GrayImage(int width, int height)
		{
			this.BoundingBox = new Rectangle(0, 0, width, height);
			this.OriginalSize = new Size(width, height);
			this.Channel = 1;
			this.IsRegionOfImage = false;
			this.Pixels = new byte[width * height];
		}

		public GrayImage(byte[] pixel, int width, int height) : this(width, height)
		{
			pixel.CopyTo(this.Pixels, 0);
		}

		public GrayImage(IImage image, ImageType image_type = ImageType.RGB) : this(image.Width, image.Height)
		{
			int[] place = new int[3];

			switch(image_type)
			{
				case ImageType.RGB:
				case ImageType.RGBA:
					place[0] = 2;
					place[1] = 1;
					place[2] = 0;
					break;

				case ImageType.BGR:
					place[0] = 0;
					place[1] = 1;
					place[2] = 2;
					break;

				case ImageType.ARGB:
					place[0] = 3;
					place[1] = 2;
					place[2] = 1;
					break;
			}

			for (int y = 0; y < image.Height; ++y)
			{
				for (int x = 0; x < image.Width; ++x)
				{
					var b = image.GetElement(x, y, place[0]);
					var g = image.GetElement(x, y, place[1]);
					var r = image.GetElement(x, y, place[2]);

					this.Pixels[y * image.Width + x] = (byte)(0.299 * r + 0.587 * g + 0.114 * b);
				}
			}
		}

		public GrayImage(GrayImage image, Rectangle region)
		{
			this.Channel = image.Channel;
			this.Pixels = image.Pixels;
			this.OriginalSize = image.OriginalSize;
			this.IsRegionOfImage = true;

			this.BoundingBox = new Rectangle(
				image.BoundingBox.X + region.X,
				image.BoundingBox.Y + region.Y,
				region.Width,
				region.Height
				);
		}

		public GrayImage(GrayImage image, Func<IImage, int, int, int, int> convertor) : this(image.Width, image.Height)
		{
			for(int y = 0;y < this.Height; ++y)
			{
				for(int x = 0; x < this.Width; ++x)
				{
					this.Pixels[y * this.Width + x] = (byte)convertor(image, x, y, 0);
				}
			}
		}

		public GrayImage(GrayImage lhs, GrayImage rhs, Func<IImage, IImage, int, int, int, int> convertor)
			: this(lhs.Width, lhs.Height)
		{
			for (int y = 0; y < this.Height; ++y)
			{
				for (int x = 0; x < this.Width; ++x)
				{
					this.Pixels[y * this.Width + x] = (byte)convertor(lhs, rhs, x, y, 0);
				}
			}
		}

		public int GetElement(int x, int y, int channel = 0)
		{
			if (x < 0 || y < 0 || x >= this.Width || y >= this.Height || channel < 0 || channel > this.Channel)
			{
				throw new ArgumentOutOfRangeException();
			}

			return this.Pixels[
				(this.OriginalSize.Width * (this.BoundingBox.Y + y) + this.BoundingBox.X + x) * this.Channel + channel];
			//return this.Pixels[(this.Width * y + x) * this.Channel + channel];
		}

		public IImage Create(Func<IImage, int, int, int, int> convertor)
		{
			return new GrayImage(this, convertor);
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
			return new GrayImage(this, new Rectangle(x, y, width, height));
		}

		/// <summary>
		/// 画像の一部を取得します
		/// </summary>
		/// <param name="region">部分画像の領域</param>
		/// <returns>部分画像</returns>
		public IImage RegionOfImage(Rectangle region)
		{
			return new GrayImage(this, region);
		}

		/// <summary>
		/// 画像のディープコピーを返します
		/// </summary>
		/// <returns>ディープコピーした画像</returns>
		public IImage Clone()
		{
			return new GrayImage(this.Pixels, this.Width, this.Height);
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
