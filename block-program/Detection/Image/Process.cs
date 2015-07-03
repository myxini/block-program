using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myxini.Recognition.Image
{
	public static class Process
	{
		int Dilate(IImage image, int x, int y, int c)
		{
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
	}
}
