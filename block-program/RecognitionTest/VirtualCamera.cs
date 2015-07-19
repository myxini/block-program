using System;
using System.Collections.Generic;

using Myxini.Recognition.Image;

namespace RecognitionTest
{
	/// <summary>
	/// デバッグ用仮想カメラです．
	/// AddFrameで追加した画像をCaptureで取得します．
	/// AddFrameで追加した画像は自動でループします
	/// </summary>
	class VirtualCamera : ICamera
	{
		public VirtualCamera()
		{
			// do nothing
		}

		public void AddFrame(IImage image)
		{
			this.Frames.Add(image);
		}

		public IImage Capture()
		{
			++CurrentPosition;
			if(this.Frames.Count == CurrentPosition)
			{
				CurrentPosition = 0;
			}

			return this.Frames[this.CurrentPosition];
		}

		public bool IsOpened
		{
			get { return true; }
		}

		private List<IImage> Frames = new List<IImage>();
		private int CurrentPosition = -1;
	}
}
