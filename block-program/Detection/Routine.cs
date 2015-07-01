using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myxini.Recognition
{
    public class Routine
    {
        private IList<InstructionBlock> list_instruction = new List<InstructionBlock>();

        public Routine(ControlBlock trigger)
        {
            Instructions = list_instruction;
            Trigger = trigger;
        }

        public IEnumerable<InstructionBlock> Instructions { get; private set; }
        public ControlBlock Trigger { get; private set; }

        public void Append(InstructionBlock instruction)
        {
            list_instruction.Add(instruction);
        }
    }
}
