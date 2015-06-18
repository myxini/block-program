using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Myxini.Communication;
using Myxini.Recognition;

namespace CommunicationTest
{
    [TestClass]
    public class PacketTest
    {
        [TestMethod]
        public void RotateCommandTest()
        {
            var service = new CommunicationService();
            var script = new Script();
            service.Run(script);
        }
    }
}
