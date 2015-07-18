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
        /// <summary>
        /// 速さを表すパラメータ。1.0で最大
        /// </summary>
        public float Velocity { get; set; }
        /// <summary>
        /// 回転数を表すパラメータ。1で1ブロック分進むかも
        /// </summary>
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
