using System;
using System.Collections.Generic;

namespace Myxini.Recognition.Image
{
	static partial class Process
	{
		private static bool IsIntersection(IImage image, int x, int y)
		{
			return 0 <= x && x < image.Width && 0 <= y && y < image.Height;
		}

		private static int GetElement(IImage image, int x, int y)
		{
			int value = 0;

			for(int c = 0; c < image.Channel; ++c)
			{
				value |= image.GetElement(x, y, c) << 8;
			}

			return value;
		}
		
		//aの属すグループの代表に向かって経路圧縮（代表を返す）
		private static int Compress(List<int> parents, int a)
		{
			while (a != parents[a])
			{
				parents[a] = parents[parents[a]];
				a = parents[a];
			}
			return a;
		}

		//aの属すグループとbの属すグループを併合（併合後の代表を返す）
		private static int Link(List<int> parents, int a, int b)
		{
			a = Compress(parents, a);
			b = Compress(parents, b);
			if (a < b)
			{
				parents[b] = a;
				return parents[b];
			}

			parents[a] = b;
			return parents[a];
		}
		
		private static int relabel(List<int> parents)
		{
			int index = 0;
			for (int i = 0; i < parents.Count; i++)
			{
				if (i == parents[i])
				{
					parents[i] = index++;
				}
				else
				{
					parents[i] = parents[parents[i]];
				}
			}

			return index;
		}

		public static int[] Labeling(IImage image)
		{
			var parents = new List<int>();
			var output = new int[image.Width * image.Height];
			

			int index = 0;

			for (int y = 0; y < image.Height; ++y)
			{
				for (int x = 0; x < image.Width; ++x)
				{
					int value = GetElement(image, x, y);

					bool in_left = (IsIntersection(image, x - 1, y) && value == GetElement(image, x - 1, y)); //左
					bool in_top = (IsIntersection(image, x, y - 1) && value == GetElement(image, x, y - 1)); //上
					bool in_left_top = (IsIntersection(image, x - 1, y - 1) && value == GetElement(image, x - 1, y - 1)); //左上
					bool in_right_top = (IsIntersection(image, x + 1, y - 1) && value == GetElement(image, x + 1, y - 1)); //右上

					output[y * image.Width + x] = index;
					if (in_left || in_top || in_left_top || in_right_top)
					{
						parents.Add(index);
						if (in_left) output[y * image.Width + x] = Link(parents, output[y * image.Width + x], output[y * image.Width + x - 1]);
						if (in_top) output[y * image.Width + x] = Link(parents, output[y * image.Width + x], output[(y - 1) * image.Width + x]);
						if (in_left_top) output[y * image.Width + x] = Link(parents, output[y * image.Width + x], output[(y - 1) * image.Width + x - 1]);
						if (in_right_top) output[y * image.Width + x] = Link(parents, output[y * image.Width + x], output[(y - 1) * image.Width + x + 1]);
						parents.RemoveAt(parents.Count - 1);
					}
					else
					{
						parents.Add(index++);
					}
				}
			}
			
			int regions = relabel(parents);
			for (int y = 0; y < image.Height; ++y)
			{
				for (int x = 0; x < image.Width; ++x)
				{
					output[y * image.Width + x] = parents[output[y * image.Width + x]];
				}
			}

			return output;
		}
	}
}
