using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myxini.Recognition.Image
{
	public static class Process<Image>
		where Image : IImage
	{
		/// <summary>
		/// 膨張処理
		/// 基本的にコンストラクタにつっこんでください
		/// </summary>
		public static int Dilate(Image image, int x, int y, int c)
		{
			if(x < 0 || y < 0 || x >= image.Width || y >= image.Height)
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
		public static int Erode(Image image, int x, int y, int c)
		{
			if (x < 0 || y < 0 || x >= image.Width || y >= image.Height)
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

		private static int DilateSlow(Image image, int x, int y, int c)
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
		private static int ErodeSlow(Image image, int x, int y, int c)
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
