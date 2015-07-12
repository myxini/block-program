using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Myxini.Recognition;
using Myxini.Recognition.Image;
using Myxini.Recognition.Properties;
using System.Drawing;

namespace RecognitionTest
{
    [TestClass]
    public class ClassifierTest
    {
        [TestMethod]
        public void ClusteringTest()
        {
            var classifier = new Classifier();

            Clustering(classifier, Resources.PatternLED0, Command.LED, 0);
            Clustering(classifier, Resources.PatternLED1, Command.LED, 1);
            Clustering(classifier, Resources.PatternMoveBackward1, Command.Move, -1);
            Clustering(classifier, Resources.PatternMoveForward1, Command.Move, 1);
            Clustering(classifier, Resources.PatternRotateCCW, Command.Rotate, -1);
            Clustering(classifier, Resources.PatternRotateCW, Command.Rotate, 1);
            Clustering(classifier, Resources.PatternMicroSwitch, Command.MicroSwitch, null);
            Clustering(classifier, Resources.PatternPSD, Command.PSD, null);
            Clustering(classifier, Resources.PatternStart, Command.Start, null);
            Clustering(classifier, Resources.PatternEnd, Command.End, null);
        }

        private void Clustering(Classifier classifier, Bitmap pattern, Command command, int? parameter0)
        {
            IImage image = new BitmapImage(pattern);
            IBlock block = classifier.Clustering(image);
            Assert.AreEqual(command, block.CommandIdentification);
            if (block.Parameter.ValueLength() == 1)
            {
                Assert.AreEqual(block.Parameter.Value(0), parameter0);
            }
        }
    }
}
