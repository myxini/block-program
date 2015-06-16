using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace Myxini.Communication
{
    class CommunicationService
    {
        public SerialPort RobotPort { get; set; }
        public CommunicationService()
        {
            this.RobotPort = new SerialPort();
        }
    }
}
