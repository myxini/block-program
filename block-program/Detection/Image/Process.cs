using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myxini.Recognition.Image
{
	public static partial class Process
	{
		public static List<double> Average(IImage image)
		{
			var result = new List<double>(image.Channel);

			for (int c = 0; c < image.Channel; ++c)
			{
				result[c] = Average(image, c);
			}

			return result;
		}
		public static double Average(IImage image, int channel)
		{
			double result = 0.0;

			for (int y = 0; y < image.Height; ++y)
			{
				for (int x = 0; x < image.Width; ++x)
				{
					result += image.GetElement(x, y, channel);
				}
			}

			return result / (image.Height * image.Width);
		}

		/// <summary>
		/// 膨張処理
		/// 基本的にコンストラクタにつっこんでください
		/// </summary>
		public static int Dilate(IImage image, int x, int y, int c)
		{
			if(x == 0 || y == 0 || x >= image.Width || y >= image.Height)
			{
				return DilateSlow(image, x, y, c);
			}

			return
				image.GetElement(x - 1, y - 1, c) != 0 ||
				image.GetElement(x, y - 1, c) != 0 ||
				image.GetElement(x + 1, y - 1, c) != 0 ||
				image.GetElement(x - 1, y, c) != 0 ||
				image.GetElement(x, y, c) != 0 ||
				image.GetElement(x + 1, y, c) != 0 ||
				image.GetElement(x - 1, y + 1, c) != 0 ||
				image.GetElement(x, y + 1, c) != 0 ||
				image.GetElement(x + 1, y + 1, c) != 0 ? int.MaxValue : 0;
		}

		/// <summary>
		/// 膨張処理
		/// 基本的にコンストラクタにつっこんでください
		/// </summary>
		public static int Erode(IImage image, int x, int y, int c)
		{
			if (x == 0 || y == 0 || x >= image.Width || y >= image.Height)
			{
				return DilateSlow(image, x, y, c);
			}

			return
				image.GetElement(x - 1, y - 1, c) == 0 ||
				image.GetElement(x, y - 1, c) == 0 ||
				image.GetElement(x + 1, y - 1, c) == 0 ||
				image.GetElement(x - 1, y, c) == 0 ||
				image.GetElement(x, y, c) == 0 ||
				image.GetElement(x + 1, y, c) == 0 ||
				image.GetElement(x - 1, y + 1, c) == 0 ||
				image.GetElement(x, y + 1, c) == 0 ||
				image.GetElement(x + 1, y + 1, c) == 0 ? 0 : int.MaxValue;
		}

		private static int DilateSlow(IImage image, int x, int y, int c)
		{
			for(int py = y - 1; py < (y + 1); ++py)
			{
				for (int px = x - 1; px < (x + 1); ++px)
				{
					try
					{
						if (image.GetElement(px, py, c) != 0)
						{
							return int.MaxValue;
						}
					}
					catch(Exception )
					{

					}
				}
			}

			return 0;
		}
		private static int ErodeSlow(IImage image, int x, int y, int c)
		{
			for (int py = y - 1; py < (y + 1); ++py)
			{
				for (int px = x - 1; px < (x + 1); ++px)
				{
					try
					{
						if (image.GetElement(px, py, c) == 0)
						{
							return 0;
						}
					}
					catch (Exception)
					{

					}
				}
			}

			return int.MaxValue;
		}
	}
}
