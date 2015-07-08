using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    class ConsoleOutput : IOutput
    {
        private bool linefeed = true;

        public ConsoleOutput() { }

        public ConsoleOutput(bool linefeed)
        {
            this.linefeed = linefeed;
        }

        public virtual void Output(string message)
        {
            Console.Write(message);
            if (linefeed)
            {
                Console.WriteLine();
            }
        }
    }
}
