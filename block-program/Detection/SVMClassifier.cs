﻿using System;
using System.Collections.Generic;
using Myxini.Recognition.Image;

namespace Myxini.Recognition
{
	public class SVMClassifier : IClassifier
	{
		public SVMClassifier(string learn_directory)
		{
			this.LearnDirectory = learn_directory;

			if(!System.IO.Directory.Exists(learn_directory))
			{
				throw new System.IO.DirectoryNotFoundException();
			}
		}

		public IBlock Clustering(IImage raw_block, IPatternMatchingAlgorithm algorithm = null)
		{
			switch(raw_block.Channel)
			{
				case 3:
					DebugOutput.SaveColorImage(TARGET_FILE_NAME, raw_block);
					break;
				case 4:
					DebugOutput.SaveColorImage(TARGET_FILE_NAME, raw_block, 1);
					break;
			}
			var proc = new System.Diagnostics.Process();
			proc.StartInfo.FileName = EXECUTOR_NAME;
			proc.StartInfo.Arguments = String.Format(ARGUMENTS, TARGET_FILE_NAME, LearnDirectory);
			proc.Start();
			proc.WaitForExit();
			var result = proc.ExitCode;

			return GenerateBlockFromExitCode(result);
		}

		private IBlock GenerateBlockFromExitCode(int code)
		{
			switch(code)
			{
				case 1:
					return new InstructionBlock(Command.Rotate, new BlockParameter(new int[]{ -1 }));
				case 2:
					return new InstructionBlock(Command.Rotate, new BlockParameter(new int[]{ 1 }));
				case 3:
					return new InstructionBlock(Command.Move, new BlockParameter(new int[]{1}));
				case 4:
					return new ControlBlock(Command.Start, new BlockParameter());
				case 5:
					return new ControlBlock(Command.PSD, new BlockParameter());
				default:
					throw new ArgumentException();
			}
		}

		private string LearnDirectory;
		private string OutputFileName;
		private const string TARGET_FILE_NAME = "target.png";
		private const string EXECUTOR_NAME = "SVM.exe";
		private const string ARGUMENTS = "-m svm.model -i {0} --input-dir={1} -o _output.txt";
	}
}
