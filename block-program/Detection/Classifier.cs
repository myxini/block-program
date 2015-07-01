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
			}
		}

		private IList<Pattern> patterns = new List<Pattern>
        {
            //LED
            new Pattern(
                new Image.BitmapImage(Properties.Resources.PatternDummy), // ダミービットマップの例
                new InstructionBlock(Command.LED, new BlockParameter(new int[]{ 0 }))
            ),
            new Pattern(
                /* なにかパターンの画像 */null, 
                new InstructionBlock(Command.LED, new BlockParameter(new int[]{ 1 }))
            ),
            //Move
            new Pattern(
                /* なにかパターンの画像 */null, 
                new InstructionBlock(Command.Move, new BlockParameter(new int[]{ -3 }))
            ),
            new Pattern(
                /* なにかパターンの画像 */null, 
                new InstructionBlock(Command.Move, new BlockParameter(new int[]{ -2 }))
            ),
            new Pattern(
                /* なにかパターンの画像 */null, 
                new InstructionBlock(Command.Move, new BlockParameter(new int[]{ -1 }))
            ),
            new Pattern(
                /* なにかパターンの画像 */null, 
                new InstructionBlock(Command.Move, new BlockParameter(new int[]{ 1 }))
            ),
            new Pattern(
                /* なにかパターンの画像 */null, 
                new InstructionBlock(Command.Move, new BlockParameter(new int[]{ 2 }))
            ),
            new Pattern(
                /* なにかパターンの画像 */null, 
                new InstructionBlock(Command.Move, new BlockParameter(new int[]{ 3 }))
            ),
            //Rotate
            new Pattern(
                /* なにかパターンの画像 */null, 
                new InstructionBlock(Command.Rotate, new BlockParameter(new int[]{ -1 }))
            ),
            new Pattern(
                /* なにかパターンの画像 */null, 
                new InstructionBlock(Command.Rotate, new BlockParameter(new int[]{ 1 }))
            ),
            //MicroSwitch
            new Pattern(
                /* なにかパターンの画像 */null, 
                new InstructionBlock(Command.MicroSwitch, new BlockParameter())
            ),
            //PSD
            new Pattern(
                /* なにかパターンの画像 */null, 
                new ControlBlock(Command.PSD, new BlockParameter())
            ),
            //Start
            new Pattern(
                /* なにかパターンの画像 */null, 
                new ControlBlock(Command.Start, new BlockParameter())
            ),
            //End
            new Pattern(
                /* なにかパターンの画像 */null, 
                new ControlBlock(Command.End, new BlockParameter())
            ),
        };

		public IBlock Clustering(Raw.IRawBlock raw_block)
		{
			// ここでパターンマッチングして最もマッチするパターンに
            // 紐付けしたIBlockを返す
			Pattern pattern_max_matching = patterns
					.OrderByDescending(pattern => Math.Abs(pattern.Match(raw_block.BoundingImage)))
					.First();
			return pattern_max_matching.Block;
		}
	}
}
