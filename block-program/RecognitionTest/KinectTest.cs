using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Myxini.Recognition.Image;
using Myxini.Recognition.Raw;

namespace Myxini.Recognition.RecognitionTest
{
	/// <summary>
	/// KinectTest の概要の説明
	/// </summary>
	[TestClass]
	public class KinectTest
	{
		public KinectTest()
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
		public void OpenTest()
		{
			 new Kinect(0);
		}

		[TestMethod()]
		[ExpectedException(typeof(InvalidOperationException))]
		public void CaptureFailedTest()
		{
			new Kinect(-1);
		}

		[TestMethod, Timeout(10000)]
		public void CaptureTest()
		{
			var kinect = new Kinect(0);

			IImage image = null;
			
			do
			{
				image = kinect.Capture();
			} while (image == null);

			Assert.IsNotNull(image);
			Assert.AreEqual(image.Channel, 4);
			Assert.AreEqual(image.Width, 640);
			Assert.AreEqual(image.Height, 480);
			Assert.AreEqual(image.IsRegionOfImage, false);
		}
	}
}
