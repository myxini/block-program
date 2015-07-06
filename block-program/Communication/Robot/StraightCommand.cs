using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myxini.Communication.Robot
{
    /// <summary>
    /// ロボットへの直進コマンドをあらわすクラス
    /// </summary>
    class StraightCommand : MoveCommand
    {
        public float Velocity { get; set; }
        public float Time { get; set; }
        public StraightCommand()
        {
            this.CommandID = 0x01;
        }
        public override Packet ToPacket()
        {
            var p = new Pc2RobotPacket()
            {
                RobotID = this.RobotID,
                IsNeedInterrupt = this.IsNeedInterrupt,
                CommandID = this.CommandID,
                Property1 = (byte)(this.Velocity * 127),
                Property2 = (byte)(this.Time)
            };
            p.AddCheckSum();
            return p;
        }
    }
}
