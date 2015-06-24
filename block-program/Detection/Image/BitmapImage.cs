﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myxini.Recognition.Image
{
    class BitmapImage : IImage
    {
        private Bitmap bitmap;

        public BitmapImage(Bitmap bitmap)
        {
            this.bitmap = bitmap;
            BoundingBox = new Raw.Rectangle(0, 0, bitmap.Height, bitmap.Width);
        }

        public int GetElement(int x, int y, int channel)
        {
            Color pixel = bitmap.GetPixel(x, y);
            switch (channel)
            {
                case 0:
                    return pixel.R;
                case 1:
                    return pixel.G;
                case 2:
                    return pixel.B;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public IImage RegionOfImage(int x, int y, int width, int height)
        {
            Rectangle rect = new Rectangle(x, y, x + width, y + height);
            return new BitmapImage(bitmap.Clone(rect, bitmap.PixelFormat));
        }

        public IImage RegionOfImage(Raw.Rectangle region)
        {
            return RegionOfImage(region.X, region.Y, region.Width, region.Height);
        }

        public IImage Clone()
        {
            return new BitmapImage(bitmap);
        }

        public int Channel
        {
            get
            {
                return 3;
            }
        }

        public Raw.Rectangle BoundingBox { get; private set; }

        public int Width
        {
            get
            {
                return BoundingBox.Width;
            }
        }

        public int Height
        {
            get
            {
                return BoundingBox.Height;
            }
        }

        public bool IsRegionOfImage
        {
            get { throw new NotImplementedException(); }
        }
    }
}
