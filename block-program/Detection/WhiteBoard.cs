using System;
using Myxini.Recognition.Image;
using Myxini.Recognition.Raw;

namespace Myxini.Recognition
{
	public class WhiteBoard : IBoard
	{
		public WhiteBoard(int iteration)
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
			throw new NotImplementedException();
		}

		private Size ImageSize;

		private double WhiteBoardFrontSurfaceDistance { set; get; }
		private double WhiteBoardBackSurfaceDistance { set; get; }

		private int Iteration { set; get; }
	}
}
