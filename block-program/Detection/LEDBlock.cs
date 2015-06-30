namespace Myxini.Recognition
{
	class LEDBlock : Instruction
	{
		LEDBlock(int idenitify) : base(Command.LED, new BlockParameter(new int[]{idenitify, 0}))
		{

		}
	}
}
