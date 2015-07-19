using System;
using Microsoft.Kinect;

namespace Myxini.Recognition.Image
{
	public class Kinect : ICamera
	{
		public Kinect(int index = 0)
		{
			if (KinectSensor.KinectSensors.Count <= index || index < 0)
			{
				throw new InvalidOperationException();
			}

			this.Sensor = KinectSensor.KinectSensors[index];

			this.Sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
			this.Sensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);

			this.Sensor.ColorFrameReady += this.OnUpdateColorImage;
			this.Sensor.DepthFrameReady += this.OnUpdateDepthImage;

			this.Sensor.Start();
		}

		public Kinect(KinectSensor kinect)
		{
			this.Sensor = kinect;
			if(this.Sensor.IsRunning)
			{
				this.Sensor.Stop();
			}
			
			this.Sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
			this.Sensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);

			this.Sensor.ColorFrameReady += this.OnUpdateColorImage;
			this.Sensor.DepthFrameReady += this.OnUpdateDepthImage;
			
			this.Sensor.Start();
		}

		public static KinectSensor enumerateKinect(Func<KinectSensor, bool> function)
		{
			foreach(var kinect in KinectSensor.KinectSensors)
			{
				if(function(kinect))
				{
					return kinect;
				}
			}

			return null;
		}

		public IImage Capture()
		{
			return this.Image;
		}

		public void OnUpdateColorImage(Object sender, ColorImageFrameReadyEventArgs e)
		{
			using (var img = e.OpenColorImageFrame())
			{
				if (img == null)
				{
					return;
				}
				
				var raw_img = img.GetRawPixelData();

				this.ColorInputImage = new ColorImage(raw_img, img.Width, img.Height, img.BytesPerPixel);
			}

			if (this.ColorInputImage != null && this.DepthInputImage != null)
			{
				this.Image = new KinectImage(this.ColorInputImage, this.DepthInputImage);
			}
		}

		public void OnUpdateDepthImage(Object sender, DepthImageFrameReadyEventArgs e)
		{
			using (var img = e.OpenDepthImageFrame())
			{
				if (img == null)
				{
					return;
				}

				var raw_img = img.GetRawPixelData();


				this.DepthInputImage = new DepthImage(raw_img, this.Sensor.ColorStream.FrameWidth, this.Sensor.ColorStream.FrameHeight);
			}

			if(this.ColorInputImage != null && this.DepthInputImage != null)
			{
				this.Image = new KinectImage(this.ColorInputImage, this.DepthInputImage);
			}
		}

		public bool IsOpened
		{
			get { return this.Image != null; }
		}

		private KinectSensor Sensor;
		private KinectImage Image;
		private ColorImage ColorInputImage;
		private DepthImage DepthInputImage;
	}
}
