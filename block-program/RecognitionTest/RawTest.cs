using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Myxini.Recognition.Raw;

namespace RecognitionTest
{
	[TestClass]
	public class RawTest
	{
		[TestMethod]
		public void TestPoint()
		{
			const int point_x = 20, point_y = 10;

			var point = new Point(point_x, point_y);

			Assert.Equals(point.X, point_x);
			Assert.Equals(point.Y, point_y);
		}

		[TestMethod]
		public void TestSize()
		{
			const int width = 20, height = 10;

			var size = new Size(width, height);

			Assert.Equals(size.Width, width);
			Assert.Equals(size.Height, height);

			Assert.Equals(size.Area, width * height);
		}

	}
}
