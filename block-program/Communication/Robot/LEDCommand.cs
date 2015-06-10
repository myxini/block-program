using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myxini.Communication.Robot
{
    /// <summary>
    /// ロボットへのLチカコマンドをあらわすクラス
    /// </summary>
    class LEDCommand : StaticCommand
    {
        public LEDCommand()
        {
        }
        public Packet ToPacket()
        {
            throw new NotSupportedException();
        }
        
    }
}
