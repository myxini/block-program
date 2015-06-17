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

        public Script()
        {
            Routines = list_routine;
        }

        public IEnumerable<Routine> Routines { get; private set; }

        public void AddRoutine(Routine routine)
        {
            list_routine.Add(routine);
        }
    }
}
