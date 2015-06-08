namespace Myxini.Recognition
{
	public class RotateBlock : MovementBlock
	{
		RotateBlock(int angle) : base(Command.Rotate, new BlockParameter(new int[]{angle, 0}))
		{

		}
	}
}
