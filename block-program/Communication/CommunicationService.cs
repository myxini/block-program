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
            this.RobotPort.DataReceived += DataReceived;
        }

        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var port = sender as SerialPort;
            if(port == null)
            {
                return;
            }
            string indata = port.ReadExisting();
            byte[] data = System.Text.ASCIIEncoding.ASCII.GetBytes(indata);
            var packet = new Robot.Robot2PcPacket(data);
        }

        protected void Do(Robot.Command command)
        {
            if(this.RobotPort.IsOpen)
            {
                var packet = (byte[])command.ToPacket();
                this.RobotPort.Write(packet, 0, packet.Count());
            }
        }
    }
}
