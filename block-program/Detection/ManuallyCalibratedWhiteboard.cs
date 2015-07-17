using System;
using System.Threading.Tasks;
using Myxini.Recognition.Image;
using Myxini.Recognition.Raw;

namespace Myxini.Recognition
{
	public class ManuallyCalibratedWhiteboard : IBoard
	{
		private  Raw.Rectangle SelectedRegion;

		public ManuallyCalibratedWhiteboard()
		{

		}

		public void Calibration(ICamera camera)
		{
			var window = new Myxini.Recognition.UI.SelectRectangleWindow(camera);
		
			window.ShowDialog();
			var selected_region = window.Area;

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
