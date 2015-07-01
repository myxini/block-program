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
        #region シリアル用のconst値
        private const int BAUDRATE = 9600;
        private const Parity PARITY = Parity.None;
        private const int DATABITS = 8;
        private const StopBits STOPBITS = StopBits.One;
        #endregion
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
                return this._isRunning;
            }
            private set
            {
                this._isRunning = value;
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
            if(this.IsRunning)
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
                var packetbytes = (byte[])command.ToPacket();
                this.RobotPort.Write(packetbytes, 0, packetbytes.Count());
            }
        }

        private async void RunAsync()
        {
            try
            {
                this.RobotPort.Open();
                this.IsRunning = this.RobotPort.IsOpen;
            }
            catch(Exception e)
            {
                return;
            }
            await Task.Run(() =>
            {
                while (this.IsRunning)
                {
                    var script = this.RobotScript;
                    foreach(var routine in this.RobotScript.Routines)
                    {
                        routine.
                    }
                }
            });
            this.RobotPort.Close();
            
        }

        public void Run(Myxini.Recognition.Script script)
        {
            this.RobotScript = script;
            this.RunAsync();
        }

        public void Stop()
        {
            this.IsRunning = false;
        }
    }
}
