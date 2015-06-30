namespace Myxini.Recognition
{
	public class ControlBlock : IBlock
	{
		public ControlBlock(Command command, BlockParameter parameter)
		{
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
