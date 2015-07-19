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

			var selected_region = new System.Drawing.Rectangle();

			window.Closed += (object sender, EventArgs e) =>
				{
					selected_region = window.Area;
				};

			window.ShowDialog();
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
