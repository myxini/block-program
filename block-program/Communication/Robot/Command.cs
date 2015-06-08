using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myxini.Communication.Robot
{
    /// <summary>
    /// ロボットへのコマンド1つをあらわすクラス
    /// </summary>
    public abstract class Command
    {
        public abstract byte[] ToByteArray();
    }
}
