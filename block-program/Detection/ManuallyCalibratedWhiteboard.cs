using System;
using Myxini.Recognition.Image;

namespace Myxini.Recognition
{
	public class ManuallyCalibratedWhiteboard : IBoard
	{
		public void Calibration(ICamera camera)
		{
			var image = camera.Capture();

			

			throw new NotImplementedException();
		}

		public Raw.Size GetBlockSize(Raw.Size size)
		{
			throw new NotImplementedException();
		}

		public IImage GetBackgroundDeleteImage(IImage image)
		{
			throw new NotImplementedException();
		}

	}
}
