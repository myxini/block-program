using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myxini.Recognition
{
    public class Routine
    {
        private IList<Instruction> list_instruction = new List<Instruction>();

        public Routine(ControlBlock trigger)
        {
            Instructions = list_instruction;
            Trigger = trigger;
        }

        public IEnumerable<Instruction> Instructions { get; private set; }
        public ControlBlock Trigger { get; private set; }
    }
}
