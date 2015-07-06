using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myxini.Communication.Robot
{
    /// <summary>
    /// ロボットへの旋回コマンドをあらわすクラス
    /// </summary>
    class RotateCommand : MoveCommand
    {
        public float AngularVelocity { get; set; }
        public float Angle { get; set; }

        public RotateCommand()
        {
            this.CommandID = 0x02;
        }

        public override Packet ToPacket()
        {
            var packet = new Pc2RobotPacket()
            {
                RobotID = this.RobotID,
                IsNeedInterrupt = this.IsNeedInterrupt,
                CommandID = this.CommandID,
                Property1 = (byte)(this.AngularVelocity * 127),
                Property2 = (byte)(this.Angle)
            };
            packet.AddCheckSum();
            return packet;
        }
    }
}
