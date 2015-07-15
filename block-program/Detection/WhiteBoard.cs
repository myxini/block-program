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
			var color_pixels = new byte[image.Width * image.Height * 3];
			
			var min_max = Image.Process.FindMinMax(image);
			
			double avg_distance = 0.0;
			int total_pixel = image.Width * image.Height;
			for(int y = 0; y < image.Height; ++y)
			{
				for(int x = 0;x < image.Width; ++x)
				{
					avg_distance += (double)(image.GetElement(x, y, 0)) / total_pixel;
				}
			}

			for(int y = 0; y < image.Height; ++y)
			{
				for(int x = 0;x < image.Width; ++x)
				{
					if((image.GetElement(x, y, 0) - 50) > avg_distance)
					{
						depth_pixels[y * image.Width + x] = 0;
						for(int c = 1; c < image.Channel; ++c)
						{
							color_pixels[(y * image.Width + x) * 3 + (c - 1)] = 0;
						}
					}
					else
					{
						for(int c = 1; c < image.Channel; ++c)
						{
							color_pixels[(y * image.Width + x) * 3 + (c - 1)] = (byte)image.GetElement(x, y, c);
						}
					}
				}
			}
			
			return new KinectImage(depth_pixels, color_pixels, image.Width, image.Height);
		}

		private Size ImageSize;

		private double WhiteBoardFrontSurfaceDistance { set; get; }
		private double WhiteBoardBackSurfaceDistance { set; get; }

		private int Iteration { set; get; }
	}
}
