namespace Myxini.Recognition
{
	public class RotateBlock : Instruction
	{
		public RotateBlock(int angle) : base(Command.Rotate, new BlockParameter(new int[]{angle, 0}))
		{

		}
	}
}
