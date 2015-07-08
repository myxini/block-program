using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    class CopnsoleErrorOutput : ConsoleOutput
    {
        public override void Output(string message)
        {
            base.Output("ERROR: ");
            base.Output(message);
        }
    }
}
