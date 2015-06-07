namespace Myxini.Recognition
{
	class LEDBlock : MovementBlock
	{
		LEDBlock(int idenitify) : base(Command.LED, new BlockParameter(new int[]{idenitify, 0}))
		{

		}
	}
}
