using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myxini.Recognition
{
    public class Script
    {
        private IList<Routine> list_routine = new List<Routine>();

        /// <summary>
        /// 直前に操作した Routine
        /// </summary>
        private Routine just_before_routine;

        public Script()
        {
            Routines = list_routine;
        }

        public IEnumerable<Routine> Routines { get; private set; }

       /// <summary>
        /// trigger がトリガである Routine に instruction を追加する
        /// </summary>
        /// <param name="instruction"></param>
        /// <param name="trigger"></param>
        public void Add(InstructionBlock instruction, ControlBlock trigger)
        {
            // とりあえず Routine を追加してみる
            Add(trigger);
            // 今追加した Routine に instruction を追加
            Add(instruction, just_before_routine);
        }

        /// <summary>
        /// 直前に操作した Routine に instruction を追加する
        /// </summary>
        /// <param name="instruction"></param>
        public void Add(InstructionBlock instruction)
        {
            // そもそも操作していない
            if (just_before_routine == null)
            {
                throw new InvalidOperationException(
                    "Attempt to add an instruction before adding any Routine"
                );
            }
            // 直前に操作した Routine に追加
            Add(instruction, just_before_routine);
        }

        /// <summary>
        /// trigger がトリガである Routine がない場合，追加する．
        /// </summary>
        /// <param name="trigger"></param>
        public void Add(ControlBlock trigger)
        {
            Routine existing_routine;
            // trigger が list_routine にすでに存在する
            if ((existing_routine = list_routine.
                FirstOrDefault(routine => routine.Trigger == trigger)) != null)
            {
                just_before_routine = existing_routine;
                return;
            }
            Routine new_routine = new Routine(trigger);
            list_routine.Add(new_routine);
            just_before_routine = new_routine;
        }

        public override string ToString()
        {
            string s = "[Script]" + Environment.NewLine;
            foreach (Routine routine in Routines)
            {
                s += "  " + routine.ToString() + Environment.NewLine;
            }
            return s;
        }

        /// <summary>
        /// routine の末尾に instruction を追加する
        /// </summary>
        /// <param name="instruction"></param>
        /// <param name="just_before_routine"></param>
        private void Add(InstructionBlock instruction, Routine routine)
        {
            routine.Add(instruction);
        }
    }
}
