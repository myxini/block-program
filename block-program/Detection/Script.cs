using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Myxini.Recognition
{
    public class Script
    {
        private List<Trigger> list_triger = new List<Trigger>();

        public Script()
        {
            Triggers = list_triger;
        }

        public IEnumerable<Trigger> Triggers { get; private set; }

    }
}
