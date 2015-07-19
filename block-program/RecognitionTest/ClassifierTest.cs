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
            var algorithm = new SADAlgorithm();

            Clustering(classifier, algorithm, Resources.PatternLED0, Command.LED, 0);
            Clustering(classifier, algorithm, Resources.PatternLED1, Command.LED, 1);
            Clustering(classifier, algorithm, Resources.PatternMoveBackward1, Command.Move, -1);
            Clustering(classifier, algorithm, Resources.PatternMoveForward1, Command.Move, 1);
            Clustering(classifier, algorithm, Resources.PatternRotateCCW, Command.Rotate, -1);
            Clustering(classifier, algorithm, Resources.PatternRotateCW, Command.Rotate, 1);
            Clustering(classifier, algorithm, Resources.PatternMicroSwitch, Command.MicroSwitch, null);
            Clustering(classifier, algorithm, Resources.PatternPSD, Command.PSD, null);
            Clustering(classifier, algorithm, Resources.PatternStart, Command.Start, null);
            Clustering(classifier, algorithm, Resources.PatternEnd, Command.End, null);
        }

        private void Clustering(
            Classifier classifier,
            IPatternMatchingAlgorithm algorithm,
            Bitmap pattern,
            Command command,
            int? parameter0
        )
        {
            IImage image = new BitmapImage(pattern);
            IBlock block = classifier.Clustering(image, algorithm);
            Assert.AreEqual(command, block.CommandIdentification);
            if (block.Parameter.ValueLength() == 1)
            {
                Assert.AreEqual(block.Parameter.Value(0), parameter0);
            }
        }
    }
}
