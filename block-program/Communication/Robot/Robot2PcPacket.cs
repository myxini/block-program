using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myxini.Communication.Robot
{
    class Robot2PcPacket : Packet
    {
        enum PacketIndex : int
        {
            HEAD_HIGH = 0,
            HEAD_LOW,
            ROBOT_ID,
            SENSOR_ID,
            SENSOR_VALUE_LOW,
            SENSOR_VALUE_HIGH,
            CHKSUM
        }

        private byte[] __packetData = new byte[7];
        protected override byte[] _packetData
        {
            get
            {
                return this.__packetData;
            }
        }
        public byte RobotID { get; private set; }
        public byte SensorID { get; private set; }
        public UInt16 SensorValue { get; private set; }

        public Robot2PcPacket(byte[] packet)
        {
            if (packet.Length < 7)
            {
                throw new InvalidOperationException("Packet Length is too short");
            }
            this.__packetData = packet;
            if (!(this.__packetData[(int)PacketIndex.HEAD_HIGH] == 0x12
                && this.__packetData[(int)PacketIndex.HEAD_LOW] == 0x20))
            {
                throw new InvalidOperationException("Pack Head is invalid");
            }
            if(!this.IsValid())
            {
                throw new InvalidOperationException("Packet check sum is invalid");
            }
            this.RobotID = packet[(int)PacketIndex.ROBOT_ID];
            this.SensorID = packet[(int)PacketIndex.SENSOR_ID];
            this.SensorValue = (UInt16)((packet[(int)PacketIndex.SENSOR_VALUE_HIGH] << 8) | packet[(int)PacketIndex.SENSOR_VALUE_LOW]);
        }

        public bool IsValid()
        {
            return Packet.CalcCheckSum(this._packetData) == 0;
        }
    }
}
