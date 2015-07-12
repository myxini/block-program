using System;
using Myxini.Recognition.Image;
using Myxini.Recognition.Raw;

namespace Myxini.Recognition
{
	public class WhiteBoard : IBoard
	{
		public WhiteBoard(int iteration = 4)
		{
			this.Iteration = iteration;
		}

		public void Calibration(ICamera camera)
		{
			double min = double.MaxValue, max = double.MaxValue;
			for(int i = 0; i < this.Iteration; ++i)
			{
				var image = camera.Capture();
				var average_value = Process.Average(image, 0);

				if(average_value < min)
				{
					min = average_value;
				}

				if(average_value > max)
				{
					max = average_value;
				}

				this.ImageSize = new Size(image.Width, image.Height);
//				this.ImageSize.Height = image.Height;
				System.Threading.Thread.Sleep(66);
			}

			this.WhiteBoardBackSurfaceDistance = max;
			this.WhiteBoardFrontSurfaceDistance = min;

		}

		public Size GetBlockSize(Size size)
		{
			throw new NotImplementedException();
		}

		public IImage GetBackgroundDeleteImage(IImage image)
		{
			var depth_pixels = new short[image.Width * image.Height];
			var pixels = new byte[image.Width * image.Height * 3];

			for (int y = 0; y < image.Height; ++y )
			{
				for(int x = 0; x < image.Width; ++x)
				{
					var depth = image.GetElement(x, y, 0);
					if ((depth > this.WhiteBoardFrontSurfaceDistance) &&
						(depth < this.WhiteBoardBackSurfaceDistance))
					{
						depth_pixels[y * image.Width + x] = (short)depth;
						pixels[(y * image.Width + x) * 3 + 0] = (byte)image.GetElement(x, y, 1);
						pixels[(y * image.Width + x) * 3 + 1] = (byte)image.GetElement(x, y, 2);
						pixels[(y * image.Width + x) * 3 + 2] = (byte)image.GetElement(x, y, 3);
					}
					else
					{
						depth_pixels[y * image.Width + x] = 0;
						pixels[(y * image.Width + x) * 3 + 0] = 0;
						pixels[(y * image.Width + x) * 3 + 1] = 0;
						pixels[(y * image.Width + x) * 3 + 2] = 0;
					}
				}
			}

			return new KinectImage(depth_pixels, pixels, image.Width, image.Height);
		}

		private Size ImageSize;

		private double WhiteBoardFrontSurfaceDistance { set; get; }
		private double WhiteBoardBackSurfaceDistance { set; get; }

		private int Iteration { set; get; }
	}
}
