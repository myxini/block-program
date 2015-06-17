using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myxini.Recognition
{
	class Classifier : IClassifier
	{
		private class Pattern
		{
			private Image.IImage pattern;
			public IBlock Block { get; private set; }

			public Pattern(Image.IImage pattern, IBlock block)
			{
				this.pattern = pattern;
				Block = block;
			}

			public double Match(Image.IImage image)
			{
				// ここでパターンマッチング
				// とりあえず SADで
				uint distance = 0;
				for (int y = 0; y < image.Width; ++y)
				{
					for (int x = 0; x < image.Height; ++x)
					{
						distance += (uint)Math.Pow(image.GetElement(x, y, 0) - this.pattern.GetElement(x, y, 0), 2);
					}
				}

				return distance;

				// 仮に値を返す
				//return (new Random()).NextDouble() * 2 - 1;
			}
		}

		private IList<Pattern> patterns = new List<Pattern>
        {
            //LED
            new Pattern(
                /* なにかパターンの画像 */null, 
                new Instruction(Command.LED, new BlockParameter(new int[]{ 0 }), false)
            ),
            new Pattern(
                /* なにかパターンの画像 */null, 
                new Instruction(Command.LED, new BlockParameter(new int[]{ 1 }), false)
            ),
            //Move
            new Pattern(
                /* なにかパターンの画像 */null, 
                new Instruction(Command.Move, new BlockParameter(new int[]{ -3 }), false)
            ),
            new Pattern(
                /* なにかパターンの画像 */null, 
                new Instruction(Command.Move, new BlockParameter(new int[]{ -2 }), false)
            ),
            new Pattern(
                /* なにかパターンの画像 */null, 
                new Instruction(Command.Move, new BlockParameter(new int[]{ -1 }), false)
            ),
            new Pattern(
                /* なにかパターンの画像 */null, 
                new Instruction(Command.Move, new BlockParameter(new int[]{ 1 }), false)
            ),
            new Pattern(
                /* なにかパターンの画像 */null, 
                new Instruction(Command.Move, new BlockParameter(new int[]{ 2 }), false)
            ),
            new Pattern(
                /* なにかパターンの画像 */null, 
                new Instruction(Command.Move, new BlockParameter(new int[]{ 3 }), false)
            ),
            //Rotate
            new Pattern(
                /* なにかパターンの画像 */null, 
                new Instruction(Command.Rotate, new BlockParameter(new int[]{ -1 }), false)
            ),
            new Pattern(
                /* なにかパターンの画像 */null, 
                new Instruction(Command.Rotate, new BlockParameter(new int[]{ 1 }), false)
            ),
            //MicroSwitch
            new Pattern(
                /* なにかパターンの画像 */null, 
                new Instruction(Command.MicroSwitch, new BlockParameter(), false)
            ),
            //PSD
            new Pattern(
                /* なにかパターンの画像 */null, 
                new Instruction(Command.PSD, new BlockParameter(), false)
            ),
            //Start
            new Pattern(
                /* なにかパターンの画像 */null, 
                new Instruction(Command.Start, new BlockParameter(), false)
            ),
            //End
            new Pattern(
                /* なにかパターンの画像 */null, 
                new Instruction(Command.End, new BlockParameter(), false)
            ),
        };

		public IBlock Clustering(Raw.IRawBlock raw_block)
		{
			// ここでパターンマッチングして最もマッチする
			Pattern pattern_max_matching = patterns
					.OrderByDescending(pattern => Math.Abs(pattern.Match(raw_block.BoundingImage)))
					.First();
			return pattern_max_matching.Block;
		}
	}
}
