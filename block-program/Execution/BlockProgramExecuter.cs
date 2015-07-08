using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myxini.Execution
{
    class BlockProgramExecuter : IBlockProgramExecuter
    {
        /// <summary>
        /// 通信するやつ
        /// </summary>
        private Communication.CommunicationService service;

        /// <summary>
        /// ホワイトボードを撮るカメラ
        /// </summary>
        private Recognition.Image.ICamera camera = new Recognition.Image.Kinect();

        public BlockProgramExecuter(Communication.CommunicationService service)
        {
            this.service = service;
        }

        /// <summary>
        /// ホワイトボード上に構成されたプログラムを1回読んで実行する
        /// </summary>
        public void Execute()
        {
            // カメラでホワイトボードをパシャリ
            Recognition.Image.IImage image_whiteboard = camera.Capture();

            // 写真からScriptを作る
            Recognition.Recognizer recognizer = new Recognition.Recognizer();
            Recognition.Script script = recognizer.Recognition(image_whiteboard);

            // 通信するやつを使ってScriptを実行
            service.Run(script);
        }

        /// <summary>
        /// 実行中のプログラムがあれば停止する
        /// </summary>
        public void Stop()
        {
            // 通信するやつを使って実行を停止
            service.Stop();
        }
    }
}
