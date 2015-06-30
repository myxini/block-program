namespace Myxini.Recognition
{
	class MoveBlock : Instruction
	{
		public MoveBlock(int velocity, int duration) : base(Command.Move, new BlockParameter(new int[]{velocity, duration}))
		{

		}
	}
}
