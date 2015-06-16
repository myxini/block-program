namespace Myxini.Communication.Robot
{
    /// <summary>
    /// ロボットとやり取りするパケットをあらわすクラス
    /// </summary>
    public abstract class Packet
    {
        protected abstract byte[] _packetData { get; }

        private static byte calcCheckSum(byte[] packet)
        {
            byte sum = 0;
            foreach(var b in packet)
            {
                sum ^= b;
            }
            return sum;
        }

        /// <summary>
        /// PacketDataの最後の要素をチェックサムで埋めます
        /// </summary>
        protected void addCheckSum()
        {
            this._packetData[_packetData.Length - 1] = Packet.calcCheckSum(this._packetData);
        }
    }
}