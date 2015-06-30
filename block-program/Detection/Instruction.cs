﻿
namespace Myxini.Recognition
{
    public class Instruction : IBlock
    {
        public Instruction(Command command, BlockParameter parameter)
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
    }
}
