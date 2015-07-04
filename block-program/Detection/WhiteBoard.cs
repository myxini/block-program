using System;
using Myxini.Recognition.Image;
using Myxini.Recognition.Raw;

namespace Myxini.Recognition
{
	public class WhiteBoard : IBoard
	{
		public void Calibration(ICamera camera)
		{
			var image = camera.Capture();

		}

		public Size GetBlockSize(Size size);
		public IImage GetBackgroundDeleteImage(IImage image);
	}
}
