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
        /// <summary>
        /// 速さを表すパラメータ(1で最大)
        /// </summary>
        public float AngularVelocity { get; set; }
        /// <summary>
        /// 角度を表す。degでお願いします。
        /// </summary>
        public float Angle { get; set; }
        /// <summary>
        /// 角度とパルス数の変換を請け負う関数
        /// </summary>
        private float AngleToPulse(float deg)
        {
            return deg * 24.0f / 90.0f;
        }

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
                Property2 = (byte)(this.AngleToPulse(this.AngularVelocity))
            };
            packet.AddCheckSum();
            return packet;
        }
    }
}
