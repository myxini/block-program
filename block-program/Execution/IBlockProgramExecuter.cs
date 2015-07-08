using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myxini.Recognition
{
    interface IBlockProgramExecuter
    {
        /// <summary>
        /// ホワイトボード上に構成されたプログラムを1回読んで実行する
        /// </summary>
        void Execute();
    }
}
