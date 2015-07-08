using Myxini.Communication;
using Myxini.Execution;
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
            // 入出力するやつを生成
            string prompt = "> ";
            IInput input = new ConsoleInput(prompt);
            IOutput error_out = new CopnsoleErrorOutput();
            IOutput output = new ConsoleOutput();

            // ポート名を入力
            string portname = input.Input("Enter COM port name.");

            // おはなしサービスを生成
            CommunicationService service = null;
            try
            {
                service = new CommunicationService(portname);
            }
            catch (Exception e)
            {
                error_out.Output(e.StackTrace);
            }

            // 実行器を生成
            BlockProgramExecuter executer = new BlockProgramExecuter(service);
            
            // 実行
            executer.Execute();

            // おまじない
            Console.ReadLine();
        }
    }
}
