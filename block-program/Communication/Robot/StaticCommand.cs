using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myxini.Communication.Robot
{
    /// <summary>
    /// ロボットへの何か静的動作コマンドをあらわすクラス
    /// </summary>
    class StaticCommand : Command
    {
        public StaticCommand()
        {
            public Packet ToPacket()
            {
                throw new NotSupportedException();
                return null;
            }
        }
    }
}
