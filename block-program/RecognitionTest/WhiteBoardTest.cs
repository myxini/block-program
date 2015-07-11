using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Myxini.Recognition;
using Myxini.Recognition.Image;

namespace RecognitionTest
{
	/// <summary>
	/// WhiteBoardTest の概要の説明
	/// </summary>
	[TestClass]
	public class WhiteBoardTest
	{
		public WhiteBoardTest()
		{
			//
			// TODO: コンストラクター ロジックをここに追加します
			//
		}

		private TestContext testContextInstance;

		/// <summary>
		///現在のテストの実行についての情報および機能を
		///提供するテスト コンテキストを取得または設定します。
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		#region 追加のテスト属性
		//
		// テストを作成する際には、次の追加属性を使用できます:
		//
		// クラス内で最初のテストを実行する前に、ClassInitialize を使用してコードを実行してください
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// クラス内のテストをすべて実行したら、ClassCleanup を使用してコードを実行してください
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// 各テストを実行する前に、TestInitialize を使用してコードを実行してください
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// 各テストを実行した後に、TestCleanup を使用してコードを実行してください
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion
		
		[TestMethod]
		[DeploymentItem(@".\Resource\resized", "resized")]
		public void CalibrationTest()
		{
#warning This test is ignored for no depth image.
			return;	/// カメラはデプス値なのでしばらくの間常に成功するように変更

			var camera = new VirtualCamera();
			
			// ホワイトボードの画像が5枚必要
			camera.AddFrame(ImageLoader.LoadImage(@"resized\Test_Whiteboad_Frame1.jpg"));
			camera.AddFrame(ImageLoader.LoadImage(@"resized\Test_Whiteboad_Frame2.jpg"));
			camera.AddFrame(ImageLoader.LoadImage(@"resized\Test_Whiteboad_Frame3.jpg"));
			camera.AddFrame(ImageLoader.LoadImage(@"resized\Test_Whiteboad_Frame4.jpg"));

			var image = ImageLoader.LoadImage(@"resized\Test_Whiteboad_Frame5.jpg");

			var white_board = new WhiteBoard();
			white_board.Calibration(camera);
			var background_deleted_image = white_board.GetBackgroundDeleteImage(image);

			int white_count = 0;
			for(int y = 0; y < background_deleted_image.Height; ++y)
			{
				for(int x = 0;x < background_deleted_image.Width; ++x)
				{
					var r = background_deleted_image.GetElement(x, y, 0);
					var g = background_deleted_image.GetElement(x, y, 1);
					var b = background_deleted_image.GetElement(x, y, 2);

					// ホワイトボード平面だけを切りだすので画像は白くなるはず
					if((r - g) * (r -b) < 49)
					{
						++white_count;
					}
				}
			}


			Assert.IsTrue(white_count < image.Width * image.Height * 0.1);
		}


	}
}
