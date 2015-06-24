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
        private const int BAUDRATE = 9600;
        private const Parity PARITY = Parity.None;
        private const int DATABITS = 8;
        private const StopBits STOPBITS = StopBits.One;
        public SerialPort RobotPort { get; set; }
        public string RobotPortName
        {
            get
            {
                return this.RobotPort.PortName;
            }
            set
            {
                this.RobotPort.PortName = value;
            }
        }
        public Script RobotScript { get; private set; }
        private bool _isRunning = false;
        public bool IsRunning 
        { 
            get
            {
                return this.IsRunning;
            }
        }

        public static string[] GetAvailablePorts()
        {
            return SerialPort.GetPortNames();
        }

        public CommunicationService(string portname)
        {
            this.RobotPort = new SerialPort()
            {
                BaudRate = BAUDRATE,
                Parity = PARITY,
                DataBits = DATABITS,
                StopBits = STOPBITS
            };
            this.RobotPortName = portname;
            this.RobotPort.DataReceived += DataReceived;
        }

        ~CommunicationService()
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
            this.RobotPort.Open();
            this._isRunning = this.RobotPort.IsOpen;
        }

        public void Stop()
        {
            if(!this._isRunning)
            {
                return;
            }
            if(this.RobotPort.IsOpen)
            {
                this.RobotPort.Close();
            }
        }
    }
}
