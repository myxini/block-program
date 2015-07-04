using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myxini.Communication
{
    class PacketBuilder
    {
        public byte RobotID { get; set; }

        public PacketBuilder()
        {
            this.RobotID = 0;
        }

        private Robot.Command Build(Myxini.Recognition.InstructionBlock block)
        {
            switch(block.CommandIdentification)
            {
                case Recognition.Command.Move:
                    return this.BuildMoveCommand(block);
                case Recognition.Command.Rotate:
                    return this.BuildRotateCommand(block);
                case Recognition.Command.LED:
                    return this.BuildLEDCommand(block);
                case Recognition.Command.End:
                    return null;
                default:
                    throw new InvalidOperationException("CommandIdentification is invalid");
            }
        }

        private Robot.StraightCommand BuildMoveCommand(Myxini.Recognition.InstructionBlock block)
        {
            float velocity = .0f;
            float time = 1.0f;
            switch(block.Parameter.Value(0))
            {
                case 1:
                    velocity = 0.3f;
                    break;
                case 2:
                    velocity = 0.6f;
                    break;
                case 3:
                    velocity = 0.9f;
                    break;
                case -1:
                    velocity = -0.3f;
                    break;
                case -2:
                    velocity = -0.6f;
                    break;
                case -3:
                    velocity = -0.9f;
                    break;
            }
            return new Robot.StraightCommand()
            {
                RobotID = this.RobotID,
                Velocity =  velocity,
                Time = time
            };
        }

        private Robot.RotateCommand BuildRotateCommand(Recognition.InstructionBlock block)
        {
            float velocity = 0.5f;
            float angle = .0f;
            switch(block.Parameter.Value(0))
            {
                case -1:
                    angle = -90f;
                    break;
                case 1:
                    angle = 90f;
                    break;
            }
            return new Robot.RotateCommand()
            {
                RobotID = this.RobotID,
                AngularVelocity = velocity,
                Angle = angle
            };
        }

        private Robot.LEDCommand BuildLEDCommand(Recognition.InstructionBlock block)
        {
            return new Robot.LEDCommand()
            {
                RobotID = this.RobotID,
                LEDNumber = 0,
                Switch = (byte)block.Parameter.Value(0)
            };
        }

        public Robot.CommandList Build(IEnumerable<Myxini.Recognition.InstructionBlock> instrucions)
        {
            var dst = new Robot.CommandList();
            foreach(var block in instrucions)
            {
                try
                {
                    var command = this.Build(block);
                    if (command != null)
                    {
                        dst.Add(command);
                    }
                }
                catch (Exception)
                {
                    ;
                }
            }
            return dst;
        }
    }
}
