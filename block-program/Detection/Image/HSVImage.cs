using Myxini.Recognition.Raw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myxini.Recognition.Image
{
    public class HSVImage : IImage
    {
		public HSVImage(int width, int height)
		{
			this.BoundingBox = new Rectangle(0, 0, width, height);
			this.OriginalSize = new Size(width, height);
			this.Channel = 3;
			this.IsRegionOfImage = false;
			this.Pixels = new byte[width * height * this.Channel];
		}

		public HSVImage(byte[] pixels, int width, int height, int stride = 3) : this(width, height)
		{
			//pixels.CopyTo(this.Pixels, 0;
			for (int i = 0; i < width * height; ++i)
			{
                int b = pixels[i * stride + 0];
                int g = pixels[i * stride + 1];
                int r = pixels[i * stride + 2];
                System.Drawing.Color color = System.Drawing.Color.FromArgb(
                    pixels[i * stride + 0],
                    pixels[i * stride + 1],
                    pixels[i * stride + 2]
                );
                this.Pixels[i * this.Channel + 0] = (byte) (color.GetHue() * 255 / 360); // H
                this.Pixels[i * this.Channel + 1] = (byte) (color.GetSaturation() * 255); // S
				this.Pixels[i * this.Channel + 2] = (byte) (color.GetBrightness() * 255); // V
			}
		}

		public HSVImage(HSVImage image, Rectangle region)
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

		public HSVImage(HSVImage image, Func<IImage, int, int, int, int> convertor)
			: this(image.Width, image.Height)
		{
			for(int y = 0;y < this.Height; ++y)
			{
				for(int x = 0; x < this.Width; ++x)
				{
					this.Pixels[(y * this.Width + x) * this.Channel + 0] = (byte)convertor(image, x, y, 0);
					this.Pixels[(y * this.Width + x) * this.Channel + 1] = (byte)convertor(image, x, y, 1);
					this.Pixels[(y * this.Width + x) * this.Channel + 2] = (byte)convertor(image, x, y, 2);
				}
			}
		}

		public HSVImage(HSVImage lhs, HSVImage rhs, Func<IImage, IImage, int, int, int, int> convertor)
			: this(lhs.Width, lhs.Height)
		{
			for (int c = 0; c < lhs.Channel; ++c)
			{
				for (int y = 0; y < this.Height; ++y)
				{
					for (int x = 0; x < this.Width; ++x)
					{
						this.Pixels[y * this.Width + x] = (byte)convertor(lhs, rhs, x, y, c);
					}
				}
			}
		}

		public int GetElement(int x, int y, int channel)
		{
			if(x < 0 || y < 0 || x >= this.Width || y >= this.Height || channel < 0 || channel > this.Channel)
			{
				throw new ArgumentOutOfRangeException();
			}

			return this.Pixels[
				(this.OriginalSize.Width * (this.BoundingBox.Y + y) + this.BoundingBox.X + x) * this.Channel + channel];
		}

		public IImage Create(Func<IImage, int, int, int, int> convertor)
		{
			return new HSVImage(this, convertor);
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
			var w = Math.Min(x + width, this.Width) - x;
			var h = Math.Min(y + height, this.Height) - y;
			return new HSVImage(this, new Rectangle(x, y, w, h));
		}

		/// <summary>
		/// 画像の一部を取得します
		/// </summary>
		/// <param name="region">部分画像の領域</param>
		/// <returns>部分画像</returns>
		public IImage RegionOfImage(Rectangle region)
		{
			return new HSVImage(this, region);
		}

		/// <summary>
		/// 画像のディープコピーを返します
		/// </summary>
		/// <returns>ディープコピーした画像</returns>
		public IImage Clone()
		{
			return new HSVImage(this.Pixels, this.Width, this.Height);
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
