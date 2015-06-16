using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myxini.Communication.Robot
{
    class Pc2RobotPacket : Packet
    {
        enum PacketIndex : int
        {
            HEAD_HIGH = 0,
            HEAD_LOW,
            ROBOT_ID,
            INTERRUPT_COMAND_ID,
            PROP1,
            PROP2,
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

        public UInt16 HEAD
        {
            get
            {
                return (UInt16)(((UInt16)(((UInt16)this._packetData[(int)PacketIndex.HEAD_HIGH]) << 8 )) & ((UInt16)this._packetData[(int)PacketIndex.HEAD_LOW]));
            }
            set
            {
                this._packetData[(int)PacketIndex.HEAD_HIGH] = (byte)((value >> 8) & 0xff);
                this._packetData[(int)PacketIndex.HEAD_LOW] = (byte)(value & 0xff);
            }
        }
        public byte RobotID
        {
            get
            {
                return this._packetData[(int)PacketIndex.ROBOT_ID];
            }
            set
            {
                this._packetData[(int)PacketIndex.ROBOT_ID] = value;
            }
        }

        public bool IsNeedInterrupt
        {
            get
            {
                return (this._packetData[(int)PacketIndex.INTERRUPT_COMAND_ID] & 0x80) != 0;
            }
            set
            {
                if(value)
                {
                    this._packetData[(int)PacketIndex.INTERRUPT_COMAND_ID]
                        = (byte)(this._packetData[(int)PacketIndex.INTERRUPT_COMAND_ID] | 0x80);
                }
                else
                {
                    this._packetData[(int)PacketIndex.INTERRUPT_COMAND_ID]
                        = (byte)(this._packetData[(int)PacketIndex.INTERRUPT_COMAND_ID] & 0x7f);
                }
            }
        }

        public byte CommandID
        {
            get
            {
                return (byte)(this._packetData[(int)PacketIndex.INTERRUPT_COMAND_ID] & 0x7f);
            }
            set
            {
                this._packetData[(int)PacketIndex.INTERRUPT_COMAND_ID] &= (byte)((value & 0x7f) | 0x80);
            }
        }

        protected byte Property1
        {
            get
            {
                return (byte)(this._packetData[(int)PacketIndex.PROP1]);
            }
            set
            {
                this._packetData[(int)PacketIndex.PROP1] = value;
            }
        }

        protected byte Property2
        {
            get
            {
                return (byte)(this._packetData[(int)PacketIndex.PROP2]);
            }
            set
            {
                this._packetData[(int)PacketIndex.PROP2] = value;
            }
        }
    }
}
