using System;
using System.Collections.Generic;
using Myxini.Recognition.Raw;
using Myxini.Recognition.Image;

namespace Myxini.Recognition
{
	public class Recognizer
	{
		class OuterRectangle
		{
			public int Top = int.MaxValue;
			public int Bottom = int.MinValue;
			public int Left = int.MaxValue;
			public int Right = int.MinValue;
		}

		public Script Recognition(IImage kinect_image)
		{
			var rectangles = this.FindBlockRectangle(kinect_image);

			var image = new GrayImage(kinect_image, GrayImage.ImageType.ARGB);
			var cell_image = CellDescriptor.DescriptImage(image);

			cell_image = new GrayImage(cell_image, Process.Dilate);
			cell_image = new GrayImage(cell_image, Process.Dilate);
			cell_image = new GrayImage(cell_image, Process.Dilate);

			var labels = Process.Labeling(cell_image);


			List<Tuple<IBlock, Rectangle>> control_block = new List<Tuple<IBlock, Rectangle>>();
			List<Tuple<IBlock, Rectangle>> other_block = new List<Tuple<IBlock, Rectangle>>();

			var classifier = new Classifier();
            var algorithm = new SADAlgorithm();
			foreach(var rectangle in rectangles)
			{
				var target = kinect_image.RegionOfImage(rectangle);
				var result = classifier.Clustering(target, algorithm);

				if(result.IsControlBlock)
				{
					control_block.Add(new Tuple<IBlock, Rectangle>(result, rectangle));
				}
				else
				{
					other_block.Add(new Tuple<IBlock, Rectangle>(result, rectangle));
				}
			}

			Script result_script = new Script();
			
			foreach(var trigger in control_block)
			{
				result_script.Add(trigger.Item1 as ControlBlock);

				foreach(var other in other_block)
				{
					if(this.IsConnectedBlock(labels, cell_image.BoundingBox.BoundingSize, trigger.Item2, other.Item2))
					{
						result_script.Add(other.Item1 as InstructionBlock);
					}
				}
			}

			return result_script;
		}

		private bool IsConnectedBlock(int[] labels, Size label_image_size, Rectangle a, Rectangle b)
		{
			Point a_bottom_right = new Point(a.X + a.Width, a.Y + a.Height);
//			Point a_top_left = new Point(a.X, a.Y);
			Point b_bottom_right = new Point(b.X + b.Width, b.Y + b.Height);
//			Point b_top_left = new Point(b.X, b.Y);

			for (int ry = a.Y; ry < a_bottom_right.Y; ++ry)
			{
				for (int rx = a.X; rx < a_bottom_right.X; ++rx)
				{
					for (int by = b.Y; by < b_bottom_right.Y; ++by)
					{
						for (int bx = b.X; bx < a_bottom_right.X; ++bx)
						{
							var r_label = labels[ry * label_image_size.Width + rx];
							var b_label = labels[by * label_image_size.Width + bx];

							if (r_label == 0 || b_label == 0)
							{
								continue;
							}

							if (r_label == b_label)
							{
								return true;
							}
						}
					}
				}
			}

			return false;
		}


		private List<Rectangle> FindBlockRectangle(IImage depth)
		{
			var rectangle_dictionary = new Dictionary<int, OuterRectangle>();

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

					if(!rectangle_dictionary.ContainsKey(l))
					{
						rectangle_dictionary.Add(l, new OuterRectangle());
					}

					var outer_rectangle = rectangle_dictionary[l];

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

			var result = new List<Rectangle>();
			foreach (var element in rectangle_dictionary)
			{
				Rectangle region = new Rectangle(
					new Point(element.Value.Left, element.Value.Top), 
					new Point(element.Value.Right, element.Value.Bottom));

				if (region.BoundingSize.Area < 20 && 
					region.BoundingSize.Area  > depth.BoundingBox.BoundingSize.Area * 0.4)
				{
					continue;
				}

				result.Add(region);
			}

			return result;
		}

	}
}
