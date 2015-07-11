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
			var kinect = new Kinect(0);
		}

		[TestMethod]
		public void CaptureTest()
		{
			var kinect = new Kinect(0);
			var image = kinect.Capture();

			Assert.Equals(image.Channel, 4);
			Assert.Equals(image.Width, 640);
			Assert.Equals(image.Height, 480);
			Assert.Equals(image.IsRegionOfImage, false);
		}
	}
}
