using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Myxini.Communication;
using Myxini.Communication.Robot;
using Myxini.Recognition;

namespace CommunicationTest
{
    [TestClass]
    public class PacketTest
    {
        private TestContext testContextInstance;
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
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

        [TestMethod]
        public void RotateCommandTest()
        {
        }

        [TestMethod]
        public void SerialOpenTest()
        {
            var ports = CommunicationService.GetAvailablePorts();
            CommunicationService service = null;
            if (ports.Length == 1)
            {
                service = new CommunicationService(ports[0]);
                service.Run(new Script());
            }
            else
            {
                foreach (var port in ports)
                {
                    service = new CommunicationService(port);
                    service.Run(new Script());
                    if (service.IsRunning)
                    {
                        break;
                    }
                }
            }
            Assert.IsTrue(service.IsRunning);
            service.Stop();
=======
            var service = new CommunicationService("4");
            var script = new Script();
            service.Run(script);
>>>>>>> a91b32cff28944175dc22c95c07d19c01b3f1682
        }
    }
}
