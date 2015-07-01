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
        private Dictionary<Command, List<InstructionBlock>> _robotScript;
        private Command _currentInstructionType;
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
            this._robotScript = new Dictionary<Command, List<InstructionBlock>>();
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
            this.UpdateCurrentCommand(packet.SensorID, packet.SensorValue);
        }

        private void UpdateCurrentCommand(byte sensorID, ushort sensorValue)
        {
        }

        protected void Do(Robot.Command command)
        {
            if(this.RobotPort.IsOpen)
            {
                var packetbytes = (byte[])command.ToPacket();
                this.RobotPort.Write(packetbytes, 0, packetbytes.Count());
            }
        }

        private void RunLoop()
        {
            while (this.IsRunning)
            {
                var script = this._robotScript;
                var currentInstruction = this._currentInstructionType;
                foreach(var instruction in this._robotScript[currentInstruction])
                {
                }
            }
        }

        private async void RunAsync()
        {
            try
            {
                this.RobotPort.Open();
                this.IsRunning = this.RobotPort.IsOpen;
            }
            catch(Exception)
            {
                return;
            }
            await Task.Run(() =>
            {
                this.RunLoop();
            });
            this.RobotPort.Close();   
        }

        private Dictionary<Command, List<InstructionBlock>> LoadScript(Myxini.Recognition.Script script)
        {
            var dst = new Dictionary<Command, List<InstructionBlock>>();
            foreach(var routine in script.Routines)
            {
                dst.Add(
                    routine.Trigger.CommandIdentification,
                    new List<InstructionBlock>(routine.Instructions)
                );
            }
            return dst;
        }

        public void Run(Myxini.Recognition.Script script)
        {
            this._robotScript = this.LoadScript(script);
            this.RunAsync();
        }

        public void Stop()
        {
            this.IsRunning = false;
        }
    }
}
