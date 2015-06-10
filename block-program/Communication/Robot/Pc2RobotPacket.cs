using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myxini.Communication.Robot
{
    class Pc2RobotPacket : Packet
    {
        protected const byte[] HEAD = {0x06, 0x09};
    }
}
