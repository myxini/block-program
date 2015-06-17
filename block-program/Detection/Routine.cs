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

        public Routine()
        {
            Instructions = list_instruction;
        }

        public IEnumerable<Instruction> Instructions { get; private set; }
    }
}
