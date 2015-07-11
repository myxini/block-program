using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Myxini.Recognition.Raw;


namespace RecognitionTest
{
	[TestClass]
	public class RawTest
	{
		[TestMethod]
		public void TestMethod1()
		{
		}

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

		[TestMethod]
		public void TestRectangle()
		{
			const int left = 20, top = 10, width = 300, height = 200;

			var rect = new Rectangle(new Point(left, top), new Size(width, height));

			var rect2 = new Rectangle(new Point(left, top), new Point(left + width, top + height));

			Assert.Equals(rect.Position.X, rect2.Position.X);
			Assert.Equals(rect.Position.Y, rect2.Position.Y);
			Assert.Equals(rect.BoundingSize.Width, rect2.BoundingSize.Width);
			Assert.Equals(rect.BoundingSize.Height, rect2.BoundingSize.Height);

			var rect3 = new Rectangle(left, top, width, height);
			Assert.Equals(rect.Position.X, rect3.Position.X);
			Assert.Equals(rect.Position.Y, rect3.Position.Y);
			Assert.Equals(rect.BoundingSize.Width, rect3.BoundingSize.Width);
			Assert.Equals(rect.BoundingSize.Height, rect3.BoundingSize.Height);
		}
	}
}
