using Myxini.Recognition.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myxini.Recognition
{
	public class Classifier : IClassifier
	{
		private IList<Pattern> patterns = new List<Pattern>
        {
            //LED
            new Pattern(
                new Image.BitmapImage(Properties.Resources.PatternLED0),
                new InstructionBlock(Command.LED, new BlockParameter(new int[]{ 0 }))
            ),
            new Pattern(
                new Image.BitmapImage(Properties.Resources.PatternLED1),
                new InstructionBlock(Command.LED, new BlockParameter(new int[]{ 1 }))
            ),
            //Move
            new Pattern(
                new Image.BitmapImage(Properties.Resources.PatternDummy), // ダミービットマップ
                new InstructionBlock(Command.Move, new BlockParameter(new int[]{ -3 }))
            ),
            new Pattern(
                new Image.BitmapImage(Properties.Resources.PatternDummy), // ダミービットマップ
                new InstructionBlock(Command.Move, new BlockParameter(new int[]{ -2 }))
            ),
            new Pattern(
                new Image.BitmapImage(Properties.Resources.PatternMoveBackward1),
                new InstructionBlock(Command.Move, new BlockParameter(new int[]{ -1 }))
            ),
            new Pattern(
                new Image.BitmapImage(Properties.Resources.PatternMoveForward1),
                new InstructionBlock(Command.Move, new BlockParameter(new int[]{ 1 }))
            ),
            new Pattern(
                new Image.BitmapImage(Properties.Resources.PatternDummy), // ダミービットマップ
                new InstructionBlock(Command.Move, new BlockParameter(new int[]{ 2 }))
            ),
            new Pattern(
                new Image.BitmapImage(Properties.Resources.PatternDummy), // ダミービットマップ
                new InstructionBlock(Command.Move, new BlockParameter(new int[]{ 3 }))
            ),
            //Rotate
            new Pattern(
                new Image.BitmapImage(Properties.Resources.PatternRotateCCW),
                new InstructionBlock(Command.Rotate, new BlockParameter(new int[]{ -1 }))
            ),
            new Pattern(
                new Image.BitmapImage(Properties.Resources.PatternRotateCW),
                new InstructionBlock(Command.Rotate, new BlockParameter(new int[]{ 1 }))
            ),
            //MicroSwitch
            new Pattern(
                new Image.BitmapImage(Properties.Resources.PatternMicroSwitch),
                new InstructionBlock(Command.MicroSwitch, new BlockParameter())
            ),
            //PSD
            new Pattern(
                new Image.BitmapImage(Properties.Resources.PatternPSD),
                new ControlBlock(Command.PSD, new BlockParameter())
            ),
            //Start
            new Pattern(
                new Image.BitmapImage(Properties.Resources.PatternStart),
                new ControlBlock(Command.Start, new BlockParameter())
            ),
            //End
            new Pattern(
                new Image.BitmapImage(Properties.Resources.PatternEnd),
                new ControlBlock(Command.End, new BlockParameter())
            ),
        };

		public IBlock Clustering(IImage raw_block, IPatternMatchingAlgorithm algorithm)
		{
			// ここでパターンマッチングして最もマッチするパターンに
            // 紐付けしたIBlockを返す
            Pattern pattern_max_matching = algorithm.Match(patterns, raw_block);
			return pattern_max_matching.Block;
		}
	}
}
