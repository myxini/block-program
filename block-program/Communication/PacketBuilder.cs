using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myxini.Communication
{
    class PacketBuilder
    {
        public Robot.CommandList Build(IEnumerable<Myxini.Recognition.InstructionBlock> instrucions)
        {
            return new Robot.CommandList();
        }
    }
}
