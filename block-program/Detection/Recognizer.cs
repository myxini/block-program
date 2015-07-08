using System;
using System.Collections.Generic;
using Myxini.Recognition.Raw;
using Myxini.Recognition.Image;

namespace Myxini.Recognition
{
	public class Recognizer
	{
		struct OuterRectangle
		{
			public int Top = int.MaxValue;
			public int Bottom = int.MinValue;
			public int Left = int.MaxValue;
			public int Right = int.MinValue;
		}

		public Script Recognition(IImage kinect_image)
		{
			

			throw new NotImplementedException();
		}

		private List<Rectangle> FindBlockRectangle(IImage depth)
		{
			Dictionary<int, OuterRectangle> result;

			var label = Process.Labeling(depth);

			for (int y = 0; y < depth.Height; ++y)
			{
				for (int x = 0; x < depth.Height; ++x)
				{
					var l = label[y * depth.Width + x];

					if (l == 0)
					{
						continue;
					}

					if(!result.ContainsKey(l))
					{
						result.Add(l, new OuterRectangle());
					}

					var outer_rectangle = result[l];

					if (outer_rectangle.Left > x)
					{
						outer_rectangle.Left = x;
					}

					if (outer_rectangle.Right < x)
					{
						outer_rectangle.Right = x;
					}

					if (outer_rectangle.Top > y)
					{
						outer_rectangle.Top = y;
					}

					if (outer_rectangle.Bottom < y)
					{
						outer_rectangle.Bottom = y;
					}
				}
			}

			rect.reserve(result.size());
			for (auto&& out : result)
			{
				cv::Rect region(cv::Point(out.second.Left, out.second.Top), cv::Point(out.second.Right, out.second.Bottom));

				if (region.area() < 20)
				{
					continue;
				}

				rect.push_back(region);
			}

			throw new NotImplementedException();
		}
	}
}
