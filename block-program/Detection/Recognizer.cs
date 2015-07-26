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
			ColorImage color= kinect_image as ColorImage;
			switch(kinect_image.Channel)
			{
				case 4:
					byte[] color_pixel = new byte[kinect_image.BoundingBox.BoundingSize.Area * 3];
					for (int y = 0; y < kinect_image.Height; ++y )
					{
						for(int x = 0; x < kinect_image.Width; ++x)
						{
							color_pixel[(y * kinect_image.Width + x) * 3 + 0] = (byte)kinect_image.GetElement(x, y, 1);
							color_pixel[(y * kinect_image.Width + x) * 3 + 1] = (byte)kinect_image.GetElement(x, y, 2);
							color_pixel[(y * kinect_image.Width + x) * 3 + 2] = (byte)kinect_image.GetElement(x, y, 3);
						}
					}
					color = new ColorImage(color_pixel, kinect_image.Width, kinect_image.Height);
					break;
			}

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

			List<Tuple<IBlock, Rectangle>> control_block = new List<Tuple<IBlock, Rectangle>>();
			List<Tuple<IBlock, Rectangle>> other_block = new List<Tuple<IBlock, Rectangle>>();

			//var classifier = new Classifier();
			var classifier = new SVMClassifier(@".\learning\");
			var algorithm = new SADClassifier();
			int iteration = 0;
			foreach (var rectangle in rectangles)
			{
				var target = color.RegionOfImage(rectangle);
				DebugOutput.SaveColorImage(iteration.ToString() + ".png", target);
				++iteration;
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

			/// ブロックの連結について調べる

			var image = new GrayImage(kinect_image, GrayImage.ImageType.ABGR);
			var cell_image = CellDescriptor.DescriptImage(image);
			
			cell_image = new GrayImage(cell_image, Process.Dilate);
			cell_image = new GrayImage(cell_image, Process.Dilate);
			cell_image = new GrayImage(cell_image, Process.Dilate);
			DebugOutput.SaveGrayImage("cell_image.png", cell_image);
			var labels = Process.Labeling(cell_image);
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
			var noise_deleted_image = DeleteSinglePixelNoise(normalized_output);

			DebugOutput.SaveGrayImage("noise_deleted_image.png", noise_deleted_image);

			var candidate_area_map = CreateScoreMap(noise_deleted_image);
			
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
			
			//for (int y = 0; y < noise_deleted_image.Height; ++y)
			//{
			//	for (int x = 0; x < noise_deleted_image.Width; ++x)
			//	{
			//		candidate_pixels[y * noise_deleted_image.Width + x] = (byte)(candidate_area_map[y * noise_deleted_image.Width + x] * (float)byte.MaxValue);
			//	}
			//}
			//candidate_img = new GrayImage(candidate_pixels, noise_deleted_image.Width, noise_deleted_image.Height);
			//DebugOutput.SaveGrayImage("candidate_img_median.png", candidate_img);

			var found_points = GetMaximalRects(candidate_area_map, noise_deleted_image.BoundingBox.BoundingSize);

			var candidate_rect = new List<Rectangle>();
			foreach(var p in found_points)
			{
				candidate_rect.Add(new Rectangle(p, this.MaskSize));
			}
			
			var color_pixels = new byte[kinect_image.BoundingBox.BoundingSize.Area * 3];
			for(int y = 0; y < kinect_image.Height; ++y)
			{
				for(int x = 0; x < kinect_image.Width; ++x)
				{
					for(int c = 0; c < 3; ++c)
					{
						color_pixels[(y * kinect_image.Width + x) * 3 + c] = (byte)kinect_image.GetElement(x, y, c + 1);
					}
				}
			}

			var color_img=new ColorImage(color_pixels, kinect_image.Width, kinect_image.Height);
			var output_rects = RestrictRectangle(candidate_rect, noise_deleted_image, color_img);

			output_rects = RemoveIntersectionRectangle(color_img, output_rects);
			output_rects = RemoveNearRectangle(output_rects);
			
			return output_rects;
		}

		private List<Point> GetMaximalRects(float[] candidate_map, Size size)
		{
			var maximal = new List<Point>();

			for(int y = 1; y < (size.Height - 1); ++y)
			{
				for(int x = 1; x < (size.Width - 1); ++x)
				{
					var fy1 = candidate_map[y * size.Width + x] - candidate_map[(y - 1) * size.Width + x];
					var fy2 = candidate_map[(y + 1) * size.Width + x] - candidate_map[y * size.Width + x];
					var fx1 = candidate_map[y * size.Width + x] - candidate_map[y * size.Width + x - 1];
					var fx2 = candidate_map[y * size.Width + x + 1] - candidate_map[y * size.Width + x];
					
					if (fy1 >= 0 && fy2 < 0 && fx1 >= 0 && fx2 < 0) // 平坦な極地を検出するために>=
					{
						maximal.Add(new Point(x, y));
					}

				}
			}

			return maximal;
		}

		private IImage DeleteSinglePixelNoise(IImage image)
		{
			return image.Create(
				(IImage input, int x, int y, int c) =>
				{
					if (x == 0 || y == 0 || x == (input.Width - 1) || y == (input.Height - 1))
					{
						return 0;
					}

					if (input.GetElement(x, y, c) != 0)
					{
						if (
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
		}

		private float[] CreateScoreMap(IImage image)
		{
			var candidate_area_map = new float[image.BoundingBox.BoundingSize.Area];

			for (int y = 0; y < (image.Height - this.MaskSize.Height); ++y)
			{
				for (int x = 0; x < (image.Width - this.MaskSize.Width); ++x)
				{
					var find_rect = new Rectangle(new Point(x, y), this.MaskSize);
					var score = ((double)Process.CountNoneZero(image.RegionOfImage(find_rect))) / this.MaskSize.Area;

					if (score < 0.10)
					{
						score = 0.0f;
					}

					candidate_area_map[y * image.Width + x] = (float)score;
				}
			}

			return candidate_area_map;
		}

		private List<Rectangle> RestrictRectangle(List<Rectangle> candidate_rects, IImage diff_img, ColorImage color_image)
		{
			Tuple<byte, byte> green_hue_range = new Tuple<byte, byte>(35, 70);
			Tuple<byte, byte> blue_hue_range = new Tuple<byte, byte>(100, 135);
			var color_ranges = new List<Tuple<byte,byte>>(){green_hue_range, blue_hue_range};
			//const byte green_hue = 80;	/// 70-130
			//const byte blue_hue = 100;	/// 200-270
			var hsv_img = new HSVImage(color_image);
			var rerect_dictionary = new Dictionary<int, List<Rectangle>>();
			var output_rects = new List<Rectangle>();
			var skip_rect = new List<int>();

			for (int i = 0; i < candidate_rects.Count; ++i)
			{
				for (int j = 0; j < candidate_rects.Count; ++j)
				{
					if (skip_rect.Contains(j))
					{
						continue;
					}

					if (IsIntersection(candidate_rects[i], candidate_rects[j]))
					{
						skip_rect.Add(i);
						skip_rect.Add(j);
						if (!rerect_dictionary.ContainsKey(i))
						{
							rerect_dictionary[i] = new List<Rectangle>();
						}
						rerect_dictionary[i].Add(candidate_rects[j]);
					}
				}
			}
			
			/// 候補である矩形のうちまったく重複していない矩形を削除する
			for (int i = 0; i < candidate_rects.Count; ++i)
			{
				if (skip_rect.Contains(i))
				{
					continue;
				}

				var counts = InRange(hsv_img.RegionOfImage(candidate_rects[i]), color_ranges);

				int count = int.MinValue;
				foreach(var c in counts)
				{
					if(count < c)
					{
						count = c;
					}
				}

				if(count > (candidate_rects[i].BoundingSize.Area * 0.3))
				{
					continue;
				}

				output_rects.Add(candidate_rects[i]);
			}

			// 候補である矩形から重複している分について削除する

			/*foreach (var rects in rerect_dictionary)
			{
				Rectangle most_candidate = new Rectangle();

				foreach (var r in rects.Value)
				{
					var counts = InRange(hsv_img.RegionOfImage(r), color_ranges);

					foreach (var c in counts)
					{
						if (c > (r.BoundingSize.Area * 0.1))
						{
							most_candidate = r;
						}
					}
				}

				if (most_candidate.BoundingSize.Area > 0)
				{
					output_rects.Add(most_candidate);
				}
			}*/
			
			foreach (var rects in rerect_dictionary)
			{
				int max_count = 0;
				Rectangle most_candidate = new Rectangle();

				foreach (var r in rects.Value)
				{
					var roi_diff_img = diff_img.RegionOfImage(r);
					var count = Process.CountNoneZero(roi_diff_img);

					if (count > max_count)
					{
						max_count = count;
						most_candidate = r;
					}
				}

				output_rects.Add(most_candidate);
			}

			return output_rects;
		}

		private List<Rectangle> RemoveIntersectionRectangle(IImage image, List<Rectangle> input_rects)
		{
			var skip_lists = new List<int>();
			var intersection_list = new Dictionary<int, List<int>>();

			for(int i = 0; i < input_rects.Count; ++i)
			{
				for(int j = i; j < input_rects.Count; ++j)
				{
					if(IsIntersection(input_rects[i], input_rects[j]))
					{
						if(!intersection_list.ContainsKey(i))
						{
							intersection_list.Add(i, new List<int>());
						}

						intersection_list[i].Add(j);
					}
				}
			}

			var results = new List<int>();

			foreach(var inter in intersection_list)
			{
				int max_score = 0, max_index = -1;
				foreach(var i in inter.Value)
				{
					
					if(skip_lists.Contains(i))
					{
						continue;
					}

					int sum_br = 0;
					int sum_gr = 0;

					var roi = image.RegionOfImage(input_rects[i]);

					for (int y = 0; y < roi.Height; ++y)
					{
						for (int x = 0; x < roi.Width; ++x)
						{
							sum_br += Math.Abs(roi.GetElement(x, y, 0) - roi.GetElement(x, y, 2));
							sum_gr += Math.Abs(roi.GetElement(x, y, 1) - roi.GetElement(x, y, 2));
						}
					}

					var score = (sum_br + sum_gr);
					if (max_score < score)
					{
						max_score = score;
						skip_lists.Add(max_index);
						max_index = i;
					}
					else
					{
						skip_lists.Add(i);
					}
					

					results.Add(max_index);
				}
			}
			
			var result_rect = new List<Rectangle>();

			foreach(var r in results)
			{
				if(r < 0)
				{
					continue;
				}

				var rect = input_rects[r];
				bool skip = false;

				foreach (var rr in result_rect)
				{
					if(rr == rect)
					{
						skip = true;
						break;
					}
				}

				if(skip)
				{
					continue;
				}

				result_rect.Add(rect);
			}

			return result_rect;
		}

		private List<Rectangle> RemoveNearRectangle(List<Rectangle> rect)
		{
			var tmp_output = new List<Rectangle>();
			var intersection_dictionary = new Dictionary<int, List<int>>();

			for(int i = 0; i < rect.Count; ++i)
			{
				for(int j = i + 1; j < rect.Count; ++j)
				{
					if(IsIntersection(rect[i], rect[j]))
					{
						if(!intersection_dictionary.ContainsKey(i))
						{
							intersection_dictionary.Add(i, new List<int>());
						}
						intersection_dictionary[i].Add(j);
					}
				}
			}

			var skip_lists = new List<int>();
			foreach(var intersection in intersection_dictionary)
			{
				skip_lists.Add(intersection.Key);
				var avg = rect[intersection.Key];
				var n = intersection.Value.Count + 1;
				foreach(var i in intersection.Value)
				{
					skip_lists.Add(i);
					avg.X += rect[i].X;
					avg.Y += rect[i].Y;
					avg.Width += rect[i].Width;
					avg.Height += rect[i].Height;
				}
				
				avg.X /= n;
				avg.Y /= n;
				avg.Width /= n;
				avg.Height /= n;
				tmp_output.Add(avg);
			}

			for(int i = 0; i < rect.Count; ++i)
			{
				if(skip_lists.Contains(i))
				{
					continue;
				}

				tmp_output.Add(rect[i]);
			}

			return tmp_output;
		}

		private List<int> InRange(IImage image, List<Tuple<byte, byte>> range)
		{
			var result = new List<int>(range.Count);
			
			foreach(var r in range)
			{
				int count = 0;
				for (int y = 0; y < image.Height; ++y )
				{
					for (int x = 0; x < image.Width; ++x)
					{
						var elem = image.GetElement(x,y, 0);
						if(elem > r.Item1 && elem < r.Item2)
						{
							++count;
						}
					}
				}

				result.Add(count);
			}

			return result;
		}

		private float[] MedianFilter(float[] pixels, Size size)
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
		
		private bool IsIntersection(Rectangle a, Rectangle b)
		{
			int r1MinX = Math.Min(a.X, a.X + a.Width);
			int r1MaxX = Math.Max(a.X, a.X + a.Width);
			int r1MinY = Math.Min(a.Y, a.Y + a.Height);
			int r1MaxY = Math.Max(a.Y, a.Y + a.Height);

			// Compute the min and max of the second rectangle on both axes
			int r2MinX = Math.Min(b.X, b.X + b.Width);
			int r2MaxX = Math.Max(b.X, b.X + b.Width);
			int r2MinY = Math.Min(b.Y, b.Y + b.Height);
			int r2MaxY = Math.Max(b.Y, b.Y + b.Height);

			// Compute the intersection boundaries
			int interLeft = Math.Max(r1MinX, r2MinX);
			int interTop = Math.Max(r1MinY, r2MinY);
			int interRight = Math.Min(r1MaxX, r2MaxX);
			int interBottom = Math.Min(r1MaxY, r2MaxY);

			// If the intersection is valid (positive non zero area), then there is an intersection
			return ((interLeft < interRight) && (interTop < interBottom));
		}
	}
}
