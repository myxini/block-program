﻿using System;
using Microsoft.Kinect;

namespace Myxini.Recognition.Image
{
	public class Kinect : ICamera
	{
		public Kinect(int index = 0)
		{
			if(KinectSensor.KinectSensors.Count <= index)
			{
				throw new InvalidOperationException();
			}

			this.Sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
			this.Sensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);

			this.Sensor.ColorFrameReady += this.OnUpdateColorImage;
			this.Sensor.DepthFrameReady += this.OnUpdateDepthImage;
		}

		public Kinect(KinectSensor kinect)
		{
			this.Sensor = kinect;
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

				this.ColorInputImage = new ColorImage(raw_img, this.Sensor.DepthStream.FrameWidth, this.Sensor.DepthStream.FrameHeight);
			}

			this.Image = new KinectImage(this.ColorInputImage, this.DepthInputImage);
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

			this.Image = new KinectImage(this.ColorInputImage, this.DepthInputImage);
		}

		private KinectSensor Sensor;
		private KinectImage Image;
		private ColorImage ColorInputImage;
		private DepthImage DepthInputImage;
	}
}