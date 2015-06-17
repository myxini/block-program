namespace Myxini.Recognition
{
	public class ControlBlock : Instruction
	{
		public ControlBlock(Command command, BlockParameter parameter)
			: base(command, parameter, true)
		{
		}
	}
}
