using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Myxini.Recognition;
using Myxini.Recognition.Image;

namespace RecognitionTest
{
	[TestClass]
	public class ImageTest
	{
		private static System.Drawing.Bitmap Color;
		private static System.Drawing.Bitmap Gray;
		private static byte[] ColorPixels;
		private static byte[] GrayPixels;


		[ClassInitialize]
		public static void IntializeThisTest()
		{
			Color = new System.Drawing.Bitmap("test_color_img.bmp");
			Gray = new System.Drawing.Bitmap("test_gray_img.bmp");

			ColorPixels = new byte[Color.Width * Color.Height * 3];
			GrayPixels = new byte[Gray.Width * Gray.Height];

			for (int y = 0; y < Color.Width; ++y)
			{
				for (int x = 0; x < Color.Height; ++x)
				{
					var color = Color.GetPixel(x, y);
					ColorPixels[y * Color.Width + x * 3 + 0] = color.B;
					ColorPixels[y * Color.Width + x * 3 + 1] = color.G;
					ColorPixels[y * Color.Width + x * 3 + 2] = color.R;
				}
			}

			for (int y = 0; y < Gray.Width; ++y)
			{
				for (int x = 0; x < Gray.Height; ++x)
				{
					GrayPixels[y * Gray.Width + x] = Gray.GetPixel(x, y).R;
				}
			}
		}

		[ClassCleanup]
		public static void CleanupThisTest()
		{
			Color.Dispose();
			Gray.Dispose();
		}

		[TestMethod]
		public void ConstructAllocateMemoryOnlyTest()
		{
			const int image_width = 640, image_height = 480;
			var color = new ColorImage(image_width, image_height);

			Assert.AreEqual(image_width, color.Width);
			Assert.AreEqual(image_height, color.Height);
			Assert.AreEqual(color.Channel, 3);
			Assert.AreEqual(color.IsRegionOfImage, false);

			color.GetElement(0, 0, 0);
			color.GetElement(0, 0, 1);
			color.GetElement(0, 0, 2);
		}

		[TestMethod]
		public void ConstructFromPixelTest()
		{
			var color = new ColorImage(ColorPixels, Color.Width, Color.Height);

			Assert.AreEqual(Color.Width, color.Width);
			Assert.AreEqual(Color.Height, color.Height);
			Assert.AreEqual(color.Channel, 3);
			Assert.AreEqual(color.IsRegionOfImage, false);

			for (int y = 0; y < color.Width; ++y)
			{
				for (int x = 0; x < color.Height; ++x)
				{
					var true_value = Color.GetPixel(x, y);
					Assert.AreEqual(color.GetElement(x, y, 0), true_value.B);
					Assert.AreEqual(color.GetElement(x, y, 1), true_value.G);
					Assert.AreEqual(color.GetElement(x, y, 2), true_value.R);
				}
			}
		}


		[TestMethod]
		public void ConstrucrROITest()
		{
			var color = new ColorImage(ColorPixels, Color.Width, Color.Height);

			var roi_left_top = new Myxini.Recognition.Raw.Point(20, 20);
			var roi_size = new Myxini.Recognition.Raw.Size(40, 40);

			var roi = new ColorImage(color, new Myxini.Recognition.Raw.Rectangle(roi_left_top, roi_size));

			Assert.AreEqual(roi.Width, Color.Width);
			Assert.AreEqual(roi.Height, Color.Height);
			Assert.AreEqual(roi.Channel, color.Channel);
			Assert.AreEqual(roi.IsRegionOfImage, true);
			Assert.AreEqual(roi.BoundingBox.X, roi_left_top.X);
			Assert.AreEqual(roi.BoundingBox.Y, roi_left_top.Y);
			Assert.AreEqual(roi.BoundingBox.Width, roi_size.Width);
			Assert.AreEqual(roi.BoundingBox.Width, roi_size.Height);

			for (int y = 0; y < roi.Width; ++y)
			{
				for (int x = 0; x < roi.Height; ++x)
				{
					Assert.AreEqual(roi.GetElement(x, y, 0), color.GetElement(x + roi_left_top.X, y + roi_left_top.Y, 0));
					Assert.AreEqual(roi.GetElement(x, y, 1), color.GetElement(x + roi_left_top.X, y + roi_left_top.Y, 1));
					Assert.AreEqual(roi.GetElement(x, y, 2), color.GetElement(x + roi_left_top.X, y + roi_left_top.Y, 2));
				}
			}
		}

	}
}
