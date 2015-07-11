using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Myxini.Recognition;
using System.Collections.Generic;
using System.Linq;

namespace RecognitionTest
{
	[TestClass]
	public class ScriptTest
	{
		private Script script = new Script();

		[TestMethod]
		public void AddTest()
		{
            ControlBlock start = new ControlBlock(Command.Start, new BlockParameter());
            script.Add(start);
            script.Add(new InstructionBlock(Command.LED, new BlockParameter(new int[]{ 0 })));
            script.Add(new InstructionBlock(Command.LED, new BlockParameter(new int[] { 1 })));

            ControlBlock psd = new ControlBlock(Command.PSD, new BlockParameter());
            script.Add(psd);
            script.Add(new InstructionBlock(Command.LED, new BlockParameter(new int[] { 0 })));
            script.Add(new InstructionBlock(Command.LED, new BlockParameter(new int[] { 1 })));

            script.Add(start);
            script.Add(new InstructionBlock(Command.Rotate, new BlockParameter(new int[] { 1 })));

            script.Add(new InstructionBlock(Command.Move, new BlockParameter(new int[] { 1 })), psd);
            
            System.Diagnostics.Debug.WriteLine(script);
        }

		[TestMethod]
		public void GetTest()
		{
            AddTest();
            IEnumerable<Routine> routines = script.Routines;
            System.Diagnostics.Debug.WriteLine("Count: " + routines.Count());

            foreach (Routine routine in routines)
            {
                System.Diagnostics.Debug.WriteLine(routine);
            }
		}
	}
}
