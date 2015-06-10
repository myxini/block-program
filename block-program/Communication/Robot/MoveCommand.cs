using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myxini.Communication.Robot
{
    /// <summary>
    /// ロボットへの何か動けというコマンドをあらわすクラス
    /// </summary>
    class MoveCommand : Command
    {
        public MoveCommand()
        {
            public Packet ToPacket()
            {
                throw new NotSupportedException();
                return null;
            }
        }
    }
}
