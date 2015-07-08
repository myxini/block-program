//using Myxini.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    class Program
    {
        static void Main(string[] args)
        {
            string prompt = "> ";
            IInput input = new ConsoleInput(prompt);

            string portname = input.Input("Enter COM port name.");

            // CommunicationService servce = new CommunicationService();


            Console.WriteLine(portname);
            Console.ReadLine();
        }
    }
}
