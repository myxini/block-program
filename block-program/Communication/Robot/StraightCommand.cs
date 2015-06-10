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
        public StraightCommand()
        {
        }
        public Packet ToPacket()
        {
            throw new NotImplementedException();
            return null;
        }
    }
}
