namespace Myxini.Recognition
{
	public class RotateBlock : InstructionBlock
	{
		public RotateBlock(int angle) : base(Command.Rotate, new BlockParameter(new int[]{angle, 0}))
		{

		}
	}
}
