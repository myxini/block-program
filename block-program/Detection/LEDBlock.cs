namespace Myxini.Recognition
{
	class LEDBlock : InstructionBlock
	{
		LEDBlock(int idenitify) : base(Command.LED, new BlockParameter(new int[]{idenitify, 0}))
		{

		}
	}
}
