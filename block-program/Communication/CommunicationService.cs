﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        private const int COMMAND_DURATION = 4500;
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
        #region センサIDとセンサ名をひもづける定数値
        const byte SENSORID_PSD = 0x07;
        const byte SENSORID_MICROSWITCH = 0x02;
        #endregion
        #region ロボットにおいてどのスレッドが走るかを決めるしきい値群
        private const int STATE_PSD_THRES = 150;
        private const int STATE_MICROSWITCH_THRES = 1;
        #endregion
        private Dictionary<Command, Robot.CommandList> _robotScript;
        private Command _currentInstructionType = Command.Start;
        private PacketBuilder _builer = new PacketBuilder();
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
            this._robotScript = new Dictionary<Command, Robot.CommandList>();
            this.RobotPort.DataReceived += DataReceived;
            this._builer.RobotID = 1;
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
            int bytesToRed = port.BytesToRead;
            if(bytesToRed < 7)
            {
                return;
            }
            /*
            string indata = port.ReadExisting();
            byte[] data = System.Text.ASCIIEncoding.ASCII.GetBytes(indata);
            int headIndex = Array.FindIndex(
                data,
                new Predicate<byte>((byte b) => { return b == 0x12; })
                );
            if(headIndex + 7 > data.Length)
            {
                return;
            }
            byte[] packetData = data.Skip(headIndex).Take(7).ToArray();
             */
            byte[] revBuffer = new byte[bytesToRed];
            port.Read(revBuffer, 0, bytesToRed);
            int headindex = Array.FindLastIndex(
                revBuffer,
                revBuffer.Length - 6,
                new Predicate<byte>((byte b) => { return b == 0x12; }));
            if(headindex < 0)
            {
                return;
            }
            byte[] packetData = revBuffer.Skip(headindex).Take(7).ToArray();
//            System.Diagnostics.Debug.WriteLine(indata);
//            System.Diagnostics.Debug.WriteLine(BitConverter.ToString(data));
            System.Diagnostics.Debug.WriteLine(BitConverter.ToString(packetData));
            try
            {
                var packet = new Robot.Robot2PcPacket(packetData);
                this.UpdateCurrentCommand(packet.SensorID, packet.SensorValue);
            }
            catch (Exception exce)
            {
                System.Diagnostics.Debug.WriteLine(exce.Message);
            }
        }

        private void UpdateCurrentCommand(byte sensorID, ushort sensorValue)
        {
            System.Diagnostics.Debug.WriteLine(sensorID.ToString() + ": " + sensorValue.ToString());
            if(sensorID == SENSORID_PSD && sensorValue >= STATE_PSD_THRES)
            {
                this._currentInstructionType = Command.PSD;
            }
            else if(sensorID == SENSORID_MICROSWITCH && sensorValue == STATE_MICROSWITCH_THRES)
            {
                this._currentInstructionType = Command.MicroSwitch;
            }
            else
            {
                this._currentInstructionType = Command.Start;
            }
        }

        protected void Do(Robot.Command command)
        {
            if(this.RobotPort.IsOpen)
            {
                var packetbytes = (byte[])command.ToPacket();
                for (int i = 0; i < packetbytes.Count(); ++i)
                {
                    this.RobotPort.Write(packetbytes, i, 1);
                }
//                this.RobotPort.Write(packetbytes, 0, packetbytes.Count());
            }
        }

        private void RunLoop()
        {
            while (this.IsRunning)
            {
                var script = this._robotScript;
                var currentInstruction = this._currentInstructionType;
                if (!this._robotScript.ContainsKey(currentInstruction))
                {
                    currentInstruction = Command.Start;
                }
                foreach (var command in this._robotScript[currentInstruction])
                {
                    this.Do(command);
                    Thread.Sleep(COMMAND_DURATION);
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
            catch(Exception e)
            {
                throw e;
            }
            await Task.Run(() =>
            {
                this.RunLoop();
            });
            this.RobotPort.Close();   
        }

        private void LoadScript(Myxini.Recognition.Script script)
        {
            this._robotScript.Clear();
            foreach(var routine in script.Routines)
            {
                this._robotScript.Add(
                    routine.Trigger.CommandIdentification,
                    this._builer.Build(routine.Instructions)
                );
            }
        }

        public void Run(Myxini.Recognition.Script script)
        {
            this.LoadScript(script);
            this.RunAsync();
        }

        public void Stop()
        {
            this.IsRunning = false;
        }
    }
}
