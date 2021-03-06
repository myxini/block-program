﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myxini.Recognition
{
    public class Routine
    {
        private IList<InstructionBlock> list_instruction = new List<InstructionBlock>();

        public Routine(ControlBlock trigger)
        {
            Instructions = list_instruction;
            Trigger = trigger;
        }

        public IEnumerable<InstructionBlock> Instructions { get; private set; }
        public ControlBlock Trigger { get; private set; }

        public void Add(InstructionBlock instruction)
        {
            list_instruction.Add(instruction);
        }

        public override string ToString()
        {
            string s = "[Routine]" + Environment.NewLine;
            if (Trigger != null)
            {
                s += "    Trigger:" + Environment.NewLine;
                s += Trigger.ToString() + Environment.NewLine;
            }
            if (Instructions.Count() > 0)
            {
                s += "    Instructions:" + Environment.NewLine;
            }
            foreach (InstructionBlock instruction in Instructions)
            {
                s += instruction.ToString() + Environment.NewLine;
            }
            return s;
        }
    }
}
