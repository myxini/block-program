namespace Myxini.Recognition
{
	class MoveBlock : InstructionBlock
	{
		public MoveBlock(int velocity, int duration) : base(Command.Move, new BlockParameter(new int[]{velocity, duration}))
		{

		}
	}
}
