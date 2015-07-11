using System;
using Myxini.Recognition.Image;


namespace RecognitionTest
{
	public class ImageLoader
	{
		public static IImage LoadImage(string filename)
		{
			var bitmap = new System.Drawing.Bitmap(filename);

			var pixels = new byte[bitmap.Width * bitmap.Height * 3];
			for (int y = 0; y < bitmap.Height; ++y)
			{
				for (int x = 0; x < bitmap.Width; ++x)
				{
					var color = bitmap.GetPixel(x, y);
					pixels[y * bitmap.Width + x + 0] = color.B;
					pixels[y * bitmap.Width + x + 1] = color.G;
					pixels[y * bitmap.Width + x + 2] = color.R;
				}
			}

			var image = new ColorImage(pixels, bitmap.Width, bitmap.Height);
			return image;
		}
	}
}
