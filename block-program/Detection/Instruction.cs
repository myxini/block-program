
namespace Myxini.Recognition
{
	public class Instruction : IBlock
	{
		public Instruction(Command command, BlockParameter parameter, bool is_control_block)
		{
			this.CommandIdentification = command;
			this.Parameter = parameter;
			this.IsControlBlock = is_control_block;
		}

		public bool IsControlBlock { get; private set; }
		public Command CommandIdentification { get; private set; }
		public BlockParameter Parameter { get; private set; }
	}
}
