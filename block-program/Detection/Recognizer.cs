using System;
using System.Collections.Generic;
using Myxini.Recognition.Raw;
using Myxini.Recognition.Image;

namespace Myxini.Recognition
{
	public class Recognizer
	{
		private Size MaskSize = new Size(30, 30);

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

			var debug_file = new System.IO.StreamWriter("output.txt", false);

			foreach(var rect in rectangles)
			{
				debug_file.Write(rect.X);
				debug_file.Write(",");
				debug_file.Write(rect.Y);
				debug_file.Write(",");
				debug_file.Write(rect.Width);
				debug_file.Write(",");
				debug_file.Write(rect.Height);
//				debug_file.Write(",");
				debug_file.WriteLine();
			}
			debug_file.Flush();

			var image = new GrayImage(kinect_image, GrayImage.ImageType.ABGR);
			var cell_image = CellDescriptor.DescriptImage(image);

			cell_image = new GrayImage(cell_image, Process.Dilate);
			cell_image = new GrayImage(cell_image, Process.Dilate);
			cell_image = new GrayImage(cell_image, Process.Dilate);

			var labels = Process.Labeling(cell_image);


			List<Tuple<IBlock, Rectangle>> control_block = new List<Tuple<IBlock, Rectangle>>();
			List<Tuple<IBlock, Rectangle>> other_block = new List<Tuple<IBlock, Rectangle>>();

			var classifier = new Classifier();
			var algorithm = new SADAlgorithm();
			foreach (var rectangle in rectangles)
			{
				var target = kinect_image.RegionOfImage(rectangle);
				var result = classifier.Clustering(target, algorithm);

				if (result.IsControlBlock)
				{
					control_block.Add(new Tuple<IBlock, Rectangle>(result, rectangle));
				}
				else
				{
					other_block.Add(new Tuple<IBlock, Rectangle>(result, rectangle));
				}
			}

			Script result_script = new Script();

			foreach (var trigger in control_block)
			{
				result_script.Add(trigger.Item1 as ControlBlock);

				foreach (var other in other_block)
				{
					if (this.IsConnectedBlock(labels, cell_image.BoundingBox.BoundingSize, trigger.Item2, other.Item2))
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


		private List<Rectangle> FindBlockRectangle(IImage kinect_image)
		{
			var img = new GrayImage(kinect_image, GrayImage.ImageType.ABGR);

			var mask_size = this.MaskSize;

			DebugOutput.SaveGrayImage("gray_img.png", img);
			var output = new GrayImage(img, 
				(IImage input, int x, int y, int c)=>
					{
						if(x == 0 || y == 0 || x == (input.Width - 1) || y == (input.Height - 1))
						{
							return 0;
						}

						var fx = input.GetElement(x + 1, y, c) - input.GetElement(x, y, c);
						var fy = input.GetElement(x, y + 1, c) - input.GetElement(x, y - 1, c);

						return (int)Math.Sqrt(fx * fx + fy * fy);
					}
				);

			DebugOutput.SaveGrayImage("output.png", output);
			// 出力用の画像の正規化
			var minmax = Process.FindMinMax(output);
			
			var normalized_output = new GrayImage(
				output, (IImage input, int x, int y, int c)=>
					{
						var value = ((double)(output.GetElement(x, y, c) - minmax.Item1[c])) / (minmax.Item2[c] - minmax.Item1[c]) ;

						if(value < 0.2)
						{
							return 0;
						}

						return 0xff;
					}
				);

			DebugOutput.SaveGrayImage("normalized_image.png", normalized_output);
			//ごま塩ノイズの削除
			var noise_deleted_image = new GrayImage(
				normalized_output,
				(IImage input, int x, int y, int c)=>
					{
						if(x == 0 || y == 0 || x == (input.Width - 1) || y == (input.Height - 1))
						{
							return 0;
						}

						if(input.GetElement(x, y, c) != 0)
						{
							if(
								input.GetElement(x - 1, y - 1, c) != 0 ||
								input.GetElement(x - 1, y - 0, c) != 0 ||
								input.GetElement(x - 1, y + 1, c) != 0 ||
								input.GetElement(x - 0, y - 1, c) != 0 ||
//								input.GetElement(x - 0, y - 0, c) != 0 ||
								input.GetElement(x - 0, y + 1, c) != 0 ||
								input.GetElement(x + 1, y - 1, c) != 0 ||
								input.GetElement(x + 1, y - 0, c) != 0 ||
								input.GetElement(x + 1, y + 1, c) != 0)
							{
								return 0xff;
							}
							else
							{
								return 0x00;
							}
						}

						return 0x00;
					}
				);

			DebugOutput.SaveGrayImage("noise_deleted_image.png", noise_deleted_image);
			var candidate_area_map = new float[noise_deleted_image.BoundingBox.BoundingSize.Area];
			
			for(int y = 0; y < (noise_deleted_image.Height - this.MaskSize.Height); ++y)
			{
				for(int x = 0; x < (noise_deleted_image.Width - this.MaskSize.Width); ++x)
				{
					var find_rect = new Rectangle(new Point(x,y), this.MaskSize);
					var score = ((double)Process.CountNoneZero(noise_deleted_image.RegionOfImage(find_rect))) / this.MaskSize.Area;

					if(score < 0.13)
					{
						score = 0.0f;
					}

					candidate_area_map[y * noise_deleted_image.Width + x] = (float)score;
				}
			}

			var candidate_pixels = new byte[noise_deleted_image.BoundingBox.BoundingSize.Area];
			for(int y = 0; y < noise_deleted_image.Height; ++y)
			{
				for(int x = 0;x < noise_deleted_image.Width; ++x)
				{
					candidate_pixels[y * noise_deleted_image.Width + x] = (byte)(candidate_area_map[y * noise_deleted_image.Width + x] * (float)byte.MaxValue);
				}
			}
			var candidate_img = new GrayImage(candidate_pixels, noise_deleted_image.Width, noise_deleted_image.Height);
			DebugOutput.SaveGrayImage("candidate_img.png", candidate_img);

			//candidate_area_map = medianFilter(candidate_area_map, noise_deleted_image.BoundingBox.BoundingSize);
			//candidate_area_map = medianFilter(candidate_area_map, noise_deleted_image.BoundingBox.BoundingSize);
			
			for (int y = 0; y < noise_deleted_image.Height; ++y)
			{
				for (int x = 0; x < noise_deleted_image.Width; ++x)
				{
					candidate_pixels[y * noise_deleted_image.Width + x] = (byte)(candidate_area_map[y * noise_deleted_image.Width + x] * (float)byte.MaxValue);
				}
			}
			candidate_img = new GrayImage(candidate_pixels, noise_deleted_image.Width, noise_deleted_image.Height);
			DebugOutput.SaveGrayImage("candidate_img_median.png", candidate_img);

			var found_points = GetMaximalRects(candidate_area_map, this.MaskSize);

			var result = new List<Rectangle>();
			foreach(var p in found_points)
			{
				result.Add(new Rectangle(p, this.MaskSize));
			}

			return result;
		}

		private List<Point> GetMaximalRects(float[] candidate_map, Size mask_size)
		{
			var maximal = new List<Point>();

			for(int y = 1; y < (mask_size.Height - 1); ++y)
			{
				for(int x = 1; x < (mask_size.Width - 1); ++x)
				{
					var fy1 = candidate_map[y * mask_size.Width + x] - candidate_map[(y - 1) * mask_size.Width + x];
					var fy2 = candidate_map[(y + 1) * mask_size.Width + x] - candidate_map[y * mask_size.Width + x];
					var fx1 = candidate_map[y * mask_size.Width + x] - candidate_map[y * mask_size.Width + x - 1];
					var fx2 = candidate_map[y * mask_size.Width + x + 1] - candidate_map[y * mask_size.Width + x];
					
					if (fy1 >= 0 && fy2 < 0 && fx1 >= 0 && fx2 < 0) // 平坦な極地を検出するために>=
					{
						maximal.Add(new Point(x, y));
					}

				}
			}

			return maximal;
		}

		private float[] medianFilter(float[] pixels, Size size)
		{
			float[] output = new float[size.Area];
			for (int y = 1; y < (size.Height - 1); ++y)
			{
				for (int x = 1; x < (size.Width - 1); ++x)
				{
					float sum = 0.0f;
					for (int i = -1; i < 1; ++i )
					{
						for (int j = -1; j < 1; ++j )
						{
							sum += pixels[(y + i) * size.Width + x + j];
						}
					}

					output[y * size.Width + x] = sum / 9;
					///
				}
			}

			return output;
		}

	}
}
