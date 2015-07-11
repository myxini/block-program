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

        public override string ToString()
        {
            return string.Format(
                "      [ControlBlock] Command: {0}, Parameters: {1}",
                CommandIdentification,
                Parameter.ValueLength() == 1 ? Parameter.Value(0).ToString() : "null");
        }
    }
}
