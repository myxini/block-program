using System;
using System.Diagnostics;
using System.Windows;
using System.IO;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace Myxini.Recognition.Image
{
	public class DebugOutput
	{
		[Conditional("DEBUG")] 
		public static void SaveImage(string[] file_name, IImage image)
		{
			if(image.Channel == 1)
			{
				SaveDepthImage(file_name[0], image);
			}
			else if (image.Channel == 3)
			{
				SaveColorImage(file_name[0], image);
			}
			else if (image.Channel == 4)
			{
				SaveDepthImage(file_name[0], image, 0);
				SaveColorImage(file_name[1], image, 1);
			}
		}

		[Conditional("DEBUG")] 
		public static void SaveDepthImage(string file_name, IImage image, int offset_channel = 0)
		{
			var output = new WriteableBitmap(image.Width, image.Height, 96, 96,
				System.Windows.Media.PixelFormats.Gray16, null);

			var pixel = new short[image.Width * image.Height];

			for(int y = 0; y < image.Height; ++y)
			{
				for(int x = 0; x < image.Width; ++x)
				{
					pixel[y * image.Width + x] = (short)image.GetElement(x, y, offset_channel);
				}
			}

			output.WritePixels(new Int32Rect(0, 0, image.Width, image.Height), pixel, image.Width * sizeof(short), 0);

			using (FileStream stream = new FileStream(file_name, FileMode.Create, FileAccess.Write))
			{
				var encoder = new PngBitmapEncoder();
				encoder.Frames.Add(BitmapFrame.Create(output));
				encoder.Save(stream);
				stream.Flush();
			}
		}

		[Conditional("DEBUG")] 
		public static void SaveColorImage(string file_name, IImage image, int offset_channel = 0)
		{
			var output = new System.Drawing.Bitmap(image.Width, image.Height);

			for (int y = 0; y < image.Height; ++y)
			{
				for (int x = 0; x < image.Width; ++x)
				{
					var b = image.GetElement(x, y, offset_channel + 0);
					var g = image.GetElement(x, y, offset_channel + 1);
					var r = image.GetElement(x, y, offset_channel + 2);
					output.SetPixel(x, y, System.Drawing.Color.FromArgb(r, g, b));
				}
			}

			output.Save(file_name);
		}

	}
}
