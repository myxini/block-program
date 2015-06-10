namespace Myxini.Recognition
{
	public class RotateBlock : MovementBlock
	{
		public RotateBlock(int angle) : base(Command.Rotate, new BlockParameter(new int[]{angle, 0}))
		{

		}
	}
}
