using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    class ConsoleInput : IInput
    {
        private string prompt;

        public ConsoleInput(string prompt)
        {
            this.prompt = prompt;
        }

        public string Input(string message)
        {
            Console.WriteLine(message);
            Console.Write(prompt);
            return Console.ReadLine();
        }
    }
}
