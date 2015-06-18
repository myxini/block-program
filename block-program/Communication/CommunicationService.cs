using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using Myxini.Recognition;

namespace Myxini.Communication
{
    public class CommunicationService
    {
        public SerialPort RobotPort { get; set; }
        public Script RobotScript { get; private set; }
        private bool _isRunning = false;

        public CommunicationService()
        {
            this.RobotPort = new SerialPort();
            this.RobotPort.DataReceived += DataReceived;
        }

        public ~CommunicationService()
        {
            if(this._isRunning)
            {
                this.Stop();
            }
        }

        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if(!(sender is SerialPort))
            {
                return;
            }
            var port = sender as SerialPort;
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

        public void Run(Myxini.Recognition.Script script)
        {
            this.RobotScript = script;
            this._isRunning = true;
        }

        public void Stop()
        {
            if(!this._isRunning)
            {
                return;
            }
        }
    }
}
