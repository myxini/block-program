using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Myxini.Recognition.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SelectRectangleWindow : Window
    {
		public SelectRectangleWindow(Myxini.Recognition.Image.ICamera camera)
        {
            InitializeComponent();
			this.Camera = camera;
			
			var timer = new System.Windows.Threading.DispatcherTimer { Interval = TimeSpan.FromMilliseconds(33), IsEnabled = true };
			timer.Tick += UpdateDisplay;
        }

		private Myxini.Recognition.Image.ICamera Camera;

        private Point position_down;

        private bool is_down = false;

        public System.Drawing.Rectangle Area { get; private set; }

		public ImageSource CameraImageSource { set { this.CameraImage.Source = value; } }

        private void ContentPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(ContentPanel);
            position_down = position;
            is_down = true;
            Rectangle1.Margin = new Thickness(position.X, position.Y, 0, 0);
            Rectangle1.Width = 0;
            Rectangle1.Height = 0;
            Rectangle1.Visibility = System.Windows.Visibility.Visible;
        }

        private void ContentPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (!is_down)
            {
                return;
            }
            Point position = e.GetPosition(ContentPanel);
            Rectangle1.Width = Math.Abs(position.X - position_down.X);
            Rectangle1.Height = Math.Abs(position.Y - position_down.Y);
            Rectangle1.Margin = new Thickness(
                Math.Min(position.X, position_down.X),
                Math.Min(position.Y, position_down.Y),
                0,
                0
            );
        }

        private void ContentPanel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            is_down = false;

			var screen_coordinate = this.PointToScreen(new Point(Rectangle1.Margin.Left, Rectangle1.Margin.Top));
			var image_coordinate = this.CameraImage.PointFromScreen(screen_coordinate);
			
			Area = new System.Drawing.Rectangle(
				(int) (image_coordinate.X),
				(int)(image_coordinate.Y),
                (int) Rectangle1.Width,
                (int) Rectangle1.Height
            );

        }

		private void UpdateDisplay(object sender, EventArgs e)
		{
			var image = this.Camera.Capture();

			byte[] pixel = new byte[image.Width * image.Height * 4];
			for (int y = 0; y < image.Height; ++y)
			{
				for (int x = 0; x < image.Width; ++x)
				{
					pixel[(y * image.Width + x) * 4 + 0] = (byte)image.GetElement(x, y, 1);
					pixel[(y * image.Width + x) * 4 + 1] = (byte)image.GetElement(x, y, 2);
					pixel[(y * image.Width + x) * 4 + 2] = (byte)image.GetElement(x, y, 3);
				}
			}

			var writable_image = new System.Windows.Media.Imaging.WriteableBitmap(image.Width, image.Height, 96, 96,
				System.Windows.Media.PixelFormats.Bgr32, null);
			writable_image.WritePixels(new Int32Rect(0, 0, image.Width, image.Height), pixel, sizeof(int) * image.Width, 0);

			var screen_coordinate = this.PointToScreen(new Point(image.Width, image.Height));
			var image_coordinate = this.CameraImage.PointFromScreen(screen_coordinate);

			this.CameraImage.Source = writable_image;
			this.Width = screen_coordinate.X;
			this.Height = screen_coordinate.Y;
			this.CameraImage.Width = image.Width;
			this.CameraImage.Height = image.Height;
		}
    }
}
