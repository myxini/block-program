
namespace Myxini.Recognition
{
    public class InstructionBlock : IBlock
    {
        public InstructionBlock(Command command, BlockParameter parameter)
        {
            this.CommandIdentification = command;
            this.Parameter = parameter;
        }

        public bool IsControlBlock
        {
            get
            {
                return false;
            }
        }
        public Command CommandIdentification { get; private set; }
        public BlockParameter Parameter { get; private set; }

        public override string ToString()
        {
            return string.Format(
                "      [InstructionBlock] Command: {0}, Parameters: {1}",
                CommandIdentification,
                Parameter.ValueLength() == 1 ? Parameter.Value(0).ToString() : "null"
            );
        }
    }
}
