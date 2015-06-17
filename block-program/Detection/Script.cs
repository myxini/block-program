using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myxini.Recognition
{
    public class Script
    {
        private List<Routine> list_triger = new List<Routine>();

        public Script()
        {
            Triggers = list_triger;
        }

        public IEnumerable<Routine> Triggers { get; private set; }

    }
}
