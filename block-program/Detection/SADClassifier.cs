﻿using Myxini.Recognition.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myxini.Recognition
{
	public class SADClassifier : IPatternMatchingAlgorithm
	{
		public Pattern Match(IEnumerable<Pattern> patterns, IImage raw_block)
		{
			// ここでパターンマッチングして最もマッチするパターンに
			// 紐付けしたIBlockを返す
			return patterns
							.OrderBy(pattern => Math.Abs(pattern.Match(raw_block, this)))
							.First();
		}

		public double Match(IImage pattern, IImage image)
		{
			if (pattern.Channel != image.Channel)
			{
				throw new BadImageFormatException();
			}

			// ここでパターンマッチング
			// とりあえず SADで
			uint distance = 0;
			for (int y = 0; y < image.Width; ++y)
			{
				for (int x = 0; x < image.Height; ++x)
				{
					for (int c = 0; c < image.Channel; ++c)
					{
						distance += (uint)Math.Pow(image.GetElement(x, y, c) - pattern.GetElement(x, y, c), 2);
					}
				}
			}

			return distance;
		}
	}
}
