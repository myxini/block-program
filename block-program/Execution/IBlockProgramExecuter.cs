using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myxini.Execution
{
    public interface IBlockProgramExecuter
    {
        /// <summary>
        /// ホワイトボード上に構成されたプログラムを1回読んで実行する
        /// </summary>
        void Execute();

        /// <summary>
        /// 実行中のプログラムがあれば停止する
        /// </summary>
        void Stop();
    }
}
