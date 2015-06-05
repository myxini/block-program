using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myxini.Detection.Raw
{
	struct BoundingBox
	{
		BoundingBox(int x, int y, int width, int height) : this(new Point(x,y), new Size(width, height))
		{
		}

		BoundingBox(Point position, Size bounding_size)
		{
			this.Position = position;
			this.BoundingSize = bounding_size;
		}

		Point Position { get; private set; }
		Size BoundingSize { get; private set; }
	}
}
