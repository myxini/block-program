namespace Myxini.Recognition
{
	public class ControlBlock : IBlock
	{
		public ControlBlock(Command command, BlockParameter parameter)
		{
            CommandIdentification = command;
            Parameter = parameter;
		}

        public bool IsControlBlock
        {
            get
            {
                return true;
            }
        }

        public Command CommandIdentification { get; private set; }
        public BlockParameter Parameter { get; private set; }
    }
}
