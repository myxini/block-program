namespace Myxini.Communication.Robot
{
    /// <summary>
    /// ロボットとやり取りするパケットをあらわすクラス
    /// </summary>
    public abstract class Packet
    {
        protected abstract byte[] _packetData { get; }
    }
}