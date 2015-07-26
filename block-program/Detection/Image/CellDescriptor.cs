using Myxini.Recognition.Raw;
using System;
using System.Collections.Generic;

namespace Myxini.Recognition.Image
{
	public class CellDescriptor
	{
		const int CELL_SIZE = 8;

		private class Gradient
		{
			public static readonly int BIN = 9;
			public static readonly double ORIENTATION = Math.PI / BIN;

			public Gradient()
			{
				this.Histogram = new float[BIN];
			}
			
			public float this[int index]
			{
				get { return this.Histogram[index]; }
				set { this.Histogram[index] = value; }
			}

			public float this[float n]
			{
				get { return this.Histogram[(int)Math.Floor(n / (Math.PI / ORIENTATION))]; }
				set { this.Histogram[(int)Math.Floor(n / (Math.PI / ORIENTATION))] = value; }
			}

			private float[] Histogram;
		}
		
		private static Gradient calculateGradient(GrayImage image)
		{
			var gradient = new Gradient();

			var orientation = Math.PI / Gradient.BIN;
			for (int y = 1; y < (image.Height - 1); ++y)
			{
				for (int x = 1; x < (image.Width - 1); ++x)
				{
					var fx = image.GetElement(x + 1, y) - image.GetElement(x - 1, y);
					var fy = image.GetElement(x, y + 1) - image.GetElement(x, y - 1);

					float magnitude = (float)Math.Sqrt(fx * fx + fy * fy);
					float angle = (float)Math.Atan2(fy, fx);

					if (angle < 0)
					{
						angle += (float)Math.PI;
					}
					else if (angle == Math.PI)
					{
						angle = 0.0f;
					}

					gradient[angle] += magnitude;
				}
			}

			return gradient;
		}

		private static Gradient[] DescriptHOG(GrayImage image)
		{
			var size = image.BoundingBox.BoundingSize;

			Size low_resolution_size = new Size(size.Width / CELL_SIZE, size.Height / CELL_SIZE);
			Func<Point, Point> remap_function = (Point input)=>
			{
				return new Point(
					input.X * CELL_SIZE,
					input.Y * CELL_SIZE
					);
			};

			Rectangle mapped_rect = new Rectangle(0, 0, CELL_SIZE, CELL_SIZE);

			Gradient[] output = new Gradient[low_resolution_size.Width * low_resolution_size.Height];
			
			for (int y = 0; y < low_resolution_size.Height; ++y)
			{
				for (int x = 0; x < low_resolution_size.Width; ++x)
				{
					Point point = new Point(x, y);
					Point mapped_point = remap_function(point);
					mapped_rect.X = mapped_point.X;
					mapped_rect.Y = mapped_point.Y;


					output[y * low_resolution_size.Width + x] = calculateGradient(image.RegionOfImage(mapped_rect) as GrayImage);
				}
			}

			return output;
		}

		public static GrayImage DescriptImage(GrayImage image)
		{
			var size = image.BoundingBox.BoundingSize;
			var hog = DescriptHOG(image);

			// normalize histogram
			float max_grad = float.MinValue;
			foreach (var gradient in hog)
			{
				for (int c = 0; c < Gradient.BIN; ++c)
				{
					if (max_grad < gradient[c])
					{
						max_grad = gradient[c];
					}
				}
			}
			
			
			foreach (var gradient in hog)
			{
				for (int c = 0; c < Gradient.BIN; ++c)
				{
						gradient[c] = gradient[c] / max_grad;
				}
			}

			Size low_resolution_size = new Size(size.Width / CELL_SIZE, size.Height / CELL_SIZE);

			var gray_bitmap = new byte[low_resolution_size.Width * low_resolution_size.Height];

			for (int y = 0; y < low_resolution_size.Height; ++y )
			{
				for(int x = 0; x < low_resolution_size.Width; ++x )
				{
					float sum = 0.0f;
					var index = y * low_resolution_size.Width + x;
					for (int c = 0; c < Gradient.BIN; ++c)
					{
						sum += hog[index][c];
					}

					if (sum > 0.2)
					{
						gray_bitmap[index] = 255;
					}
					else
					{
						gray_bitmap[index] = 0;
					}
				}
			}

			return new GrayImage(gray_bitmap, low_resolution_size.Width, low_resolution_size.Height);
		}

		public static int mapPoint(int p)
		{
			return p / CELL_SIZE;
		}
	}
}
