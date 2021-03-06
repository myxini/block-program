﻿using Myxini.Communication;
using Myxini.Recognition;
using Myxini.Recognition.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myxini.Execution
{
    public class BlockProgramExecuter : IBlockProgramExecuter
    {
        private IBoard whiteboard = new ManuallyCalibratedWhiteboard();

        /// <summary>
        /// 通信するやつ
        /// </summary>
        private CommunicationService service;

        /// <summary>
        /// ホワイトボードを撮るカメラ
        /// </summary>
        private ICamera camera;

        public BlockProgramExecuter(CommunicationService service)
        {
            this.service = service;

            Initialize();
        }

        /// <summary>
        /// ホワイトボード上に構成されたプログラムを1回読んで実行する
        /// </summary>
        public void Execute()
        {
            // カメラでホワイトボードをパシャリ
			IImage image_whiteboard = null;
			do
			{
				image_whiteboard = camera.Capture();
			} while (image_whiteboard.Width == 0 || image_whiteboard.Height == 0);

			//Myxini.Recognition.Image.DebugOutput.SaveImage(new string[] { "whiteboard_depth.png", "whiteboard_color.png" }, image_whiteboard.Clone());

			// 写真からScriptを作る
			Myxini.Recognition.Recognizer recognizer = new Recognition.Recognizer();

			IImage backgrond_deleted_image = whiteboard.GetBackgroundDeleteImage(image_whiteboard).Clone();
			Myxini.Recognition.Image.DebugOutput.SaveImage(new string[] { "background_deleted_depth.png", "background_deleted_color.png" }, backgrond_deleted_image.Clone());

			Script script = recognizer.Recognition(
				backgrond_deleted_image
            );

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

        private void Initialize()
        {
            camera = new Kinect();

			while (!camera.IsOpened) ;

            // キャリブレーション
            whiteboard.Calibration(camera);
        }
    }
}
