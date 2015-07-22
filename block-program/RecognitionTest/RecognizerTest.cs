using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Myxini.Recognition;
using Myxini.Recognition.Image;

namespace RecognitionTest
{
	/// <summary>
	/// RecognizerTest の概要の説明
	/// </summary>
	[TestClass]
	public class RecognizerTest
	{
		private static System.Drawing.Bitmap Color;
		private static System.Drawing.Bitmap Depth;
		private static byte[] ColorPixels;
		private static short[] DepthPixels;

		public RecognizerTest()
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

		[ClassInitialize]
		[DeploymentItem(@"$(SolutionDir)Resource", "$(TargetDir)Resource")]
		public static void IntializeThisTest(TestContext context)
		{
			Color = new System.Drawing.Bitmap(@".\Resource\background_deleted_color.png");
			Depth = new System.Drawing.Bitmap(@".\Resource\background_deleted_depth.png");

			ColorPixels = new byte[Color.Width * Color.Height * 3];

			for (int y = 0; y < Color.Height; ++y)
			{
				for (int x = 0; x < Color.Width; ++x)
				{
					var color = Color.GetPixel(x, y);
					ColorPixels[(y * Color.Width + x) * 3 + 0] = color.B;
					ColorPixels[(y * Color.Width + x) * 3 + 1] = color.G;
					ColorPixels[(y * Color.Width + x) * 3 + 2] = color.R;
				}
			}

			DepthPixels = new short[Depth.Width * Depth.Height];
			for (int y = 0; y < Color.Height; ++y)
			{
				for (int x = 0; x < Color.Width; ++x)
				{
					var color = Color.GetPixel(x, y);
					DepthPixels[(y * Color.Width + x) + 0] = color.B;
				}
			}
		}

		[ClassCleanup]
		public static void CleanupThisTest()
		{
			Color.Dispose();
			Depth.Dispose();
		}


		[TestMethod]
		public void RecognitionTest()
		{
			var color = new ColorImage(ColorPixels, Color.Width, Color.Height);
			var depth = new DepthImage(DepthPixels, Depth.Width, Depth.Height);

			var image = new KinectImage(color, depth);

			var recognizer = new Recognizer();
			var script = recognizer.Recognition(image);
		}
	}
}
