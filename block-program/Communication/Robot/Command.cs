using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myxini.Communication.Robot
{
    /// <summary>
    /// ロボットへのコマンド1つをあらわすクラス
    /// </summary>
    public abstract class Command
    {
        public byte RobotID { get; set; }
        public bool IsNeedInterrupt { get; set; }
        public byte CommandID { get; protected set; }
        public abstract Packet ToPacket();
    }
}
