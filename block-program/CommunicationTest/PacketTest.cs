﻿using System;
using System.Threading;
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
        public void CommandTest()
        {
            string[] pots = CommunicationService.GetAvailablePorts();
            CommunicationService serv;
            if (pots.Length > 0)
            {
                serv = new CommunicationService("COM10");
            }
            else
            {
                return;
            }
            Script testScript = new Script();
            Routine testRoutine = new Routine(
                new ControlBlock(
                    Myxini.Recognition.Command.Start,
                    new BlockParameter())
            );
            // 前進低速 :0x 06 09 01 01 4C 01 42
            testRoutine.Append(
                new InstructionBlock(
                    Myxini.Recognition.Command.Move,
                    new BlockParameter(new int[1] { 1 })
                )
            );
            // 右旋回 :0x 06 09 01 02 80 5A D6
            testRoutine.Append(
                new InstructionBlock(
                    Myxini.Recognition.Command.Rotate,
                    new BlockParameter(new int[1]{1})
                )
            );
            // LED点灯 :0x 06 09 01 03 00 01 0C
            testRoutine.Append(
                new InstructionBlock(
                    Myxini.Recognition.Command.LED,
                    new BlockParameter(new int[1] { 1 })
                )
            );
            // 左旋回 :0x 06 09 01 02 80 A6 2A
            testRoutine.Append(
                new InstructionBlock(
                    Myxini.Recognition.Command.Rotate,
                    new BlockParameter(new int[1] { -1 })
                )
            );
            // LED消灯 :0x 06 09 01 03 00 00 0D
            testRoutine.Append(
                new InstructionBlock(
                    Myxini.Recognition.Command.LED,
                    new BlockParameter(new int[1] { 0 })
                )
            );
            // 後退低速 :0x 06 09 01 01 B4 01 BA
            testRoutine.Append(
                new InstructionBlock(
                    Myxini.Recognition.Command.Move,
                    new BlockParameter(new int[1] { -1 })
                )
            );
            testScript.AddRoutine(testRoutine);
            var serv_private = new PrivateObject(serv);
            serv.Run(testScript);
            Thread.Sleep(5000);
        }

        [TestMethod]
        public void SendAPacketTest()
        {
            string[] pots = CommunicationService.GetAvailablePorts();
            CommunicationService serv;
            if (pots.Length > 0)
            {
                serv = new CommunicationService("COM4");
            }
            else
            {
                return;
            }
            Script testScript = new Script();
            Routine testRoutine = new Routine(
                new ControlBlock(
                    Myxini.Recognition.Command.Start,
                    new BlockParameter())
            );
            // 前進低速 :0x 06 09 01 01 26 01 28
            testRoutine.Append(new InstructionBlock(
                    Myxini.Recognition.Command.Move,
                    new BlockParameter(new int[1] { 1 })
                ));
            testScript.AddRoutine(testRoutine);
            var serv_private = new PrivateObject(serv);
            serv.Run(testScript);
            Thread.Sleep(100000);
        }

        [TestMethod]
        public void SerialOpenTest()
        {
            var ports = CommunicationService.GetAvailablePorts();
            CommunicationService service = null;
            if(ports.Length == 0)
            {
                return;
            }
            else if (ports.Length == 1)
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
        }
    }
}
