using System;
using System.Threading.Tasks;
using Myxini.Recognition.Image;
using Myxini.Recognition.Raw;

namespace Myxini.Recognition
{
	public class ManuallyCalibratedWhiteboard : IBoard
	{
		private  Raw.Rectangle SelectedRegion;

		ManuallyCalibratedWhiteboard()
		{

		}

		public void Calibration(ICamera camera)
		{
			var window = new UI.MainWindow();
			var capture_task = Task.Run(() =>
			{
				var image = camera.Capture();

				byte[] pixel = new byte[image.Width * image.Height * 4];
				for (int y = 0; y < image.Height; ++y)
				{
					for (int x = 0; x < image.Width; ++x)
					{
						pixel[(y * image.Width + x) * 4 + 0] = (byte)image.GetElement(x, y, 0);
						pixel[(y * image.Width + x) * 4 + 1] = (byte)image.GetElement(x, y, 1);
						pixel[(y * image.Width + x) * 4 + 2] = (byte)image.GetElement(x, y, 2);
					}
				}

				var writable_image = new System.Windows.Media.Imaging.WriteableBitmap(image.Width, image.Height, 96, 96,
					System.Windows.Media.PixelFormats.Bgr32, null);

				window.CameraImageSource = writable_image;
			});
		
			window.ShowDialog();
			var selected_region = window.Area;

			capture_task.Dispose();
			this.SelectedRegion = new Rectangle(selected_region.X, selected_region.Y, selected_region.Width, selected_region.Height);
		}

		public Raw.Size GetBlockSize(Raw.Size size)
		{
			throw new NotImplementedException();
		}

		public IImage GetBackgroundDeleteImage(IImage image)
		{
			return image.RegionOfImage(this.SelectedRegion);
		}

	}
}
