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
        public byte LEDNumber { get; set; }
        public byte Switch { get; set; }
        public LEDCommand()
        {
            this.CommandID = 0x03;
        }
        public override Packet ToPacket()
        {
            var p = new Pc2RobotPacket()
            {
                CommandID = this.CommandID,
                RobotID = this.RobotID,
                IsNeedInterrupt = this.IsNeedInterrupt,
                Property1 = this.LEDNumber,
                Property2 = this.Switch
            };
            p.AddCheckSum();
            return p;
        }
        
    }
}
