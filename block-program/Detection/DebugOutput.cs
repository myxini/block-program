using System;
using System.Diagnostics;

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
			var output = new System.Drawing.Bitmap(image.Width, image.Height);

			for(int y = 0; y < image.Height; ++y)
			{
				for(int x = 0; x < image.Width; ++x)
				{
					var depth = (byte)((double)image.GetElement(x, y, offset_channel) / short.MaxValue * 255);
					output.SetPixel(x, y, System.Drawing.Color.FromArgb(depth, depth, depth));
				}
			}

			output.Save(file_name);
		}

		[Conditional("DEBUG")] 
		public static void SaveColorImage(string file_name, IImage image, int offset_channel = 0)
		{
			var output = new System.Drawing.Bitmap(image.Width, image.Height);

			for (int y = 0; y < image.Height; ++y)
			{
				for (int x = 0; x < image.Width; ++x)
				{
					var r = image.GetElement(x, y, offset_channel + 0);
					var g = image.GetElement(x, y, offset_channel + 1);
					var b = image.GetElement(x, y, offset_channel + 2);
					output.SetPixel(x, y, System.Drawing.Color.FromArgb(r, g, b));
				}
			}

			output.Save(file_name);
		}

	}
}
